using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_RPG
{
    public partial class FrmDifficultySelect : Form
    {
        public string SelectedEnemyCsv { get; private set; } = "Enemy.csv";
        public FrmDifficultySelect()
        {
            InitializeComponent();
        }

        private void btnBeginner_Click(object sender, EventArgs e)
        {
            SelectedEnemyCsv = "Enemy.csv";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            SelectedEnemyCsv = "BossEnemy.csv";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
