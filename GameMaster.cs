using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_RPG
{
    internal static class GameMaster
    {
        // @@画面を作成した後コメントを外す
        public static FrmBattleField? BattleField { get; private set; } = default!;

        // 静的コンストラクタ:このクラス初回仕使用に1回だけ実行される
        static GameMaster()
        {
            // キューに全キャラクターを挿入
            EnqAllChars();
        }

        // @@画面を作成した後コメントを外す
        public static void SetBattleField(FrmBattleField battleField)
        {
            BattleField = battleField;
        }

        public static void EnqAllChars()
        {
            // 画面を作成した後コメントを外す
            foreach (var it in CharacterList.Heroes)
            {
                BattleQueue.Enq(it.Value, it.Value.Speed);
                Debug.WriteLine($"ヒーローの{it.Value.Name}がインキューしたよ！");

            }
            foreach (var it in CharacterList.Enemies)
            {
                BattleQueue.Enq(it.Value, it.Value.Speed);
                Debug.WriteLine($"エネミーの{it.Value.Name}がインキューしたよ！");
            }
        }
        /// <summary>
        /// 次のキャラクターにYourTurnを送る
        /// </summary>
        public static void Run()
        {
            var turn = BattleQueue.Deq();

            if (turn == null) return;

            TraceLog.Write($"GameMaster: {((Character)turn).Name}にターンを渡す");
            turn.YourTurn();
        }

        /// <summary>
        /// コンストラクタを動かすためのダミーメソッド
        /// ユニットテスト用
        /// </summary>
        //public static void Dummy()
        //{
        //    // 何もしない
        //}   
    }
}
