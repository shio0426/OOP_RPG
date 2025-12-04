using System;
using System.Collections.Generic;
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

        public static void InjectRandMock(IRand mock)
        {
            _rand = mock;
        }

        private(string act, string tar) MakeDecision()
        {
            string[] commands = [.. CommandList.Commands.Keys];
            string act = commands[_rand.Next(CommandList.Commands.Count)];

            string[] targets = [];
            switch (CommandList.Commands[act].Target)
            {
                case "敵方":
                    targets = [.. CharacterList.Heroes.Keys];
                    break;
                case "味方":
                    targets = [.. CharacterList.Enemies.Keys];
                    break;
                case "自分":
                    targets = [this.Name];
                    break;  
            }
            int t = _rand.Next(targets.Length);
            string tar = targets[t];

            return (act, tar);
        }

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
            }
            else
            {
                var (act, tar) = MakeDecision();

                switch (act)
                {
                    case "攻撃魔法":
                    case "回復魔法":
                        // 魔法詠唱中に遷移
                        TraceLog.Write($"{Name}: {tar}に{act}を唱えた");
                        this.delayedAction = act;
                        this.delayedTarget = tar;
                        break;

                    case "攻撃":
                        // ターゲットに対して攻撃
                        TraceLog.Write($"{Name}: {tar}に{act}した");
                        CharacterList.AllChars[tar].AttackedBy(this);
                        break;

                    case "防御":
                        TraceLog.Write($"{Name}: {tar}を{act}している");
                        break;

                    case "庇う":
                        TraceLog.Write($"{Name}: {tar}を{act}態勢に入った");
                        break;
                }
            }
            //@@バトルキューに挿入@@
            BattleQueue.Enq(this, this.Speed);
        }
    }
}
