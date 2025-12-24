using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_RPG
{
    /// <summary>
    /// Character抽象クラス
    /// </summary>
    internal abstract class Character : IBattle
    {
        public string Name { get; }
        public uint Hp { get; private set; }
        public uint Mp { get; private set; }
        public uint HpMax { get; private set; }
        public uint MpMax { get; private set; }
        public uint Speed { get; private set; }
        public bool Block { get; set; } // 防御しているのか

        public bool Normal
        {
            get
            {
                return Hp > 1 && !Sleeping;
            }
        }
        public bool NotAlive { get { return Hp == 0; } }
        public bool SpellCasting
        {
            get
            {
                // 遅延実行が魔法の時true
                    return (delayedAction != ""
                    && CommandList.Commands[delayedAction].Kind == "Magic");
            }
        }
        public bool Sleeping { get; protected set; } = false;
        public uint SleepingCount { get; protected set; }
        public bool Poisoned { get; protected set; } = false;
        public uint PoisonedCount { get; protected set; }

        //庇う判定プロパティ
        //誰を庇っているのか
        public string CoveringName { get; set; } = "";

        //誰に庇われているのか
        public string CoveredByName { get; set; } = "";

        // 庇う依存のターゲット判定
        public bool IsCoverState
        {
            get { return !string.IsNullOrEmpty(CoveringName) || !string.IsNullOrEmpty(CoveredByName); }
        }

        public bool Counter { get; protected set; } = false;
        public bool Jump { get; protected set; } = false;
        public uint JumpLimit { get; protected set; } = 3;
        
        public bool LastStand { get; protected set; } = false;
        public uint LastStandLimit { get; protected set; } = 1;
        public uint LastStandCount { get; protected set; } = 0;



        // 魔法のように次のターンで実行するアクションとターゲットを保存
        // delayedAction == ""なら次のターンは通常
        protected string delayedAction = "";
        protected string delayedTarget = "";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="hp">Hit Point</param>
        /// <param name="mp">Magic Point</param>
        /// <param name="speed">速さ</param>
        public Character(string name, uint hp, uint mp, uint speed)
        {
            Name = name;
            Hp = hp;
            Mp = mp;
            HpMax = hp;
            MpMax = mp;
            Speed = speed;
        }

        /// <summary>
        /// このキャラクターが特定のコマンドを使用できるか判定する
        /// </summary>
        public virtual bool CanUseCommand(string commandName)
        {

            var cmdInfo = CommandList.Commands[commandName];

            // ボス専用コマンドなら、通常のキャラクターは使用不可
            if (cmdInfo.IsBossOnly) return false;


            if (commandName == "毒魔法")
            {
                // 味方(Hero)の場合
                if (this is Hero hero)
                {
                    // 職業チェック
                    if (hero.Profession == "暗黒騎士" ||
                        hero.Profession == "忍者" ||
                        hero.Profession == "黒魔導士")
                    {
                        return true;
                    }
                }
                // 敵(Enemy)の場合
                else if (this is Enemy enemy)
                {
                    // 種族チェック
                    if (enemy.Kind == "黒魔術師")
                    {
                        return true;
                    }
                }
                return false; // 条件に合わなければ不可
            }

            if (commandName == "睡眠魔法")
            {
                // 味方(Hero)の場合
                if (this is Hero hero)
                {
                    // 職業チェック
                    if (hero.Profession == "忍者" ||
                        hero.Profession == "黒魔導士" ||
                        hero.Profession == "白魔導士")
                    {
                        return true;
                    }
                }
                // 敵(Enemy)の場合
                else if (this is Enemy enemy)
                {
                    // 種族チェック
                    if (enemy.Kind == "黒魔術師")
                    {
                        return true;
                    }
                }
                return false;
            }

            if (commandName == "ケアルラ")
            {
                // 味方(Hero)の場合
                if (this is Hero hero)
                {
                    // 職業チェック
                    if (hero.Profession == "白魔導士")
                    {
                        return true;
                    }
                }
                return false;
            }

            if (commandName == "ジャンプ")
            {
                // 味方(Hero)の場合
                if (this is Hero hero)
                {
                    // 職業チェック
                    if (hero.Profession == "竜騎士")
                    {
                        return true;
                    }
                }
                return false;
            }

            if (commandName == "ラストスタンド")
            {
                // 味方(Hero)の場合
                if (this is Hero hero)
                {
                    // 職業チェック
                    if (hero.Profession == "暗黒騎士" && hero.LastStandLimit > 0)
                    {
                        if (hero.Mp == 0)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            return true;
        }


        /// <summary>
        /// 相手からの攻撃を受ける
        /// </summary>
        /// <param name="byChar"></param>
        public async Task AttackedBy(Character byChar)
        {
            if (CoveredByName != "")
            {
                //庇ってくれているキャラを探す
                Character? coveringChar = null;
                if (CharacterList.AllChars.ContainsKey(CoveredByName))
                {
                    coveringChar = CharacterList.AllChars[CoveredByName];
                }

                // 見つかったら、身代わりになってもらう
                if (coveringChar != null)
                {
                    TraceLog.Write($"{Name}: {coveringChar.Name}が攻撃を庇った！");
                    await coveringChar.AttackedBy(byChar);
                    return;
                }

            }
            TraceLog.Write($"{Name}: {byChar.Name}から攻撃を受けた");            

            // 変数定義 ===========================
            uint damage = 1;
            // ダメージを受けるキャラ:Defender
            string DefenderName = Name;
            uint DefenderHp = Hp;
            uint DefenderMp = Mp;
            uint DefenderSpeed = Speed;
            // ダメージを与えるキャラ:Attacker
            string AttackerName = byChar.Name;
            uint AttackerHp = byChar.Hp;
            uint AttackerMp = byChar.Mp;
            uint AttackerSpeed = byChar.Speed;
            // =====================================
            if (byChar is Hero hero)
            {
                switch (hero.Profession) // ヒーローの職業
                {
                    case "暗黒騎士" or "ナイト" or "戦士":
                        damage = 8;
                        break;
                    case "竜騎士" or "モンク" or "忍者" or "侍":
                        damage = 10;
                        break;
                    case "白魔導士" or "学者":
                        damage = 5;
                        break;
                }
            }
            else if (byChar is Enemy enemy) //エネミーの種族
            {
                switch (enemy.Kind)
                {
                    case "スライム" or "ゴブリン":
                        damage = 8;
                        break;
                    case "つよつよもんす": // @@のちに実装
                        damage = 10;
                        break;
                    case "黒魔術師":
                        damage = 5;
                        break;
                    case "ボス":
                        damage = 30;
                        break;
                }
            }
            if (this.Jump)
            {
                TraceLog.Write($"ジャンプ中の{Name}には当たらない！");
                return;
            }
            if (Block)
            {
                TraceLog.Write($"{Name} :防御が発動！{damage}ダメージ→{damage-5}ダメージ");
                damage = damage - 5; // 防御していたらダメージを-5
            }

            if (this.Counter)
            {
                TraceLog.Write($"{Name}は{byChar.Name}の攻撃を見切った！");
                TraceLog.Write($"{byChar.Name}に{damage}のダメージを返す");

                // カウンター攻撃アニメーション (自分が相手を殴り返す)
                if (GameMaster.BattleField != null)
                {
                    GameMaster.BattleField.AnimateAttack(this.Name, byChar.Name);
                }

                // 攻撃者にダメージ適用
                await byChar.ReceiveFixedDamage((uint)damage, this);

                // 自分はダメージを受けないので、ここで終了
                return;
            }

            if (Sleeping)
            {
                Sleeping = false;
                SleepingCount = 0;
                TraceLog.Write($"{Name}: 睡眠が解除された。");
                if (GameMaster.BattleField != null) GameMaster.BattleField!.AnimateGetup(Name);
            }

            if (damage <= DefenderHp)
            {
                DefenderHp = DefenderHp - damage;
                TraceLog.Write($"{Name}は{damage}ダメージ受けた");

                if (DefenderHp == 0)
                {
                    if (LastStand)
                    {
                        TraceLog.Write($"{Name}: {Name}はラストスタンドを実行中。");
                        Hp = 1;
                    }
                    else
                    {
                        TraceLog.Write($"{Name}: {Name}は死亡した。");
                        ClearCoverState();
                    }
                }
                if (DefenderHp == 1)
                {
                    TraceLog.Write($"{Name}: {Name}は気絶した。");
                }

                Hp = DefenderHp;
                TraceLog.Write($"{Name}: {Hp}/{HpMax}");
            }
            else
            {
                if (LastStand)
                {
                    TraceLog.Write($"{Name}: {Name}はラストスタンドを実行中。");
                    Hp = 1;
                }
                else
                {
                    Hp = 0;
                    TraceLog.Write($"{Name}: {Name}は死亡した。");
                    TraceLog.Write($"{Name}: {Hp}/{HpMax}");
                    ClearCoverState();
                }
            }
            if (GameMaster.BattleField != null) await GameMaster.BattleField.AnimateReceive(Name, (int)damage);
            GameMaster.BattleField?.CheckBattleEnd();
            return;
        }

        /// <summary>
        /// 相手からの固定ダメージを受ける
        /// </summary>
        /// <param name="byChar"></param>
        public async Task ReceiveFixedDamage(uint damage,Character byChar)
        {
 
            // 変数定義 ===========================
            // ダメージを受けるキャラ:Defender
            string DefenderName = Name;
            uint DefenderHp = Hp;
            uint DefenderMp = Mp;
            uint DefenderSpeed = Speed;
            // ダメージを与えるキャラ:Attacker
            string AttackerName = byChar.Name;
            uint AttackerHp = byChar.Hp;
            uint AttackerMp = byChar.Mp;
            uint AttackerSpeed = byChar.Speed;
            // =====================================
            if (Block)
            {
                TraceLog.Write($"{Name} :防御が発動！{damage}ダメージ→{damage - 5}ダメージ");
                damage = damage - 5; // 防御していたらダメージを-5
            }

            if (this.Jump)
            {
                TraceLog.Write($"ジャンプ中の{Name}には当たらない！");
                return;
            }

            if (Sleeping)
            {
                Sleeping = false;
                SleepingCount = 0;
                TraceLog.Write($"{Name}: 睡眠が解除された。");
                if (GameMaster.BattleField != null) GameMaster.BattleField!.AnimateGetup(Name);
            }

            if (damage <= DefenderHp)
            {
                DefenderHp = DefenderHp - damage;
                TraceLog.Write($"{Name}は{damage}ダメージ受けた");

                if (DefenderHp == 0)
                {
                    if (LastStand)
                    {
                        TraceLog.Write($"{Name}: {Name}はラストスタンドを実行中。");
                        Hp = 1;
                    }
                    else
                    {
                        TraceLog.Write($"{Name}: {Name}は死亡した。");
                        ClearCoverState();
                    }
                }
                if (DefenderHp == 1)
                {
                    TraceLog.Write($"{Name}: {Name}は気絶した。");
                }

                Hp = DefenderHp;
                TraceLog.Write($"{Name}: {Hp}/{HpMax}");
            }
            else
            {
                if (LastStand)
                {
                    TraceLog.Write($"{Name}: {Name}はラストスタンドを実行中。");
                    Hp = 1;
                }
                else
                {
                    Hp = 0;
                    TraceLog.Write($"{Name}: {Name}は死亡した。");
                    TraceLog.Write($"{Name}: {Hp}/{HpMax}");
                    ClearCoverState();
                }
            }

            if (GameMaster.BattleField != null) await GameMaster.BattleField.AnimateReceive(Name, (int)damage);
            GameMaster.BattleField?.CheckBattleEnd();
            return;
        }


        /// <summary>
        /// 相手からの魔法を受ける
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="byChar"></param>
        public async Task CastSpellOf(string spell, Character byChar) // spellの中身は攻撃魔法or回復魔法
        {
            if (CoveredByName != "" && spell !="回復魔法")
            {
                //庇ってくれているキャラを探す
                Character? coveringChar = null;
                if (CharacterList.AllChars.ContainsKey(CoveredByName))
                {
                    coveringChar = CharacterList.AllChars[CoveredByName];
                }

                // 見つかったら、身代わりになってもらう
                if (coveringChar != null)
                {
                    TraceLog.Write($"{Name}: {coveringChar.Name}が攻撃を庇った！");
                    await coveringChar.CastSpellOf(spell,byChar);
                    return;
                }
            }
            // 変数定義 ===========================
            uint damage = 1;
            uint healPoint = 1;
            uint mpCost = 0;
            // ダメージを受けるキャラ:Defender
            string DefenderName = Name;
            uint DefenderHp = Hp;
            uint DefenderMp = Mp;
            uint DefenderSpeed = Speed;
            // ダメージを与えるキャラ:Attacker
            string AttackerName = byChar.Name;
            uint AttackerHp = byChar.Hp;
            uint AttackerMp = byChar.Mp;
            uint AttackerSpeed = byChar.Speed;
            // =====================================
            if     (spell == "攻撃魔法")   mpCost = 10;
            else if(spell == "回復魔法")   mpCost = 10;
            else if (spell == "ケアルラ") mpCost = 20;
            else if(spell == "毒魔法")     mpCost = 20;
            else if (spell == "睡眠魔法")  mpCost = 20;
            else if (spell == "ジャンプ") mpCost = 0;

            if (mpCost > AttackerMp)
            {
                TraceLog.Write($"{AttackerName}は{spell}を発動できない！残りMP:{AttackerMp} 使用MP:{mpCost}");
                return;
            }
            if (NotAlive)
            {
                TraceLog.Write($"{DefenderName}は死亡しています。");
                return;
            }
            TraceLog.Write($"{AttackerName}: MPを消費します {AttackerMp}/{byChar.MpMax} → {AttackerMp - mpCost}/{byChar.MpMax}");
            AttackerMp = AttackerMp - mpCost;
            byChar.Mp = AttackerMp;
            TraceLog.Write($"{Name}: {byChar.Name}の魔法「{spell}」を受けた");

            // 攻撃魔法処理
            if (spell == "攻撃魔法")
            {
                if (byChar is Hero hero)
                {
                    switch (hero.Profession) // ヒーローの職業
                    {
                        case "暗黒騎士" or "ナイト" or "戦士" or "竜騎士" or "モンク" or "忍者" or "侍":
                            damage = 15;
                            break;
                        case "白魔導士" or "学者":
                            damage = 20;
                            break;
                    }
                }
                else if (byChar is Enemy enemy) //エネミーの種族
                {
                    switch (enemy.Kind)
                    {
                        case "スライム" or "ゴブリン":
                            damage = 15;
                            break;
                        case "ドラゴン": // @@のちに実装
                            damage = 20;
                            break;
                        case "黒魔術師":
                            damage = 30;
                            break;
                    }
                }
                if (this.Jump)
                {
                    TraceLog.Write($"ジャンプ中の{Name}には当たらない！");
                    return;
                }

                if (Sleeping)
                {
                    Sleeping = false;
                    SleepingCount = 0;
                    TraceLog.Write($"{Name}: 睡眠が解除された。");
                    if (GameMaster.BattleField != null) GameMaster.BattleField!.AnimateGetup(Name);
                }

                if (Block)
                {
                    TraceLog.Write($"{Name} :防御が発動！{damage}ダメージ→{damage - 5}ダメージ");
                    damage = damage - 5; // 防御していたらダメージを-5
                }

                if (damage <= DefenderHp)
                {
                    DefenderHp = DefenderHp - damage;
                    TraceLog.Write($"{Name}は{damage}ダメージ受けた");

                    if (DefenderHp == 0)
                    {
                        if (LastStand)
                        {
                            TraceLog.Write($"{Name}: {Name}はラストスタンドを実行中。");
                            Hp = 1;
                        }
                        else
                        {
                            TraceLog.Write($"{Name}: {Name}は死亡した。");
                            ClearCoverState();
                        }
                    }
                    if (DefenderHp == 1)
                    {
                        TraceLog.Write($"{Name}: {Name}は気絶した。");
                    }

                    Hp = DefenderHp;
                    TraceLog.Write($"{Name}: {Hp}/{HpMax}");
                }
                else
                {
                    if (LastStand)
                    {
                        TraceLog.Write($"{Name}: {Name}はラストスタンドを実行中。");
                        Hp = 1;
                    }
                    else
                    {
                        Hp = 0;
                        TraceLog.Write($"{Name}: {Name}は死亡した。");
                        TraceLog.Write($"{Name}: {Hp}/{HpMax}");
                        ClearCoverState();
                    }
                }

                if (GameMaster.BattleField != null) await GameMaster.BattleField!.AnimateReceive(Name, (int)damage);
                GameMaster.BattleField?.CheckBattleEnd();
            }

            // 回復魔法処理
            else if (spell == "回復魔法")
            { 
                if (byChar is Hero hero)
                {
                    switch (hero.Profession) // ヒーローの職業　のちに実装予定のものも仮置き
                    {
                        case "暗黒騎士" or "ナイト" or "戦士" or "竜騎士" or "モンク" or "忍者" or "侍":
                            healPoint = 3;
                            break;
                        case "白魔導士" or "学者":
                            healPoint = 40;
                            break;
                    }
                }
                else if (byChar is Enemy enemy) //エネミーの種族
                {
                    switch (enemy.Kind)
                    {
                        case "スライム" or "ゴブリン":
                            healPoint = 3;
                            break;
                        case "ドラゴン": // @@のちに実装
                            healPoint = 20;
                            break;
                        case "黒魔術師":
                            healPoint = 10;
                            break;
                    }
                }
                if (healPoint + DefenderHp >= HpMax) // HpMax以上にしない
                {
                    TraceLog.Write($"{Name}は{healPoint}回復した! {DefenderHp}/{HpMax}→{HpMax}/{HpMax}");
                    Hp = HpMax;
                }
                else
                {
                    TraceLog.Write($"{Name}は{healPoint}回復した! {DefenderHp}/{HpMax}→{DefenderHp + healPoint}/{HpMax}");
                    DefenderHp = healPoint + DefenderHp;
                    Hp = DefenderHp;
                }
            }

            // ケアルラ処理
            else if (spell == "ケアルラ")
            {
                var targetHeroes = CharacterList.Heroes.Values.Where(h => !h.NotAlive).ToList();
                if (byChar is Hero hero)
                {
                    switch (hero.Profession) // ヒーローの職業
                    {
                        case "白魔導士":
                            healPoint = 30;
                            break;
                    }
                }
                else if (byChar is Enemy enemy) //エネミーの種族
                {
                    switch (enemy.Kind)
                    {
                        case "ヒーラー":
                            healPoint = 20;
                            break;
                    }
                }
                foreach (var Hero in targetHeroes)
                {

                if (healPoint + Hero.Hp >= Hero.HpMax) // HpMax以上にしない
                {
                    TraceLog.Write($"{Hero.Name}は{healPoint}回復した! {Hero.Hp}/{Hero.HpMax}→{Hero.HpMax}/{Hero.HpMax}");
                    Hero.Hp = Hero.HpMax;
                }
                else
                {
                    TraceLog.Write($"{Hero.Name}は{healPoint}回復した! {Hero.Hp}/{Hero.HpMax}→{Hero.Hp + healPoint}/{Hero.HpMax}");
                    Hero.Hp = healPoint + Hero.Hp;
                }
                }
            }

            // 毒魔法処理
            else if(spell == "毒魔法")
            {
                if (this.Jump)
                {
                    TraceLog.Write($"ジャンプ中の{Name}には当たらない！");
                    return;
                }
                if (this.Poisoned)
                {
                    TraceLog.Write($"{Name}は既に毒状態だ");
                }
                else if (this.Sleeping)
                {
                    TraceLog.Write($"{Name}は睡眠状態だ　状態異常は重複しない！");
                }
                else
                {
                    TraceLog.Write($"{Name}は毒状態になった");
                    this.Poisoned = true;
                    this.PoisonedCount = 5;
                }
            }

            // 睡眠魔法処理
            else if (spell == "睡眠魔法")
            {
                if (this.Jump)
                {
                    TraceLog.Write($"ジャンプ中の{Name}には当たらない！");
                    return;
                }
                if (this.Sleeping)
                {
                    TraceLog.Write($"{Name}は既に睡眠状態だ");
                }
                else if (this.Poisoned)
                {
                    TraceLog.Write($"{Name}は毒状態だ　状態異常は重複しない！");
                }
                else if (this.Name == "オーディン")
                {
                    TraceLog.Write($"オーディンは眠らない。");
                }
                else
                {
                    TraceLog.Write($"{Name}は睡眠状態になった");
                    this.delayedAction = "";
                    this.delayedTarget = "";
                    this.Sleeping = true;
                    this.SleepingCount = 4;
                }
            }

            // ジャンプ処理
            if (spell == "ジャンプ")
            {
                damage = 15;
                byChar.Jump = false;
                byChar.JumpLimit = byChar.JumpLimit - 1;
                if (this.Jump)
                {
                    TraceLog.Write($"ジャンプ中の{Name}には当たらない！");
                    return;
                }

                if (Sleeping)
                {
                    Sleeping = false;
                    SleepingCount = 0;
                    TraceLog.Write($"{Name}: 睡眠が解除された。");
                    if (GameMaster.BattleField != null) GameMaster.BattleField!.AnimateGetup(Name);
                }

                if (Block)
                {
                    TraceLog.Write($"{Name} :防御が発動！{damage}ダメージ→{damage - 5}ダメージ");
                    damage = damage - 5; // 防御していたらダメージを-5
                }

                if (damage <= DefenderHp)
                {
                    DefenderHp = DefenderHp - damage;
                    TraceLog.Write($"{Name}は{damage}ダメージ受けた");

                    if (DefenderHp == 0)
                    {
                        TraceLog.Write($"{Name}: {Name}は死亡した。");
                        ClearCoverState();
                    }
                    if (DefenderHp == 1)
                    {
                        TraceLog.Write($"{Name}: {Name}は気絶した。");
                    }

                    Hp = DefenderHp;
                    TraceLog.Write($"{Name}: {Hp}/{HpMax}");
                }
                else
                {
                    if (LastStand)
                    {
                        TraceLog.Write($"{Name}: {Name}はラストスタンドを実行中。");
                        Hp = 1;
                    }
                    else
                    {
                        Hp = 0;
                        TraceLog.Write($"{Name}: {Name}は死亡した。");
                        TraceLog.Write($"{Name}: {Hp}/{HpMax}");
                        ClearCoverState();
                    }
                }

                if (GameMaster.BattleField != null) await GameMaster.BattleField!.AnimateReceive(Name, (int)damage);
                GameMaster.BattleField?.CheckBattleEnd();
            }
        }

        /// <summary>
        /// 庇う・庇われるの関係を強制解除する
        /// </summary>
        public void ClearCoverState()
        {
            // 自分が誰かを庇っている
            if (!string.IsNullOrEmpty(CoveringName))
            {
                // 相手（庇われている人）側のCoveredByNameを消す
                if (CharacterList.AllChars.ContainsKey(CoveringName))
                {
                    CharacterList.AllChars[CoveringName].CoveredByName = "";
                }
                this.CoveringName = ""; // 自分の記録も消す
            }

            // 自分が誰かに庇われている
            if (!string.IsNullOrEmpty(CoveredByName))
            {
                // 相手（庇ってくれている人）側のCoveringNameを消す
                if (CharacterList.AllChars.ContainsKey(CoveredByName))
                {
                    CharacterList.AllChars[CoveredByName].CoveringName = "";
                }
                this.CoveredByName = ""; // 自分の記録も消す
            }
        }

        /// <summary>
        /// 毒状態の処理
        /// </summary>
        public async Task PoisonedState()
        {
            uint poisonDamage = 5;
            this.PoisonedCount--;
            TraceLog.Write($"{Name}: 毒を受けている　残り{PoisonedCount}ターン");
            if (Hp <= poisonDamage)
            {
                this.Hp = 0;
                if (LastStand)
                {
                    TraceLog.Write($"{Name}: {Name}はラストスタンドを実行中。");
                    Hp = 1;
                }
                else
                {
                    TraceLog.Write($"{Name}: {Name}は死亡した。");
                    ClearCoverState();
                }
            }
            else
            {
                this.Hp -= poisonDamage;
            }
            if (GameMaster.BattleField != null) await GameMaster.BattleField!.AnimatePoison(this.Name);
            GameMaster.BattleField?.CheckBattleEnd();
            if (PoisonedCount == 0)
            {
                Poisoned = false;
                TraceLog.Write($"{Name}: 毒が解除された！");
            }
        }

        /// <summary>
        /// 睡眠状態の処理
        /// </summary>
        /// <returns></returns>
        public async Task SleepingState()
        {
            this.SleepingCount--;
            TraceLog.Write($"{Name}: 睡眠状態　残り{SleepingCount}ターン");
            if (SleepingCount == 0)
            {
                Sleeping = false;
                TraceLog.Write($"{Name}: 睡眠が解除された。");
            }
            if (GameMaster.BattleField != null) await GameMaster.BattleField!.AnimateSleep(this.Name);
        }
    }
}
