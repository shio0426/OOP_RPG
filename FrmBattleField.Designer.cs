namespace OOP_RPG
{
    partial class FrmBattleField
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            txtTraceLog = new TextBox();
            lblName = new Label();
            lstAction = new ListBox();
            lstTarget = new ListBox();
            btnPause = new Button();
            btnLogClear = new Button();
            btnQuit = new Button();
            Timer = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // txtTraceLog
            // 
            txtTraceLog.Location = new Point(119, 82);
            txtTraceLog.Multiline = true;
            txtTraceLog.Name = "txtTraceLog";
            txtTraceLog.ReadOnly = true;
            txtTraceLog.ScrollBars = ScrollBars.Vertical;
            txtTraceLog.Size = new Size(1095, 613);
            txtTraceLog.TabIndex = 0;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(1469, 85);
            lblName.Name = "lblName";
            lblName.Size = new Size(78, 25);
            lblName.TabIndex = 1;
            lblName.Text = "lblName";
            // 
            // lstAction
            // 
            lstAction.FormattingEnabled = true;
            lstAction.ItemHeight = 25;
            lstAction.Location = new Point(1469, 126);
            lstAction.Name = "lstAction";
            lstAction.Size = new Size(297, 154);
            lstAction.TabIndex = 2;
            lstAction.SelectedIndexChanged += lstAction_SelectedIndexChanged;
            // 
            // lstTarget
            // 
            lstTarget.FormattingEnabled = true;
            lstTarget.ItemHeight = 25;
            lstTarget.Location = new Point(1469, 306);
            lstTarget.Name = "lstTarget";
            lstTarget.Size = new Size(297, 154);
            lstTarget.TabIndex = 3;
            lstTarget.SelectedIndexChanged += lstTarget_SelectedIndexChanged;
            // 
            // btnPause
            // 
            btnPause.Location = new Point(1654, 555);
            btnPause.Name = "btnPause";
            btnPause.Size = new Size(112, 34);
            btnPause.TabIndex = 4;
            btnPause.Text = "Pause";
            btnPause.UseVisualStyleBackColor = true;
            btnPause.Click += btnPause_Click;
            // 
            // btnLogClear
            // 
            btnLogClear.Location = new Point(1654, 608);
            btnLogClear.Name = "btnLogClear";
            btnLogClear.Size = new Size(112, 34);
            btnLogClear.TabIndex = 5;
            btnLogClear.Text = "ログクリア";
            btnLogClear.UseVisualStyleBackColor = true;
            btnLogClear.Click += btnLogClear_Click;
            // 
            // btnQuit
            // 
            btnQuit.Location = new Point(1654, 661);
            btnQuit.Name = "btnQuit";
            btnQuit.Size = new Size(112, 34);
            btnQuit.TabIndex = 6;
            btnQuit.Text = "終了";
            btnQuit.UseVisualStyleBackColor = true;
            btnQuit.Click += btnQuit_Click;
            // 
            // Timer
            // 
            Timer.Interval = 1000;
            Timer.Tick += Timing_Tick;
            // 
            // FrmBattleField
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1898, 1024);
            Controls.Add(btnQuit);
            Controls.Add(btnLogClear);
            Controls.Add(btnPause);
            Controls.Add(lstTarget);
            Controls.Add(lstAction);
            Controls.Add(lblName);
            Controls.Add(txtTraceLog);
            Name = "FrmBattleField";
            Text = "BattleField";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtTraceLog;
        private Label lblName;
        private ListBox lstAction;
        private ListBox lstTarget;
        private Button btnPause;
        private Button btnLogClear;
        private Button btnQuit;
        private System.Windows.Forms.Timer Timer;
    }
}
