namespace OOP_RPG
{
    internal partial class FrmBattleField : Form, ICommand
    {
        private Callback _callback = static (string a, string t) => { };
        private readonly string[] _actions = [.. CommandList.Commands.Keys];
        public FrmBattleField()
        {
            InitializeComponent();

            this.Timer.Start();
        }

        private void Timing_Tick(object sender, EventArgs e)
        {
            GameMaster.Run();
        }

        public void ActivateCommand(ICallback callbackObj)
        {
            this.Timer.Stop();

            _callback = callbackObj.Callback;

            lblName.Text = ((Character)callbackObj).Name;

            foreach (var action in _actions)
            {
                lstAction.Items.Add(action);
            }
        }

        private void lstAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            var nextTarget = (string)lstAction.SelectedItem!;

            lstTarget.Items.Clear();
            switch (CommandList.Commands[nextTarget].Target)
            {
                case "ñ°ï˚":
                    foreach (var tar in CharacterList.Heroes)
                    {
                        lstTarget.Items.Add(tar.Key);
                    }
                    break;

                case "ìGï˚":
                    foreach (var tar in CharacterList.Enemies)
                    {
                        lstTarget.Items.Add(tar.Key);
                    }
                    break;

                case "é©ï™":
                    lstTarget.Items.Add(lblName.Text);
                    break;
            }
        }

        private void lstTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            string act = (string)lstAction.SelectedItem!;
            string tar = (string)lstTarget.SelectedItem!;

            _callback(act, tar);

            _callback = static (string a, string t) => { }; //ÉNÉäÉA

            lstAction.Items.Clear();
            lstTarget.Items.Clear();

            this.Timer.Start();
        }

        public void MsgLog(string text)
        {
            string sep = txtTraceLog.Text.Length == 0 ? "" : "\r\n";
            txtTraceLog.AppendText(sep + text);
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (Timer.Enabled)
            {
                Timer.Stop();
            }
            else
            {
                Timer.Start();
            }
        }

        private void btnLogClear_Click(object sender, EventArgs e)
        {
            txtTraceLog.Clear();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
