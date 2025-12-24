using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_RPG
{
    /// <summary>
    /// Heroクラス : プレイヤーが操作するキャラクター
    /// </summary>
    internal class Hero : Character, ITurn, ICallback
    {
        public char Gender { get; private set; }
        public string Profession { get; private set; }

        public Hero(string name, uint hp, uint mp, uint speed, char gender, string profession)
            : base(name, hp, mp, speed)
        {
            Gender = gender;
            Profession = profession;
        }

        /// <summary>
        /// 自分のターン
        /// </summary>
        public async Task YourTurn()
        {
            if (!string.IsNullOrEmpty(CoveringName))
            {
                string targetName = CoveringName;

                // 庇っていた相手のフラグも消す
                if (CharacterList.Heroes.ContainsKey(targetName))
                {
                    CharacterList.Heroes[targetName].CoveredByName = "";
                }

                // 自分のフラグを消す
                this.CoveringName = "";
            }

            this.Block = false;

            TraceLog.Write($"{Name}: ターンが移った");

            if (this.LastStand)
            {
                this.LastStandCount--;
                if (this.LastStandCount == 0)
                {
                    this.LastStand = false;
                    TraceLog.Write($"{Name}: ラストスタンド状態が解除された");
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

            if (this.NotAlive)
            {
                TraceLog.Write($"{Name}: 戦闘不能である");
                return;
            }

            if (this.SpellCasting)
            {
                //魔法詠唱中
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
                // ケアルラアニメーション
                else if (delayedAction == "ケアルラ")
                {
                    var targetHeroes = CharacterList.Heroes.Values.Where(h => !h.NotAlive).ToList();
                    var targetNames = targetHeroes.Select(h => h.Name).ToList();
                    GameMaster.BattleField?.AnimateAllHeal(this.Name, targetNames);
                }
                // ジャンプアニメーション
                else if (delayedAction == "ジャンプ") GameMaster.BattleField?.AnimateAttack(this.Name, finalTargetName);

                else GameMaster.BattleField?.AnimateMiss(this.Name);
                // 魔法を発動する
                var tar = CharacterList.AllChars[this.delayedTarget];
                await tar.CastSpellOf(this.delayedAction, this);

                // 次のターンで遅延実行はしないので状態クリア
                this.delayedAction = "";
                this.delayedTarget = "";

                //@@バトルキューに挿入@@
                BattleQueue.Enq(this, this.Speed);
            }
            else
            {
                //@@画面入力@@
                GameMaster.BattleField?.ActivateCommand(this);
            }
        }
        public async Task Callback(string action,string target)
        {
            
            switch (action)
            {
                case "攻撃魔法":
                case "回復魔法":
                case "ケアルラ":
                case "毒魔法":
                case "睡眠魔法":
                    // 魔法の場合は遅延実行として保存
                    TraceLog.Write($"{Name}: {target}に{action}を唱えた");

                    // ブラッシュアップ　詠唱アニメーション
                    GameMaster.BattleField?.AnimateChanting(this.Name);

                    this.delayedAction = action;
                    this.delayedTarget = target;
                    break;
                case "ジャンプ":
                    // ジャンプの場合も遅延実行として保存
                    TraceLog.Write($"{Name}: ジャンプした");

                    // ジャンプアニメーション
                    GameMaster.BattleField?.AnimateJump(this.Name);
                    this.delayedAction = action;
                    this.delayedTarget = target;
                    this.Jump = true;
                    break;

                case "攻撃":
                    // ターゲットに対して攻撃
                    TraceLog.Write($"{Name}: {target}を{action}した");

                    // ブラッシュアップ　攻撃アニメーション
                    GameMaster.BattleField?.AnimateAttack(this.Name, target);

                    await CharacterList.AllChars[target].AttackedBy(this);
                    break;

                case "防御":
                    TraceLog.Write($"{Name}: {action} している ({target})");

                    this.Block = true;

                    // 防御アニメーション
                    GameMaster.BattleField?.AnimateDefense(this.Name);

                    break;

                case "庇う":
                    TraceLog.Write($"{Name}: {target}を{action} 態勢に入った");
                    // 自分がターゲットを庇う
                    this.CoveringName = target;

                    // ターゲット側にも「庇われている」と記録する
                    if (CharacterList.Heroes.ContainsKey(target))
                    {
                        CharacterList.Heroes[target].CoveredByName = this.Name;
                    }

                    // 庇うアニメーション
                    GameMaster.BattleField?.AnimateCover(this.Name, target);

                    break;

                case "ラストスタンド":
                    TraceLog.Write($"{Name}: {action} を発動した");

                    this.LastStand = true;
                    this.LastStandCount = 5;
                    this.LastStandLimit = 0;

                    // ラストスタンドアニメーション
                    GameMaster.BattleField?.AnimateLastStand(this.Name);

                    break;
            }

            // @@バトルキューに挿入@@
            BattleQueue.Enq(this,this.Speed);
        }
    }
}
