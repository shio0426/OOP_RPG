namespace OOP_RPG
{
    partial class FrmDifficultySelect
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnBeginner = new Button();
            btnAdvanced = new Button();
            SuspendLayout();
            // 
            // btnBeginner
            // 
            btnBeginner.Font = new Font("JFドットAyuゴシック20", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            btnBeginner.Location = new Point(184, 199);
            btnBeginner.Name = "btnBeginner";
            btnBeginner.Size = new Size(164, 48);
            btnBeginner.TabIndex = 0;
            btnBeginner.Text = "初級";
            btnBeginner.UseVisualStyleBackColor = true;
            btnBeginner.Click += btnBeginner_Click;
            // 
            // btnAdvanced
            // 
            btnAdvanced.Font = new Font("JFドットAyuゴシック20", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            btnAdvanced.Location = new Point(440, 199);
            btnAdvanced.Name = "btnAdvanced";
            btnAdvanced.Size = new Size(164, 48);
            btnAdvanced.TabIndex = 1;
            btnAdvanced.Text = "上級";
            btnAdvanced.UseVisualStyleBackColor = true;
            btnAdvanced.Click += btnAdvanced_Click;
            // 
            // FrmDifficultySelect
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnAdvanced);
            Controls.Add(btnBeginner);
            Name = "FrmDifficultySelect";
            Text = "難易度選択";
            ResumeLayout(false);
        }

        #endregion

        private Button btnBeginner;
        private Button btnAdvanced;
    }
}