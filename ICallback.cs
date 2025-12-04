using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_RPG
{
    /// <summary>
    /// Callbackデリゲート<br/>
    /// デリゲート(delegate) = メソッドをデータ型とするもの<br/>
    /// ここではCalllbackという型(2つの引数を取り戻り値なしのメソッド)を宣言する<br/>
    /// </summary>
    /// <param name="action"></param>
    /// <param name="target"></param>
    delegate void Callback(string action,string target);
    /// <summary>
    /// コールバックを受けるインターフェイス
    /// </summary>
    internal interface ICallback
    {
        public void Callback(string action, string target);
    }
}
