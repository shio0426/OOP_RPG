using System.Security.Cryptography.X509Certificates;

namespace OOP_RPG
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            //ApplicationConfiguration.Initialize();
            //Application.Run(new FrmBattleField());
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 難易度選択画面を出す
            FrmDifficultySelect selectForm = new FrmDifficultySelect();

            // ボタンが押されたら処理を進める
            if (selectForm.ShowDialog() == DialogResult.OK)
            {
                // 選択されたファイル名を取得
                string selectedFile = selectForm.SelectedEnemyCsv;
                string difficulty = "Normal"; // デフォルト

                if (selectedFile == "Enemy.csv")
                {
                    difficulty = "Easy";      // 初級
                }
                else if (selectedFile == "BossEnemy.csv")
                {
                    difficulty = "Hard";      // 上級
                }

                // フォーム破棄
                selectForm.Dispose();

                CharacterList.Initialize(selectedFile);

                // ゲーム画面の生成
                var bf = new FrmBattleField(difficulty);

                GameMaster.SetBattleField(bf);

                TraceLog.SetBattleField(bf);

                TraceLog.Write("バトル開始！");

                // ゲーム起動
                Application.Run(bf);
            }
            else
            {
                selectForm.Dispose();
            }
        }
    }
}