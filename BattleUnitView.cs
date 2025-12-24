using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OOP_RPG
{
    internal class BattleUnitView
    {
        // パブリ
        public PictureBox Pic { get; }
        public Point HomeLocation { get; }
        public Image DefaultImage { get; }
        public bool IsPlayer { get; }

        // ラベル
        private Label? _nameLbl;
        private Label? _hpLbl;
        private Label? _mpLbl;
        private Label? _statusLbl;
        private FlowLayoutPanel? _buffPanel;

        private PictureBox? _lastStandPic;

        // コンストラクタ
        public BattleUnitView(
            PictureBox pic,
            bool isPlayer,
            Label? nameLbl = null,
            Label? hpLbl = null, 
            Label? mpLbl = null,
            Label? statusLbl = null, 
            FlowLayoutPanel? buffPanel = null
            )
        {
            Pic = pic;
            HomeLocation = pic.Location;
            DefaultImage = pic.Image;
            IsPlayer = isPlayer;

            _nameLbl = nameLbl;
            _hpLbl = hpLbl;
            _mpLbl = mpLbl;
            _statusLbl = statusLbl;
            _buffPanel = buffPanel;

            _lastStandPic = CreateEffect("闇", Pic);
            SetupEnemyLabel(); // 敵ならラベルの透過処理などを行う
        }

        /// <summary>
        /// エネミーのラベル処理メソッド
        /// </summary>
        private void SetupEnemyLabel()
        {
            if (!IsPlayer && _statusLbl != null)
            {
                _statusLbl.Parent = Pic;
                _statusLbl.BackColor = Color.Transparent;
                _statusLbl.Location = new Point(0, 0);
                Pic.Paint += (s, e) => {
                    using (var brush = new SolidBrush(Color.FromArgb(150, 0, 0, 0)))
                        e.Graphics.FillRectangle(brush, _statusLbl.Bounds);
                };
            }
        }

        /// <summary>
        /// ステータス更新メソッド
        /// </summary>
        /// <param name="chara"></param>
        /// <param name="buffImages"></param>
        public void Update(Character chara, Image?[] poisonImages, Image?[] sleepImages, Image?[] LastStandImages)
        {
            if (_nameLbl != null) _nameLbl.Text = chara.Name;
            if (_hpLbl != null) _hpLbl.Text = $"HP: {chara.Hp}/{chara.HpMax}";
            if (_mpLbl != null) _mpLbl.Text = $"MP: {chara.Mp}/{chara.MpMax}";
            if (_statusLbl != null) _statusLbl.Text = $"HP: {chara.Hp}/{chara.HpMax}\nMP: {chara.Mp}/{chara.MpMax}";

            UpdateBuffs(chara, poisonImages, sleepImages, LastStandImages);

            if (_lastStandPic != null)
            {
                if (chara.LastStand && !_lastStandPic.Visible)
                {
                    _lastStandPic.Visible = true;
                }
                else if (!chara.LastStand && _lastStandPic.Visible)
                {
                    _lastStandPic.Visible = false;
                }
            }
            //Pic.Invalidate();
        }

        /// <summary>
        /// バフアイコン更新処理
        /// </summary>
        /// <param name="chara"></param>
        /// <param name="buffImages"></param>
        private void UpdateBuffs(Character chara, Image?[] poisonImages, Image?[] sleepImages, Image?[] LastStandImages)
        {
            if (_buffPanel != null)
            {
                // 古いアイコンを削除
                foreach (Control c in _buffPanel.Controls) c.Dispose();
                _buffPanel.Controls.Clear();

                // 防御
                if (chara.Block) AddIcon(Properties.Resources.盾1);

                // 詠唱
                if (chara.SpellCasting) AddIcon(Properties.Resources.詠唱1);

                // 毒
                if (chara.Poisoned)
                {
                    int count = (int)chara.PoisonedCount;
                    if (count >= 1 && count < poisonImages.Length)
                    {
                        AddIcon(poisonImages[count]);
                    }
                }

                // 睡眠
                if (chara.Sleeping)
                {
                    int count = (int)chara.SleepingCount;
                    if (count >= 1 && count < sleepImages.Length)
                    {
                        AddIcon(sleepImages[count]);
                    }
                }

                // 不死
                if (chara.LastStand)
                {
                    int count = (int)chara.LastStandCount;
                    if (count >= 1 && count < LastStandImages.Length)
                    {
                        AddIcon(LastStandImages[count]);
                    }
                }
            }

            // 敵用
            if (_statusLbl != null)
            {
                _statusLbl.Text = $"HP: {chara.Hp}/{chara.HpMax}\nMP: {chara.Mp}/{chara.MpMax}";
            }
        }

        /// <summary>
        /// バフアイコン追加用のメソッド
        /// </summary>
        /// <param name="img"></param>
        private void AddIcon(Image? img)
        {
            if (img == null || _buffPanel == null) return;

            PictureBox p = new PictureBox();
            p.Image = img;
            p.Size = new Size(33, 33);
            p.SizeMode = PictureBoxSizeMode.Zoom;
            p.BackColor = Color.Transparent;
            p.Margin = new Padding(1);
            _buffPanel.Controls.Add(p);
        }

        public PictureBox CreateEffect(string resourceName, Control parent)
        {
            // リソースから画像取得
            var obj = Properties.Resources.ResourceManager.GetObject(resourceName);
            Image? img = obj as Image;
            if (img == null) return null!;

            PictureBox p = new PictureBox();
            p.Image = img;
            p.SizeMode = PictureBoxSizeMode.Zoom;
            p.BackColor = Color.Transparent;

            // 親の設定
            p.Parent = parent;
            p.Size = parent.Size;
            p.Location = new Point(0, 0);
            p.Visible = false;
            p.BringToFront();

            return p;
        }

    }
}
