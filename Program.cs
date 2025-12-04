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

            // ゲーム画面の生成
            var bf = new FrmBattleField();

            // GameMasterにゲーム画面の参照をセット
            GameMaster.SetBattleField(bf);

            // TraceLogにゲーム画面の参照をセット
            TraceLog.SetBattleField(bf);
            TraceLog.Write("バトル開始！");
            Application.Run(bf);
        }
    }
}