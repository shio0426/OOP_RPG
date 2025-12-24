using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_RPG
{
    internal interface IBattle
    {
        /// <summary>
        /// 相手からの攻撃を受ける
        /// </summary>
        /// <param name="byChar">攻撃したキャラクター</param>
        public Task AttackedBy(Character byChar);

        /// <summary>
        /// 相手からの魔法を受ける
        /// </summary>
        /// <param name="spell">魔法の名前</param>
        /// <param name="byChar">魔法をかけたキャラクター</param>
        public Task CastSpellOf(string spell, Character byChar);
    }
}
