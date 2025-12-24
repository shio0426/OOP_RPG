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
            lblHero1_HP = new Label();
            lblHero2_HP = new Label();
            lblHero3_HP = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            flpHero3_buff = new FlowLayoutPanel();
            flpHero2_buff = new FlowLayoutPanel();
            flpHero1_buff = new FlowLayoutPanel();
            lblHero1_MP = new Label();
            lblHero1_name = new Label();
            lblHero2_name = new Label();
            lblHero3_name = new Label();
            lblHero2_MP = new Label();
            lblHero3_MP = new Label();
            picPoison1 = new PictureBox();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picPoison1).BeginInit();
            SuspendLayout();
            // 
            // txtTraceLog
            // 
            txtTraceLog.Font = new Font("Microsoft Sans Serif", 8F);
            txtTraceLog.Location = new Point(1112, 43);
            txtTraceLog.Margin = new Padding(4, 3, 4, 3);
            txtTraceLog.Multiline = true;
            txtTraceLog.Name = "txtTraceLog";
            txtTraceLog.ReadOnly = true;
            txtTraceLog.ScrollBars = ScrollBars.Vertical;
            txtTraceLog.Size = new Size(369, 563);
            txtTraceLog.TabIndex = 0;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Underline);
            lblName.ForeColor = Color.Black;
            lblName.Location = new Point(46, 650);
            lblName.Margin = new Padding(4, 0, 4, 0);
            lblName.Name = "lblName";
            lblName.Size = new Size(54, 15);
            lblName.TabIndex = 1;
            lblName.Text = "lblName";
            // 
            // lstAction
            // 
            lstAction.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Underline);
            lstAction.FormattingEnabled = true;
            lstAction.ItemHeight = 15;
            lstAction.Location = new Point(46, 668);
            lstAction.Margin = new Padding(4, 3, 4, 3);
            lstAction.Name = "lstAction";
            lstAction.Size = new Size(186, 139);
            lstAction.TabIndex = 2;
            lstAction.SelectedIndexChanged += lstAction_SelectedIndexChanged;
            // 
            // lstTarget
            // 
            lstTarget.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Underline);
            lstTarget.FormattingEnabled = true;
            lstTarget.ItemHeight = 15;
            lstTarget.Location = new Point(253, 668);
            lstTarget.Margin = new Padding(4, 3, 4, 3);
            lstTarget.Name = "lstTarget";
            lstTarget.Size = new Size(188, 139);
            lstTarget.TabIndex = 3;
            lstTarget.SelectedIndexChanged += lstTarget_SelectedIndexChanged;
            // 
            // btnPause
            // 
            btnPause.Font = new Font("Microsoft Sans Serif", 8F);
            btnPause.Location = new Point(1255, 612);
            btnPause.Margin = new Padding(4, 3, 4, 3);
            btnPause.Name = "btnPause";
            btnPause.Size = new Size(70, 30);
            btnPause.TabIndex = 4;
            btnPause.Text = "Pause";
            btnPause.UseVisualStyleBackColor = true;
            btnPause.Click += btnPause_Click;
            // 
            // btnLogClear
            // 
            btnLogClear.Font = new Font("Microsoft Sans Serif", 8F);
            btnLogClear.Location = new Point(1333, 612);
            btnLogClear.Margin = new Padding(4, 3, 4, 3);
            btnLogClear.Name = "btnLogClear";
            btnLogClear.Size = new Size(70, 30);
            btnLogClear.TabIndex = 5;
            btnLogClear.Text = "ログクリア";
            btnLogClear.UseVisualStyleBackColor = true;
            btnLogClear.Click += btnLogClear_Click;
            // 
            // btnQuit
            // 
            btnQuit.Font = new Font("Microsoft Sans Serif", 8F);
            btnQuit.Location = new Point(1411, 612);
            btnQuit.Margin = new Padding(4, 3, 4, 3);
            btnQuit.Name = "btnQuit";
            btnQuit.Size = new Size(70, 30);
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
            // lblHero1_HP
            // 
            lblHero1_HP.Anchor = AnchorStyles.None;
            lblHero1_HP.AutoSize = true;
            lblHero1_HP.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblHero1_HP.ForeColor = Color.Black;
            lblHero1_HP.Location = new Point(130, 18);
            lblHero1_HP.Margin = new Padding(4, 0, 4, 0);
            lblHero1_HP.Name = "lblHero1_HP";
            lblHero1_HP.Size = new Size(66, 13);
            lblHero1_HP.TabIndex = 20;
            lblHero1_HP.Text = "HP:100/100";
            // 
            // lblHero2_HP
            // 
            lblHero2_HP.Anchor = AnchorStyles.None;
            lblHero2_HP.AutoSize = true;
            lblHero2_HP.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblHero2_HP.ForeColor = Color.Black;
            lblHero2_HP.Location = new Point(130, 65);
            lblHero2_HP.Margin = new Padding(4, 0, 4, 0);
            lblHero2_HP.Name = "lblHero2_HP";
            lblHero2_HP.Size = new Size(66, 13);
            lblHero2_HP.TabIndex = 21;
            lblHero2_HP.Text = "HP:100/100";
            // 
            // lblHero3_HP
            // 
            lblHero3_HP.Anchor = AnchorStyles.None;
            lblHero3_HP.AutoSize = true;
            lblHero3_HP.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblHero3_HP.ForeColor = Color.Black;
            lblHero3_HP.Location = new Point(130, 112);
            lblHero3_HP.Margin = new Padding(4, 0, 4, 0);
            lblHero3_HP.Name = "lblHero3_HP";
            lblHero3_HP.Size = new Size(66, 13);
            lblHero3_HP.TabIndex = 22;
            lblHero3_HP.Text = "HP:100/100";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble;
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.8333321F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 49.1666679F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 103F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 705F));
            tableLayoutPanel1.Controls.Add(flpHero3_buff, 3, 2);
            tableLayoutPanel1.Controls.Add(flpHero2_buff, 3, 1);
            tableLayoutPanel1.Controls.Add(flpHero1_buff, 3, 0);
            tableLayoutPanel1.Controls.Add(lblHero1_MP, 2, 0);
            tableLayoutPanel1.Controls.Add(lblHero1_HP, 1, 0);
            tableLayoutPanel1.Controls.Add(lblHero2_HP, 1, 1);
            tableLayoutPanel1.Controls.Add(lblHero3_HP, 1, 2);
            tableLayoutPanel1.Controls.Add(lblHero1_name, 0, 0);
            tableLayoutPanel1.Controls.Add(lblHero2_name, 0, 1);
            tableLayoutPanel1.Controls.Add(lblHero3_name, 0, 2);
            tableLayoutPanel1.Controls.Add(lblHero2_MP, 2, 1);
            tableLayoutPanel1.Controls.Add(lblHero3_MP, 2, 2);
            tableLayoutPanel1.Location = new Point(449, 668);
            tableLayoutPanel1.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(1032, 143);
            tableLayoutPanel1.TabIndex = 36;
            // 
            // flpHero3_buff
            // 
            flpHero3_buff.Location = new Point(326, 100);
            flpHero3_buff.Name = "flpHero3_buff";
            flpHero3_buff.Size = new Size(700, 37);
            flpHero3_buff.TabIndex = 39;
            // 
            // flpHero2_buff
            // 
            flpHero2_buff.Location = new Point(326, 53);
            flpHero2_buff.Name = "flpHero2_buff";
            flpHero2_buff.Size = new Size(700, 38);
            flpHero2_buff.TabIndex = 38;
            // 
            // flpHero1_buff
            // 
            flpHero1_buff.Location = new Point(326, 6);
            flpHero1_buff.Name = "flpHero1_buff";
            flpHero1_buff.Size = new Size(700, 38);
            flpHero1_buff.TabIndex = 37;
            // 
            // lblHero1_MP
            // 
            lblHero1_MP.Anchor = AnchorStyles.None;
            lblHero1_MP.AutoSize = true;
            lblHero1_MP.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblHero1_MP.ForeColor = Color.Black;
            lblHero1_MP.Location = new Point(235, 18);
            lblHero1_MP.Margin = new Padding(4, 0, 4, 0);
            lblHero1_MP.Name = "lblHero1_MP";
            lblHero1_MP.Size = new Size(67, 13);
            lblHero1_MP.TabIndex = 37;
            lblHero1_MP.Text = "MP:100/100";
            // 
            // lblHero1_name
            // 
            lblHero1_name.Anchor = AnchorStyles.None;
            lblHero1_name.AutoSize = true;
            lblHero1_name.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblHero1_name.ForeColor = Color.Black;
            lblHero1_name.Location = new Point(36, 18);
            lblHero1_name.Margin = new Padding(4, 0, 4, 0);
            lblHero1_name.Name = "lblHero1_name";
            lblHero1_name.Size = new Size(40, 13);
            lblHero1_name.TabIndex = 23;
            lblHero1_name.Text = "名無し";
            // 
            // lblHero2_name
            // 
            lblHero2_name.Anchor = AnchorStyles.None;
            lblHero2_name.AutoSize = true;
            lblHero2_name.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblHero2_name.ForeColor = Color.Black;
            lblHero2_name.Location = new Point(36, 65);
            lblHero2_name.Margin = new Padding(4, 0, 4, 0);
            lblHero2_name.Name = "lblHero2_name";
            lblHero2_name.Size = new Size(40, 13);
            lblHero2_name.TabIndex = 24;
            lblHero2_name.Text = "名無し";
            // 
            // lblHero3_name
            // 
            lblHero3_name.Anchor = AnchorStyles.None;
            lblHero3_name.AutoSize = true;
            lblHero3_name.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblHero3_name.ForeColor = Color.Black;
            lblHero3_name.Location = new Point(36, 112);
            lblHero3_name.Margin = new Padding(4, 0, 4, 0);
            lblHero3_name.Name = "lblHero3_name";
            lblHero3_name.Size = new Size(40, 13);
            lblHero3_name.TabIndex = 25;
            lblHero3_name.Text = "名無し";
            // 
            // lblHero2_MP
            // 
            lblHero2_MP.Anchor = AnchorStyles.None;
            lblHero2_MP.AutoSize = true;
            lblHero2_MP.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblHero2_MP.ForeColor = Color.Black;
            lblHero2_MP.Location = new Point(235, 65);
            lblHero2_MP.Margin = new Padding(4, 0, 4, 0);
            lblHero2_MP.Name = "lblHero2_MP";
            lblHero2_MP.Size = new Size(67, 13);
            lblHero2_MP.TabIndex = 38;
            lblHero2_MP.Text = "MP:100/100";
            // 
            // lblHero3_MP
            // 
            lblHero3_MP.Anchor = AnchorStyles.None;
            lblHero3_MP.AutoSize = true;
            lblHero3_MP.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblHero3_MP.ForeColor = Color.Black;
            lblHero3_MP.Location = new Point(235, 112);
            lblHero3_MP.Margin = new Padding(4, 0, 4, 0);
            lblHero3_MP.Name = "lblHero3_MP";
            lblHero3_MP.Size = new Size(67, 13);
            lblHero3_MP.TabIndex = 39;
            lblHero3_MP.Text = "MP:100/100";
            // 
            // picPoison1
            // 
            picPoison1.BackColor = Color.Transparent;
            picPoison1.Image = Properties.Resources._0;
            picPoison1.Location = new Point(1336, 488);
            picPoison1.Margin = new Padding(4, 3, 4, 3);
            picPoison1.Name = "picPoison1";
            picPoison1.Size = new Size(26, 18);
            picPoison1.SizeMode = PictureBoxSizeMode.Zoom;
            picPoison1.TabIndex = 41;
            picPoison1.TabStop = false;
            picPoison1.Visible = false;
            // 
            // FrmBattleField
            // 
            AutoScaleDimensions = new SizeF(9F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.背景2;
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(1584, 861);
            Controls.Add(picPoison1);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(btnQuit);
            Controls.Add(btnLogClear);
            Controls.Add(btnPause);
            Controls.Add(lstTarget);
            Controls.Add(lstAction);
            Controls.Add(lblName);
            Controls.Add(txtTraceLog);
            DoubleBuffered = true;
            Font = new Font("Microsoft Sans Serif", 11F);
            ForeColor = Color.Black;
            Margin = new Padding(4, 3, 4, 3);
            Name = "FrmBattleField";
            Text = "a";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picPoison1).EndInit();
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
        private Label lblHero1_HP;
        private Label lblHero2_HP;
        private Label lblHero3_HP;
        private TableLayoutPanel tableLayoutPanel1;
        private Label lblHero1_name;
        private Label lblHero2_name;
        private Label lblHero3_name;
        private Label lblHero1_MP;
        private Label lblHero2_MP;
        private Label lblHero3_MP;
        private FlowLayoutPanel flpHero3_buff;
        private FlowLayoutPanel flpHero2_buff;
        private FlowLayoutPanel flpHero1_buff;
        private PictureBox picPoison1;
    }
}
