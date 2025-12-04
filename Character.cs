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
        public bool Poisoned { get; protected set; } = false;

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
        /// 相手からの攻撃を受ける
        /// </summary>
        /// <param name="byChar"></param>
        public void AttackedBy(Character byChar)
        {
            TraceLog.Write($"{Name}: {byChar.Name}から攻撃を受けた");

            // @@攻撃を受けた時の処理@@
        }

        /// <summary>
        /// 相手からの魔法を受ける
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="byChar"></param>

        public void CastSpellOf(string spell, Character byChar)
        {
            TraceLog.Write($"{Name}: {byChar.Name}の魔法「{spell}」を受けた");

            // @@魔法を受けた時の処理@@
        }
    }
}
