using System;
using System.Diagnostics;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace OOP_RPG
{
    internal partial class FrmBattleField : Form, ICommand
    {
        private Callback _callback = static (string a, string t) => Task.CompletedTask;
        private readonly string[] _actions = [.. CommandList.Commands.Keys];
        // 名前付きタプル
        private Dictionary<string, BattleUnitView> _units = new Dictionary<string, BattleUnitView>();
        private Animate _animator = null!;


        private Image[] _damageImages = null!;

        private Image?[] _poisonImages = null!;
        private Image?[] _sleepImages = null!;
        private Image?[] _lastStandImages = null!;
        private PictureBox _picSelect = null!;

        public FrmBattleField(string difficulty)
        {
            this.AutoScroll = true;
            InitializeComponent();
            _picSelect = new PictureBox();
            InitializeImages();
            InitializeBattleUnits();

            switch (difficulty)
            {
                case "Easy": // 初級
                    this.BackgroundImage = Properties.Resources.初級背景;
                    break;

                case "Hard": // 上級
                    this.BackgroundImage = Properties.Resources.上級背景;
                    break;

                default:
                    this.BackgroundImage = Properties.Resources.背景1;
                    break;
            }

            this.BackgroundImageLayout = ImageLayout.Stretch;

            // Animatorの生成
            _animator = new Animate(
                units: _units,
                timer: this.Timer,
                form: this,
                updateStatusAction: UpdateAllStatus,
                damageImages: _damageImages
            );

            this.Timer.Start();
        } 

        private void Timing_Tick(object sender, EventArgs e)
        {
            GameMaster.Run();
            UpdateAllStatus();
        }

        // 画像配列の初期化メソッド
        private void InitializeImages()
        {
            _damageImages = new Image[]
            {
                Properties.Resources._0,
                Properties.Resources._1,
                Properties.Resources._2,
                Properties.Resources._3,
                Properties.Resources._4,
                Properties.Resources._5,
                Properties.Resources._6,
                Properties.Resources._7,
                Properties.Resources._8,
                Properties.Resources._9
            };
            _poisonImages = new Image?[] { null,Properties.Resources.毒1, Properties.Resources.毒2, Properties.Resources.毒3, Properties.Resources.毒4, Properties.Resources.毒5};
            _sleepImages = new Image?[] { null, Properties.Resources.睡眠1, Properties.Resources.睡眠2, Properties.Resources.睡眠3, Properties.Resources.睡眠4};
            _lastStandImages = new Image?[] { null, Properties.Resources.不死1, Properties.Resources.不死2, Properties.Resources.不死3, Properties.Resources.不死4, Properties.Resources.不死5 };
        }

        /// <summary>
        /// キャラクターリストから動的にコントロールを生成して配置する
        /// </summary>
        private void InitializeBattleUnits()
        {
            // 配置設定
            int startX_Hero = 900;  // 味方X座標
            int startY_Hero = 100;   // 味方Y座標
            int stepY_Hero = 150;   // 味方の縦の間隔

            int startX_Enemy = 100; // 敵X座標
            int startY_Enemy = 100;  // 敵Y座標
            int stepY_Enemy = 150;  // 敵の縦の間隔

            int heroIndex = 0;
            int enemyIndex = 0;
            int id = 1;
            bool Boss = false;

            // 味方の生成
            foreach (var hero in CharacterList.Heroes.Values)
            {
                // 画像の取得 (リソース名 = キャラクター名)
                Image? charaImg = GetImageFromResource(hero.Name);

                // PictureBoxの生成
                PictureBox pic = new PictureBox();
                pic.Name = $"pic_{hero.Name}"; // 名前
                pic.Image = charaImg;
                pic.Size = new Size(100, 100); // サイズ
                pic.SizeMode = PictureBoxSizeMode.Zoom;
                pic.BackColor = Color.Transparent;

                pic.Tag = hero;
                pic.Paint += Pbx_Character_Paint;  // 描画イベントを登録

                // 座標計算 (縦に並べる)
                pic.Location = new Point(startX_Hero, startY_Hero + (stepY_Hero * heroIndex));

                // 前面に表示
                this.Controls.Add(pic);
                pic.BringToFront();

                var nameLbl = this.Controls.Find($"lblHero{id}_name", true).FirstOrDefault() as Label;
                var hpLbl = this.Controls.Find($"lblHero{id}_HP", true).FirstOrDefault() as Label;
                var mpLbl = this.Controls.Find($"lblHero{id}_MP", true).FirstOrDefault() as Label;
                var buffPnl = this.Controls.Find($"flpHero{id}_buff", true).FirstOrDefault() as FlowLayoutPanel;
                if (nameLbl != null) nameLbl.Text = hero.Name;
                if (hpLbl != null) hpLbl.Text = $"HP: {hero.Hp}/{hero.HpMax}";
                if (mpLbl != null) mpLbl.Text = $"MP: {hero.Mp}/{hero.MpMax}";
                if (buffPnl != null) buffPnl.Controls.Clear();

                // _units に登録
                _units.Add(hero.Name, new BattleUnitView(
                    pic,
                    isPlayer: true,
                    nameLbl: nameLbl,
                    hpLbl: hpLbl,
                    mpLbl: mpLbl,
                    buffPanel: buffPnl
                ));

                heroIndex++;
                id++;
            }

            // 敵の生成
            foreach (var enemy in CharacterList.Enemies.Values)
            {

                // 画像取得
                Image? charaImg = GetImageFromResource(enemy.Name);
                if(enemy.Kind == "ボス")
                {
                    Boss = true;
                    enemyIndex++;
                }

                // PictureBox生成
                PictureBox pic = new PictureBox();
                pic.Name = $"pic_{enemy.Name}"; // 名前
                pic.Image = charaImg;
                if(Boss)
                {
                    pic.Size = new Size(200, 200); // サイズ
                    Boss = false;
                }
                else
                {
                    pic.Size = new Size(100, 100); // サイズ
                }
                pic.SizeMode = PictureBoxSizeMode.Zoom;
                pic.BackColor = Color.Transparent;

                pic.Tag = enemy;
                pic.Paint += Pbx_Character_Paint;

                // 座標計算
                pic.Location = new Point(startX_Enemy, startY_Enemy + (stepY_Enemy * enemyIndex));

                // 3. ステータスラベル生成 (敵用・画像に重ねるタイプ)
                Label lblStatus = new Label();
                lblStatus.Text = ""; // 初期は空、UpdateStatusで更新
                lblStatus.AutoSize = true;
                lblStatus.ForeColor = Color.White;

                // フォームに追加
                this.Controls.Add(pic);
                this.Controls.Add(lblStatus);
                pic.BringToFront(); // 画像を前面へ

                // _units に登録
                _units.Add(enemy.Name, new BattleUnitView(
                    pic,
                    isPlayer: false,
                    statusLbl: lblStatus
                ));

                enemyIndex++;
            }
        }

        // コマンド表示
        public void ActivateCommand(ICallback callbackObj)
        {
            this.Timer.Stop();
            _callback = callbackObj.Callback;
            lblName.Text = ((Character)callbackObj).Name;

            // 実体取得
            Character? activeChara = CharacterList.AllChars[lblName.Text];
            if (activeChara == null) return;

            // 選択対象のborderを表示
            if (_units.ContainsKey(((Character)callbackObj).Name))
            {
                var parent = _units[((Character)callbackObj).Name].Pic;
                Image? img = GetImageFromResource("選択");
                _picSelect.Image = img;
                _picSelect.SizeMode = PictureBoxSizeMode.Zoom;
                _picSelect.BackColor = Color.Transparent;

                // 親の設定
                _picSelect.Parent = parent;
                _picSelect.Size = parent.Size;
                _picSelect.Location = new Point(0, 0);
                _picSelect.Visible = true;
                _picSelect.BringToFront();
            }

            foreach (var action in _actions)
            {
                if (activeChara.CanUseCommand(action))
                {
                    lstAction.Items.Add(action);
                }
            }
        }

        private void lstAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_units.ContainsKey(lblName.Text))
            {
                var pic = _units[lblName.Text].Pic;
                pic.BorderStyle = BorderStyle.None;
            }
            if (lstAction.SelectedItem == null) return;
            var nextTarget = (string)lstAction.SelectedItem!;

            
            string useChar = lblName.Text; // 操作中のキャラクターを取得

            // CharacterList からキャラクターの実体を取得
            if (CharacterList.Heroes.ContainsKey(useChar))
            {
                Character currentChar = CharacterList.Heroes[useChar];

                uint cost = ((uint)CommandList.Commands[nextTarget].UseMp);

                if (currentChar.Mp < cost) // MPチェック
                {
                    MessageBox.Show($"MPが足りません！\n消費MP: {cost} / 現在のMP: {currentChar.Mp}");
                    // 選択を強制解除
                    lstAction.SelectedItem = "攻撃";
                    return;
                }
                if (nextTarget == "ジャンプ")
                {
                    if (currentChar.JumpLimit == 0) // ジャンプチェック
                    {
                        MessageBox.Show($"ジャンプは使用できません！\n残りジャンプ使用可能回数: {currentChar.JumpLimit}");
                        // 選択を強制解除
                        lstAction.SelectedItem = "攻撃";
                        return;
                    }
                    else
                    {
                        MessageBox.Show($"ジャンプを使用しますか？\n残りジャンプ使用可能回数: {currentChar.JumpLimit}");
                    }
                }
            }

            lstTarget.Items.Clear();

            // 庇うが選ばれたか？
            bool isCoverCommand = (nextTarget == "庇う");

            switch (CommandList.Commands[nextTarget].Target)
            {
                case "味方":
                    foreach (var tar in CharacterList.Heroes)
                    {
                        // 庇う依存のターゲット不可実装
                        if (isCoverCommand && tar.Value.IsCoverState)
                        {
                            continue;
                        }
                        
                        if(tar.Value.Jump)
                        {
                            continue;
                        }

                        // 自分自身をで除外
                        if (isCoverCommand && tar.Key == lblName.Text)
                        {
                            continue;
                        }

                        if (tar.Value.NotAlive) //戦闘不能者を選択させない
                        {
                            continue;
                        }

                        lstTarget.Items.Add(tar.Key);
                    }
                    break;

                case "敵方":
                    foreach (var tar in CharacterList.Enemies)
                    {
                        if (tar.Value.NotAlive) //戦闘不能者を選択させない
                        {
                            continue;
                        }
                        lstTarget.Items.Add(tar.Key);
                    }
                    break;

                case "自分":
                    lstTarget.Items.Add(lblName.Text);
                    break;
            }
        }

        private void lstTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            _picSelect.Visible = false;

            if (lstTarget.SelectedItem == null) return;
            string act = (string)lstAction.SelectedItem!;
            string tar = (string)lstTarget.SelectedItem!;

            _callback(act, tar);

            _callback = static (string a, string t) => Task.CompletedTask; //クリア

            lstAction.Items.Clear();
            lstTarget.Items.Clear();
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

        /// <summary>
        /// ステータス管理メソッド
        /// </summary>
        /// <param name="name"></param>
        public void UpdateStatus(string name)
        {
            if (!_units.ContainsKey(name)) return;
            BattleUnitView view = _units[name];
            Character? chara = CharacterList.AllChars[name];
            if (chara == null) return;

            view.Update(chara, _poisonImages, _sleepImages, _lastStandImages);
        }

        /// <summary>
        /// 全員のステータス更新メソッド
        /// </summary>
        public void UpdateAllStatus()
        {
            foreach (var key in _units.Keys)
            {
                UpdateStatus(key);
            }
        }

        /// <summary>
        /// リソースから画像を名前で取得するヘルパーメソッド
        /// </summary>
        /// <param name="name">画像のリソース名 (例: "Slime1_Attack")</param>
        /// <returns>画像が見つかればImage, なければnull</returns>
        private Image? GetImageFromResource(string name)
        {
            // リソースマネージャーを使って名前からオブジェクトを取得
            var obj = Properties.Resources.ResourceManager.GetObject(name);
            return obj as Image;
        }

        /// <summary>
        /// 戦闘終了判定
        /// </summary>
        internal void CheckBattleEnd()
        {
            Debug.WriteLine($"終了判断入りまーす");
            // 勝利判定（敵が全員 NotAlive か？）
            bool isVictory = CharacterList.Enemies.Values.All(e => e.NotAlive);

            if (isVictory)
            {
                var result = MessageBox.Show(
                    "勝利しました！\nゲームを終了します",
                    "Victory",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                if (result == DialogResult.OK)
                {
                    Application.Exit();
                }
                return;
            }

            // 敗北判定（味方が全員 NotAlive か？）
            bool isDefeat = CharacterList.Heroes.Values.All(h => h.NotAlive);

            if (isDefeat)
            {
                var result = MessageBox.Show(
                    "敗北しました！\nもう一度挑戦しますか？",
                    "Defeat",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation
                );

                if (result == DialogResult.Yes)
                {
                    Application.Restart();
                }
                else
                {
                    Application.Exit();
                }
                return;
            }
        }


        public void AnimateAttack(string name, string target)
        {
            _animator.AnimateAttack(name, target);
        }
        public void AnimateDefense(string name)
        {
            _animator.AnimateDefense(name);
        }
        public void AnimateChanting(string name)
        {
            _animator.AnimateChanting(name);
        }
        public void AnimateMagic(string name, string target)
        {
            _animator.AnimateMagic(name,target);
        }
        public void AnimatePoisonMagic(string name, string target)
        {
            _animator.AnimatePoisonMagic(name, target);
        }
        public void AnimateSleepMagic(string name, string target)
        {
            _animator.AnimateSleepMagic(name, target);
        }
        public void AnimateMiss(string name)
        {
            _animator.AnimateMiss(name);
        }
        public void AnimateHeal(string name, string target)
        {
            _animator.AnimateHeal(name, target);
        }
        public void AnimateAllHeal(string name, List<string> targetHeroes)
        {
            _animator.AnimateAllHeal(name, targetHeroes);
        }
        public void AnimateCover(string name, string target)
        {
            _animator.AnimateCover(name, target);
        }
        public Task AnimateReceive(string name, int damage)
        {
            return _animator.AnimateReceive(name, damage);
        }
        public async Task AnimatePoison(string name)
        {
            await _animator.AnimatePoison(name);
        }
        public Task AnimateSleep(string name)
        {
            return _animator.AnimateSleep(name);
        }
        public void AnimateGetup(string name)
        {
            _animator.AnimateGetup(name);
        }
        public async Task AnimateDeath(string name)
        {
            await _animator.AnimateDeath(name);
        }
        public Task AnimateDamage(string targetName, int damage)
        {
            return _animator.AnimateDamage(targetName, damage);
        }
        public void AnimateJump(string name)
        {
            _animator.AnimateJump(name);
        }
        public void AnimateLastStand(string name)
        {
            _animator.AnimateLastStand(name);
        }
        public void AnimateSave(string name)
        {
            _animator.AnimateSave(name);
        }
        public void AnimateCounter(string name)
        {
            _animator.AnimateCounter(name);
        }
        public void AnimateZantetsuken(string name)
        {
            _animator.AnimateZantetsuken(name);
        }
        /// <summary>
        /// キャラクターのHPバーを描画する共通イベントハンドラ
        /// </summary>
        private void Pbx_Character_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not PictureBox pbx || pbx.Tag is not Character chara) return;

            // 設定値
            int barHeight = 8;
            int barWidth = pbx.Width - 10;
            int x = 5;
            int y = pbx.Height - 12; 

            // 計算
            if (chara.HpMax == 0) return;

            float ratio = (float)chara.Hp / chara.HpMax;
            if (ratio < 0) ratio = 0;
            if (ratio > 1) ratio = 1;

            Graphics g = e.Graphics;


            using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(100, 0, 0, 0))) // 半透明の黒
            {
                g.FillRectangle(bgBrush, x, y, barWidth, barHeight);
            }
            Color hpColor = Color.LightGreen;
            if (ratio <= 0.25f) hpColor = Color.Red;       // ピンチ（赤）
            else if (ratio <= 0.5f) hpColor = Color.Orange; // 半分以下（オレンジ）

            using (SolidBrush hpBrush = new SolidBrush(hpColor))
            {
                int currentWidth = (int)(barWidth * ratio);
                g.FillRectangle(hpBrush, x, y, currentWidth, barHeight);
            }

            using (Pen borderPen = new Pen(Color.Black))
            {
                g.DrawRectangle(borderPen, x, y, barWidth, barHeight);
            }
        }
    }
}
