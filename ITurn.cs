using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_RPG
{
    /// <summary>
    /// ターン制御を受けるインターフェイス
    /// </summary>
    internal interface ITurn
    {
        /// <summary>
        /// 自分のターン
        /// </summary>
        public void YourTurn();
    }
}
