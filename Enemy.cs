using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OOP_RPG
{
    internal class Enemy : Character, ITurn
    {
        public string Kind { get; }
        private static IRand _rand = new Rand();


        public Enemy(string name, uint hp, uint mp, uint speed, string kind)
            : base(name, hp, mp, speed)
        {
            this.Kind = kind;
        }

        /// <summary>
        /// ユニットテスト用のRandモックを注入
        /// </summary>
        /// <param name="mock"></param>
        public static void InjectRandMock(IRand mock)
        {
            _rand = mock;
        }

        /// <summary>
        /// 今ターンのアクションとターゲットの決定
        /// </summary>
        /// <returns>(次ターンのアクション、次ターンのターゲット)</returns>
        protected virtual (string act, string tar) MakeDecision()
        {
            // 自分のMPで使えるコマンドだけを抽出してシャッフルする
            var validCommands = CommandList.Commands
                .Where(c => this.Mp >= c.Value.UseMp) // MPが足りるものだけ
                .Where(c => this.CanUseCommand(c.Key))
                .OrderBy(_ => _rand.Next(50))         // ランダムに並べる
                .Select(c => c.Key)
                .ToList();

            // コマンドを上から順に試して、ターゲットが見つかれば即決定
            foreach (var act in validCommands)
            {
                var cmdInfo = CommandList.Commands[act];
                string[] candidates = []; // 候補者リスト

                switch (cmdInfo.Target)
                {
                    case "敵方":
                        candidates = CharacterList.Heroes.Values // 生きている
                            .Where(h => !h.NotAlive)
                            .Select(h => h.Name)
                            .ToArray();
                        break;

                    case "味方":
                        if (act == "庇う")
                        {
                            // 生きている && 庇う関係にない && 自分以外
                            candidates = CharacterList.Enemies.Values
                                .Where(e => !e.NotAlive && !e.IsCoverState && e.Name != this.Name)
                                .Select(e => e.Name)
                                .ToArray();
                        }
                        else if (act == "回復魔法")
                        {
                            // 生きている (回復魔法を死体にかけない場合)
                            candidates = CharacterList.Enemies.Values
                                .Where(e => !e.NotAlive)
                                .Select(e => e.Name)
                                .ToArray();
                        }
                        else
                        {
                            // その他の補助魔法など：生きている味方
                            candidates = CharacterList.Enemies.Values
                                .Where(e => !e.NotAlive)
                                .Select(e => e.Name)
                                .ToArray();
                        }
                        break;

                    case "自分":
                        // 自分自身を対象にする場合
                        candidates = [this.Name];
                        break;
                }

                // 候補者が一人でもいれば、その中からランダムに選んで決定！
                if (candidates.Length > 0)
                {
                    string tar = candidates[_rand.Next(candidates.Length)];
                    return (act, tar);
                }
            }
            // 生きている勇者を適当に殴る
            string fallbackTar = CharacterList.Heroes.Values
                .Where(h => !h.NotAlive)
                .Select(h => h.Name)
                .FirstOrDefault() ?? ""; // 全滅してたら空文字

            return ("攻撃", fallbackTar);
        }

        public async Task YourTurn()
        {
            TraceLog.Write($"{Name}: ターンが移った");

            // 庇うflagの初期化
            if (!string.IsNullOrEmpty(CoveringName))
            {
                string targetName = CoveringName;

                // 庇っていた相手のフラグも消す
                if (CharacterList.Enemies.ContainsKey(targetName))
                {
                    CharacterList.Enemies[targetName].CoveredByName = "";
                }

                // 自分のフラグを消す
                this.CoveringName = "";
            }
            this.Counter = false;
            this.Block = false;

            if (this.NotAlive)
            {
                TraceLog.Write($"{Name}: 戦闘不能である");
                return;
            }


            if (this.Sleeping)
            {
                await SleepingState();
                if (this.Sleeping)
                {
                    TraceLog.Write($"{Name}: 睡眠状態である");
                    BattleQueue.Enq(this, this.Speed);
                    return;
                }
            }

            if (this.Poisoned)
            {
                await PoisonedState();
                if (GameMaster.BattleField != null)
                {
                    if (this.Hp == 0)
                    {
                        TraceLog.Write($"{Name}: 戦闘不能である");
                        return;
                    }
                }
            }

            if (this.SpellCasting)
            {
                // 魔法詠唱中
                TraceLog.Write($"{Name}: 魔法詠唱中である");

                // 実体取得
                string finalTargetName = this.delayedTarget;
                Character? finalTargetChara = CharacterList.AllChars[finalTargetName];
                if (finalTargetName == null) return;

                if (!string.IsNullOrEmpty(finalTargetChara.CoveredByName))
                {
                    // 庇ってくれている人の名前を取得
                    string covererName = finalTargetChara.CoveredByName;

                    // その人の実体が存在すれば、ターゲットをその人に変更する
                    if (CharacterList.AllChars.ContainsKey(covererName))
                    {
                        finalTargetName = covererName;
                        finalTargetChara = CharacterList.AllChars[covererName];

                        TraceLog.Write($"{Name}: {this.delayedTarget}への魔法は{finalTargetName}が庇った！");
                    }
                }

                TraceLog.Write($"{Name}: {finalTargetName}への{this.delayedAction}が発動");


                // 魔法攻撃アニメーション
                if (delayedAction == "攻撃魔法") GameMaster.BattleField?.AnimateMagic(this.Name, this.delayedTarget);
                // 回復魔法アニメーション
                else if (delayedAction == "回復魔法") GameMaster.BattleField?.AnimateHeal(this.Name, this.delayedTarget);
                // 毒魔法
                else if (delayedAction == "毒魔法")
                {
                    if (!finalTargetChara.Poisoned && !finalTargetChara.Sleeping)
                    {
                        GameMaster.BattleField?.AnimatePoisonMagic(this.Name, finalTargetName);
                    }
                    else
                    {
                        GameMaster.BattleField?.AnimateMiss(this.Name);
                    }
                }
                // 睡眠魔法
                else if (delayedAction == "睡眠魔法")
                {
                    if (!finalTargetChara.Poisoned && !finalTargetChara.Sleeping)
                    {
                        GameMaster.BattleField?.AnimateSleepMagic(this.Name, finalTargetName);
                    }
                    else
                    {
                        GameMaster.BattleField?.AnimateMiss(this.Name);
                    }
                }
                else GameMaster.BattleField?.AnimateMiss(this.Name);

                // 魔法を発動する
                var tar = CharacterList.AllChars[this.delayedTarget];
                await tar.CastSpellOf(this.delayedAction, this);

                // 次のターンで遅延実行はしないので状態クリア
                this.delayedAction = "";
                this.delayedTarget = "";
            }
            else
            {
                var (act, tar) = MakeDecision();
                Debug.WriteLine($"[エネミーアクション] {Name}の選択: コマンド[{act}] ターゲット[{tar}]");

                switch (act)
                {
                    case "攻撃魔法":
                    case "回復魔法":
                    case "毒魔法":
                    case "睡眠魔法":
                        // 魔法詠唱中に遷移
                        TraceLog.Write($"{Name}: {tar}に{act}を唱えた");
                        this.delayedAction = act;
                        this.delayedTarget = tar;

                        // ブラッシュアップ　防御アニメーション
                        GameMaster.BattleField?.AnimateChanting(this.Name);

                        break;

                    case "攻撃":
                        // ターゲットに対して攻撃
                        TraceLog.Write($"{Name}: {tar}に{act}した");

                        // ブラッシュアップ　攻撃アニメーション
                        GameMaster.BattleField?.AnimateAttack(this.Name,tar);
                        await CharacterList.AllChars[tar].AttackedBy(this);

                        break;

                    case "防御":
                        TraceLog.Write($"{Name}: {tar}を{act}している");
                        this.Block = true;

                        // ブラッシュアップ　防御アニメーション
                        GameMaster.BattleField?.AnimateDefense(this.Name);

                        break;

                    case "庇う":
                        TraceLog.Write($"{Name}: {tar}を{act}態勢に入った");
                        // 自分がターゲットを庇う
                        this.CoveringName = tar;

                        // ターゲット側にも「庇われている」と記録する
                        if (CharacterList.Enemies.ContainsKey(tar))
                        {
                            CharacterList.Enemies[tar].CoveredByName = this.Name;
                        }

                        // ブラッシュアップ　庇うアニメーション
                        GameMaster.BattleField?.AnimateCover(this.Name,tar);

                        break;

                    case "溜め":
                        TraceLog.Write($"{Name}: 強力な攻撃の準備をしている");
                        GameMaster.BattleField?.AnimateSave(this.Name);
                        break;

                    case "見切り":
                        TraceLog.Write($"{Name}: 見切りの姿勢を取った");
                        this.Counter = true;
                        GameMaster.BattleField?.AnimateCounter(this.Name);
                        break;

                    case "斬鉄剣":
                        TraceLog.Write($"{Name}: 斬鉄剣を繰り出した！！");
                        // 生きている勇者全員を取得
                        var targetHeroes = CharacterList.Heroes.Values.Where(h => !h.NotAlive).ToList();
                        // 一応名前リストも作成
                        var targetNames = targetHeroes.Select(h => h.Name).ToList();
                        uint damage = 50;

                        // @@ 斬鉄剣アニメーションに修正予定
                        GameMaster.BattleField?.AnimateZantetsuken(this.Name);
                        
                        // 対象者全員に固定ダメージ
                        foreach (var hero in targetHeroes)
                        {
                            await　hero.ReceiveFixedDamage(damage, hero);
                        }
                        break;
                }
            }
            //@@バトルキューに挿入@@
            BattleQueue.Enq(this, this.Speed);
        }
    }
}
