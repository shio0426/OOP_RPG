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
        public void YourTurn()
        {
            TraceLog.Write($"{Name}: ターンが移った");

            if (this.NotAlive)
            {
                TraceLog.Write($"{Name}: 戦闘不能である");
                return;
            }
            if (this.SpellCasting)
            {
                //魔法詠唱中
                TraceLog.Write($"{Name}: 魔法詠唱中である");
                TraceLog.Write($"{Name}: {this.delayedTarget}への{this.delayedAction}が発動");

                // 魔法を発動する
                var tar = CharacterList.AllChars[this.delayedTarget];
                tar.CastSpellOf(this.delayedAction, this);

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
        public void Callback(string action,string target)
        {
            switch (action)
            {
                case "攻撃魔法":
                case "回復魔法":
                    // 魔法の場合は遅延実行として保存
                    TraceLog.Write($"{Name}: {target}に{action}を唱えた");
                    this.delayedAction = action;
                    this.delayedTarget = target;
                    break;

                case "攻撃":
                    // ターゲットに対して攻撃
                    TraceLog.Write($"{Name}: {target}を{action}した");
                    CharacterList.AllChars[target].AttackedBy(this);
                    break;

                case "防御":
                    TraceLog.Write($"{Name}: {action} している ({target})");
                    break;

                case "庇う":
                    TraceLog.Write($"{Name}: {target}を{action} 態勢に入った");
                    break;
            }

            // @@バトルキューに挿入@@
            BattleQueue.Enq(this,this.Speed);
        }
    }
}
