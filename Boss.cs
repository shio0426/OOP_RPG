using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_RPG
{
    internal class Boss : Enemy
    {
        private readonly List<(string Action, string TargetType)> _routine = new()
        {
            ("溜め", "Self"),
            ("斬鉄剣", "Self"),
            ("防御", "Self"),
            ("見切り", "Self"),
            ("防御", "Self"),
            ("攻撃", "RandomHero"),
            ("見切り", "Self"),
            ("攻撃", "RandomHero"),
            ("防御", "Self"),
            ("見切り", "Self")
        };

        private int _turnIndex = 0;

        // 親クラスのコンストラクタを呼び出す
        public Boss(string name, uint hp, uint mp, uint speed, string kind)
            : base(name, hp, mp, speed, kind)
        {
        }

        protected override (string act, string tar) MakeDecision()
        {
            // 現在のターンに対応する行動を取得
            var command = _routine[_turnIndex];

            // turnIndexを更新（ループさせる）
            _turnIndex = (_turnIndex + 1) % _routine.Count;

            string action = command.Action;
            string targetType = command.TargetType;
            string targetName = "";

            // ターゲット選択ロジック
            switch (targetType)
            {
                case "Self":
                    targetName = this.Name;
                    break;

                case "RandomHero":
                    // 生きている勇者からランダム
                    var livingHeroes = CharacterList.Heroes.Values
                        .Where(h => !h.NotAlive).ToList();
                    if (livingHeroes.Count > 0)
                    {
                        // ランダムアクセス
                        targetName = livingHeroes[new Random().Next(livingHeroes.Count)].Name;
                    }
                    break;

                case "WeakestHero": //使うかも？
                    // HPが一番低い勇者
                    var weakHero = CharacterList.Heroes.Values
                        .Where(h => !h.NotAlive)
                        .OrderBy(h => h.Hp)
                        .FirstOrDefault();
                    targetName = weakHero?.Name ?? "";
                    break;
            }

            // ターゲットが見つからなかった場合、適当に生きている勇者を選ぶ
            if (string.IsNullOrEmpty(targetName))
            {
                var anyHero = CharacterList.Heroes.Values.FirstOrDefault(h => !h.NotAlive);
                targetName = anyHero?.Name ?? "";
            }

            // 念のためにMPチェック
            if (CommandList.Commands.ContainsKey(action))
            {
                if (this.Mp < CommandList.Commands[action].UseMp)
                {
                    action = "攻撃";
                }
            }

            return (action, targetName);
        }

        public override bool CanUseCommand(string commandName)
        {
            // 全てのコマンドが使用可能
            return true;
        }
    }
}
