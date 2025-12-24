using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace OOP_RPG
{
    /// <summary>
    /// アニメーションを管理するクラス
    /// </summary>
    internal class Animate
    {
        //外部から参照
        private readonly Dictionary<string, BattleUnitView> _units;
        private readonly System.Windows.Forms.Timer _gameTimer;
        private readonly Form _parentForm;
        private readonly Action _updateStatusAction;
        private readonly Image[] _damageImages;

        // エフェクト用PictureBox
        private PictureBox? _saveEffect = null;

        //コンストラクタ
        internal Animate(
            Dictionary<string, BattleUnitView> units,
            System.Windows.Forms.Timer timer,
            Form form,
            Action updateStatusAction,
            Image[] damageImages
            )
        {
            _units = units;
            _gameTimer = timer;
            _parentForm = form;
            _updateStatusAction = updateStatusAction;
            _damageImages = damageImages;
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
        /// エフェクト用のPictureBoxを動的に生成して配置するメソッド
        /// </summary>
        /// <param name="resourceName">リソース名</param>
        /// <param name="parent">対象キャラのPictureBox</param>
        /// <returns>生成されたPictureBox</returns>
        public PictureBox CreateEffect(string resourceName, Control parent)
        {
            // リソースから画像取得
            Image? img = GetImageFromResource(resourceName);
            if (img == null) return null!;

            PictureBox p = new PictureBox();
            p.Image = img;
            p.SizeMode = PictureBoxSizeMode.Zoom;
            p.BackColor = Color.Transparent;

            // 親の設定
            p.Parent = parent;
            p.Size = parent.Size;
            p.Location = new Point(0, 0);
            p.Visible = true;
            p.BringToFront();

            return p;
        }

        /// <summary>
        /// 攻撃アニメーションメソッド
        /// </summary>
        /// <param name="name"></param>
        public async void AnimateAttack(string name, string target)
        {
            _gameTimer.Stop();
            _updateStatusAction();
            // 名前でPictureBoxを探す
            if (!_units.ContainsKey(name))
            {
                _gameTimer.Start();
                return;
            }
            var pic = _units[name].Pic;
            pic.Image = _units[name].DefaultImage;

            // ゆっくり対象に近づく
            for (int i = 0; i < 10; i++) // 1000
            {
                if (_units[name].IsPlayer) pic.Left -= 10;
                else pic.Left += 10;

                await Task.Delay(100);
            }
            await Task.Delay(500); //1500
            // 画像変更
            Image? attackImg = GetImageFromResource($"{name}_Attack");
            if (attackImg != null)
            {
                pic.Image = attackImg;
            }
            // 一気に近づいて攻撃
            var tar = _units[target].Pic.Location;
            pic.Location = tar;
            if (_units[name].IsPlayer) pic.Left += 200;
            else pic.Left -= 200;

            _parentForm.Left += 10;
            await Task.Delay(50);
            _parentForm.Left -= 10;
            await Task.Delay(50);

            // 攻撃後の余韻
            await Task.Delay(900); // 50+50+900 = 1000

            // 元の位置に戻る
            Character? SelfChar = null;
            SelfChar = CharacterList.AllChars[name];
            if (SelfChar.Counter)
            {
                pic.Image = GetImageFromResource($"{name}_Counter");
            }
            else
            {
                pic.Image = _units[name].DefaultImage;
            }

            pic.Location = _units[name].HomeLocation;
            _gameTimer.Start();
        }

        /// <summary>
        /// 防御アニメーションメソッド
        /// </summary>
        /// <param name="name"></param>
        public async void AnimateDefense(string name)
        {
            _gameTimer.Stop();
            // 名前でPictureBoxを探す
            if (!_units.ContainsKey(name))
            {
                _gameTimer.Start();
                return;
            }
            var pic = _units[name].Pic;

            // 画像変更
            Image? blockImg = GetImageFromResource($"{name}_Block");
            if (blockImg != null)
            {
                pic.Image = blockImg;
            }

            // ぐるぐる回る
            pic.Top += 10;
            await Task.Delay(50);
            pic.Left += 10;
            await Task.Delay(50);
            pic.Top -= 10;
            await Task.Delay(50);
            pic.Left -= 10;
            await Task.Delay(50);
            pic.Top += 10;
            await Task.Delay(50);
            pic.Left += 10;
            await Task.Delay(50);
            pic.Top -= 10;
            await Task.Delay(50);
            pic.Left -= 10;
            await Task.Delay(50); // 400

            // シールドアニメーション
            _updateStatusAction();

            PictureBox? shield = CreateEffect("防御", pic);
            await Task.Delay(800);
            shield.Dispose();

            // 元の位置に戻る
            pic.Location = _units[name].HomeLocation;
            _gameTimer.Start();
        }

        /// <summary>
        /// 魔法詠唱アニメーションメソッド
        /// </summary>
        /// <param name="name"></param>
        public async void AnimateChanting(string name)
        {
            _gameTimer.Stop();
            // 名前でPictureBoxを探す
            if (!_units.ContainsKey(name))
            {
                _gameTimer.Start();
                return;
            }
            var pic = _units[name].Pic;
            pic.Image = _units[name].DefaultImage;

            // 画像変更
            Image? ChantingImg = GetImageFromResource($"{name}_Chanting");
            if (ChantingImg != null)
            {
                pic.Image = ChantingImg;
            }

            // ぐるぐる回る
            pic.Top += 10;
            await Task.Delay(50);
            pic.Left += 10;
            await Task.Delay(50);
            pic.Top -= 10;
            await Task.Delay(50);
            pic.Left -= 10;
            await Task.Delay(50);
            pic.Top += 10;
            await Task.Delay(50);
            pic.Left += 10;
            await Task.Delay(50);
            pic.Top -= 10;
            await Task.Delay(50);
            pic.Left -= 10;
            await Task.Delay(50); // 400

            // 詠唱アニメーション
            _updateStatusAction();
            PictureBox? Chanting = CreateEffect("魔法詠唱", pic);
            await Task.Delay(1000);
            Chanting.Dispose();

            // 元の位置に戻る
            pic.Location = _units[name].HomeLocation;
            _gameTimer.Start();
        }

        /// <summary>
        /// 魔法攻撃アニメーションメソッド
        /// </summary>
        /// <param name="name"></param>
        public async void AnimateMagic(string name, string target)
        {
            _gameTimer.Stop();
            // 名前でPictureBoxを探す
            if (!_units.ContainsKey(name))
            {
                _gameTimer.Start();
                return;
            }
            var pic = _units[name].Pic;
            pic.Image = _units[name].DefaultImage;

            // ターゲットが庇われていたらターゲット変更
            Character? targetChar = null;
            targetChar = CharacterList.AllChars[target];
            if (targetChar.CoveredByName != "")
            {
                //庇ってくれているキャラを探す
                Character? coveringChar = null;
                if (CharacterList.AllChars.ContainsKey(targetChar.CoveredByName))
                {
                    coveringChar = CharacterList.AllChars[targetChar.CoveredByName];
                }
                // 見つかったらターゲット変更
                if (coveringChar != null)
                {
                    target = coveringChar.Name;
                }
            }

            // 少し前進
            for (int i = 0; i < 10; i++) //1000
            {
                if (_units[name].IsPlayer) pic.Left -= 10;
                else pic.Left += 10;

                await Task.Delay(100);
            }

            await Task.Delay(500); //1000+500=1500

            // 画像変更
            Image? attackImg = GetImageFromResource($"{name}_Attack");
            if (attackImg != null)
            {
                pic.Image = attackImg;
            }

            // 魔法発動
            PictureBox? effectPic = CreateEffect("攻撃魔法", _units[target].Pic);
            for (int i = 0; i < 10; i++) //1000
            {
                _parentForm.Left += 10;
                await Task.Delay(50);
                _parentForm.Left -= 10;
                await Task.Delay(50);
            }
            effectPic.Visible = false;
            effectPic.Parent = null;
            effectPic.Dispose();


            // 元の位置に戻る
            pic.Location = _units[name].HomeLocation;
            pic.Image = _units[name].DefaultImage;
            _gameTimer.Start();
        }

        /// <summary>
        /// 毒魔法アニメーションメソッド
        /// </summary>
        /// <param name="name"></param>
        public async void AnimatePoisonMagic(string name, string target)
        {
            _gameTimer.Stop();
            // 名前でPictureBoxを探す
            if (!_units.ContainsKey(name))
            {
                _gameTimer.Start();
                return;
            }
            var pic = _units[name].Pic;
            pic.Image = _units[name].DefaultImage;

            // ターゲットが庇われていたらターゲット変更
            Character? targetChar = null;
            targetChar = CharacterList.AllChars[target];
            if (targetChar.CoveredByName != "")
            {
                //庇ってくれているキャラを探す
                Character? coveringChar = null;
                if (CharacterList.AllChars.ContainsKey(targetChar.CoveredByName))
                {
                    coveringChar = CharacterList.AllChars[targetChar.CoveredByName];
                }
                // 見つかったらターゲット変更
                if (coveringChar != null)
                {
                    target = coveringChar.Name;
                }
            }

            // 少し前進
            for (int i = 0; i < 10; i++) //1000
            {
                if (_units[name].IsPlayer) pic.Left -= 10;
                else pic.Left += 10;

                await Task.Delay(100);
            }

            await Task.Delay(500); //1000+500=1500

            // 画像変更
            Image? attackImg = GetImageFromResource($"{name}_Attack");
            if (attackImg != null)
            {
                pic.Image = attackImg;
            }

            // 魔法発動
            PictureBox? effectPic = CreateEffect("毒", _units[target].Pic);
            for (int i = 0; i < 10; i++) //1000
            {
                _parentForm.Left += 10;
                await Task.Delay(50);
                _parentForm.Left -= 10;
                await Task.Delay(50);
            }
            effectPic.Visible = false;

            // 元の位置に戻る
            pic.Location = _units[name].HomeLocation;
            pic.Image = _units[name].DefaultImage;
            _gameTimer.Start();
        }

        /// <summary>
        /// 睡眠魔法アニメーションメソッド
        /// </summary>
        /// <param name="name"></param>
        public async void AnimateSleepMagic(string name, string target)
        {
            _gameTimer.Stop();
            // 名前でPictureBoxを探す
            if (!_units.ContainsKey(name))
            {
                _gameTimer.Start();
                return;
            }
            var pic = _units[name].Pic;
            pic.Image = _units[name].DefaultImage;

            // ターゲットが庇われていたらターゲット変更
            Character? targetChar = null;
            targetChar = CharacterList.AllChars[target];
            if (targetChar.CoveredByName != "")
            {
                //庇ってくれているキャラを探す
                Character? coveringChar = null;
                if (CharacterList.AllChars.ContainsKey(targetChar.CoveredByName))
                {
                    coveringChar = CharacterList.AllChars[targetChar.CoveredByName];
                }
                // 見つかったらターゲット変更
                if (coveringChar != null)
                {
                    target = coveringChar.Name;
                }
            }

            var targetpic = _units[target].Pic;

            // 少し前進
            for (int i = 0; i < 10; i++) //1000
            {
                if (_units[name].IsPlayer) pic.Left -= 10;
                else pic.Left += 10;

                await Task.Delay(100);
            }

            await Task.Delay(500); //1000+500=1500

            // 画像変更
            Image? attackImg = GetImageFromResource($"{name}_Attack");
            if (attackImg != null)
            {
                pic.Image = attackImg;
            }

            // 魔法発動
            PictureBox? effectPic = CreateEffect("睡眠", _units[target].Pic);
            for (int i = 0; i < 10; i++) //1000
            {
                _parentForm.Left += 10;
                await Task.Delay(50);
                _parentForm.Left -= 10;
                await Task.Delay(50);
            }
            effectPic.Visible = false;

            // ターゲットの画像変更
            string sleepImgPath = $@"C:\Users\ksion\Downloads\RPG\{target}_sleep.png";
            if (System.IO.File.Exists(sleepImgPath))
            {
                targetpic.Image = Image.FromFile(sleepImgPath);
            }

            // 元の位置に戻る
            pic.Location = _units[name].HomeLocation;
            pic.Image = _units[name].DefaultImage;
            _gameTimer.Start();
        }


        /// <summary>
        /// 攻撃失敗アニメーションメソッド
        /// </summary>
        /// <param name="name"></param>
        public async void AnimateMiss(string name)
        {
            _gameTimer.Stop();
            // 名前でPictureBoxを探す
            if (!_units.ContainsKey(name))
            {
                _gameTimer.Start();
                return;
            }
            var pic = _units[name].Pic;
            pic.Image = _units[name].DefaultImage;

            // 少し前進
            for (int i = 0; i < 10; i++) //1000
            {
                if (_units[name].IsPlayer) pic.Left -= 10;
                else pic.Left += 10;

                await Task.Delay(100);
            }

            await Task.Delay(500); //1000+500=1500

            // 画像変更
            Image? attackImg = GetImageFromResource($"{name}_Attack");
            if (attackImg != null)
            {
                pic.Image = attackImg;
            }

            // 魔法発動
            PictureBox? Miss = CreateEffect("失敗1.6", pic);
            await Task.Delay(1500); //1500
            Miss.Dispose();


            // 元の位置に戻る
            pic.Location = _units[name].HomeLocation;
            pic.Image = _units[name].DefaultImage;
            _gameTimer.Start();
        }


        /// <summary>
        /// 回復魔法アニメーションメソッド
        /// </summary>
        /// <param name="name"></param>
        public async void AnimateHeal(string name, string target)
        {
            _gameTimer.Stop();
            // 名前でPictureBoxを探す
            if (!_units.ContainsKey(name))
            {
                _gameTimer.Start();
                return;
            }
            var pic = _units[name].Pic;
            pic.Image = _units[name].DefaultImage;

            // 少し前進
            for (int i = 0; i < 10; i++) //1000
            {
                if (_units[name].IsPlayer) pic.Left -= 10;
                else pic.Left += 10;

                await Task.Delay(100);
            }
            await Task.Delay(500); //1000+500=1500

            // 画像変更
            Image? attackImg = GetImageFromResource($"{name}_Attack");
            if (attackImg != null)
            {
                pic.Image = attackImg;
            }

            //回復魔法発動
            _updateStatusAction();
            PictureBox? Heal = CreateEffect("回復魔法", _units[target].Pic); 

            for (int i = 0; i < 5; i++) //1000
            {
                pic.Top += 10;
                await Task.Delay(50);
                pic.Left += 10;
                await Task.Delay(50);
                pic.Top -= 10;
                await Task.Delay(50);
                pic.Left -= 10;
                await Task.Delay(50);
            }

            Heal.Dispose();

            // 元の位置に戻る
            pic.Location = _units[name].HomeLocation;
            pic.Image = _units[name].DefaultImage;
            _gameTimer.Start();
        }

        /// <summary>
        /// ケアルラアニメーションメソッド
        /// </summary>
        /// <param name="name"></param>
        public async void AnimateAllHeal(string name, List<string> targetHeroes)
        {
            _gameTimer.Stop();
            // 名前でPictureBoxを探す
            if (!_units.ContainsKey(name))
            {
                _gameTimer.Start();
                return;
            }
            var pic = _units[name].Pic;
            pic.Image = _units[name].DefaultImage;

            // 少し前進
            for (int i = 0; i < 10; i++) //1000
            {
                if (_units[name].IsPlayer) pic.Left -= 10;
                else pic.Left += 10;

                await Task.Delay(100);
            }
            await Task.Delay(500); //1000+500=1500

            // 画像変更
            Image? attackImg = GetImageFromResource($"{name}_ケアルラ");
            if (attackImg != null)
            {
                pic.Image = attackImg;
            }

            //回復魔法発動
            // エフェクトリスト
            var activeEffects = new List<PictureBox>();
            foreach (string target in targetHeroes)
            {
                if (_units.ContainsKey(target))
                {
                    PictureBox? effect = CreateEffect("回復魔法", _units[target].Pic);
                    activeEffects.Add(effect);
                }
            }

            for (int i = 0; i < 5; i++) //1000
            {
                pic.Top += 10;
                await Task.Delay(50);
                pic.Left += 10;
                await Task.Delay(50);
                pic.Top -= 10;
                await Task.Delay(50);
                pic.Left -= 10;
                await Task.Delay(50);
            }

            foreach (var effect in activeEffects)
            {
                effect.Dispose();
            }

            // 元の位置に戻る
            pic.Location = _units[name].HomeLocation;
            pic.Image = _units[name].DefaultImage;
            _gameTimer.Start();
        }


        /// <summary>
        /// 庇うアニメーションメソッド
        /// </summary>
        /// <param name="name"></param>
        public async void AnimateCover(string name, string target)
        {
            _gameTimer.Stop();
            // 名前でPictureBoxを探す
            if (!_units.ContainsKey(name))
            {
                _gameTimer.Start();
                return;
            }
            var pic = _units[name].Pic;
            pic.Image = _units[name].DefaultImage;
            var tar = _units[target].Pic.Location;

            // 右へ移動
            // ゆっくりにしたい
            for (int i = 0; i < 10; i++) //1000
            {
                if (_units[name].IsPlayer)
                    pic.Left -= 10;
                else
                    pic.Left += 10;
                await Task.Delay(100);
            }


            await Task.Delay(500); //500+1000 = 1500

            // 対象を庇いに行く
            _updateStatusAction();
            pic.Location = tar;
            if (_units[name].IsPlayer) pic.Left -= 200;
            else pic.Left += 200;

            // 庇う画像に切り替え
            Image? attackImg = GetImageFromResource($"{name}_Attack");
            if (attackImg != null)
            {
                pic.Image = attackImg;
            }

            // 余韻
            await Task.Delay(1000); //1000

            _gameTimer.Start();
        }

        /// <summary>
        /// 攻撃被弾アニメーションメソッド
        /// </summary>
        /// <param name="name"></param>
        public async Task AnimateReceive(string name, int damage)
        {
            _gameTimer.Stop();
            // 名前でPictureBoxを探す
            if (!_units.ContainsKey(name))
            {
                _gameTimer.Start();
                return;
            }
            var pic = _units[name].Pic;

            // 元の位置を保存
            var originalLocation = pic.Location;
            var originalImage = pic.Image;

            await Task.Delay(1600);
            _updateStatusAction();
            // 画像変更
            Image? ReceiveImg = GetImageFromResource($"{name}_Receive");
            if (ReceiveImg != null)
            {
                pic.Image = ReceiveImg;
            }

            // 飛び上がる
            pic.Top -= 50;
            // ダメージ表示
            await AnimateDamage(name, damage);

            // 元の位置に戻る
            pic.Location = originalLocation;
            pic.Image = originalImage;

            // 死亡時処理
            bool isDead = false;
            if (CharacterList.Heroes.ContainsKey(name))
            {
                isDead = CharacterList.Heroes[name].NotAlive;
            }
            else if (CharacterList.Enemies.ContainsKey(name))
            {
                isDead = CharacterList.Enemies[name].NotAlive;
            }
            if (isDead)
            {
                await AnimateDeath(name);
            }

            //_gameTimer.Start();
        }


        /// <summary>
        /// 毒ダメージアニメーションメソッド
        /// </summary>
        /// <param name="name"></param>
        public async Task AnimatePoison(string name)
        {
            _gameTimer.Stop();
            // 名前でPictureBoxを探す
            if (!_units.ContainsKey(name))
            {
                _gameTimer.Start();
                return;
            }
            var pic = _units[name].Pic;

            // 元の位置、画像を保存
            var originalLocation = pic.Location;
            var originalImage = pic.Image;

            // 画像変更
            Image? ReceiveImg = GetImageFromResource($"{name}_Receive");
            if (ReceiveImg != null)
            {
                pic.Image = ReceiveImg;
            }


            // 毒アニメーション
            PictureBox? Poison = CreateEffect("毒", pic);
            await AnimateDamage(name, 5);
            Poison.Dispose();

            // 元の位置、画像に戻る
            pic.Location = originalLocation;
            pic.Image = originalImage;
            _updateStatusAction();

            // 死亡時処理
            bool isDead = false;
            if (CharacterList.Heroes.ContainsKey(name))
            {
                isDead = CharacterList.Heroes[name].NotAlive;
            }
            else if (CharacterList.Enemies.ContainsKey(name))
            {
                isDead = CharacterList.Enemies[name].NotAlive;
            }
            if (isDead)
            {
                await AnimateDeath(name);
            }

            _gameTimer.Start();
        }

        /// <summary>
        /// 睡眠アニメーションメソッド
        /// </summary>
        /// <param name="name"></param>
        public async Task AnimateSleep(string name)
        {
            _gameTimer.Stop();
            // 名前でPictureBoxを探す
            if (!_units.ContainsKey(name))
            {
                _gameTimer.Start();
                return;
            }
            var pic = _units[name].Pic;

            // 画像変更
            Image? SleepImg = GetImageFromResource($"{name}_Sleep");
            if (SleepImg != null)
            {
                pic.Image = SleepImg;
            }
            // 睡眠アニメーション
            PictureBox? Sleep = CreateEffect("睡眠", pic);
            await Task.Delay(900);
            Sleep.Dispose();

            // 元の位置
            pic.Location = _units[name].HomeLocation;
            if (!CharacterList.AllChars[name].Sleeping) pic.Image = _units[name].DefaultImage;
            _updateStatusAction();

            _gameTimer.Start();
        }

        /// <summary>
        /// 起床アニメーションメソッド
        /// </summary>
        /// <param name="name"></param>
        public void AnimateGetup(string name)
        {
            _gameTimer.Stop();

            if (!_units.ContainsKey(name))
            {
                _gameTimer.Start();
                return;
            }
            var pic = _units[name].Pic;

            pic.Image = _units[name].DefaultImage;
            pic.Location = _units[name].HomeLocation;

            _gameTimer.Start();
        }


        /// <summary>
        /// 死亡時アニメーションメソッド
        /// </summary>
        /// <param name="name"></param>
        public async Task AnimateDeath(string name)
        {
            _gameTimer.Stop();
            _updateStatusAction();
            // 名前でPictureBoxを探す
            if (!_units.ContainsKey(name))
            {
                _gameTimer.Start();
                return;
            }
            var pic = _units[name].Pic;

            // 画像変更
            Image? deathImg = GetImageFromResource($"{name}_death");
            if (deathImg != null)
            {
                pic.Image = deathImg;
            }

            PictureBox? deathEffect = CreateEffect("ゴースト", pic);
            deathEffect.Size = new Size(50, 50);
            deathEffect.Location = new Point(
                    (pic.Width - deathEffect.Width) / 2,
                    (pic.Height - deathEffect.Height) / 2
                );

            for (int i = 0; i < 3; i++) //750
            {
                deathEffect.Top -= 20;
                await Task.Delay(100);
            }
            deathEffect.Dispose();

            pic.Location = _units[name].HomeLocation;
        }

        /// <summary>
        /// ダメージ表示アニメーション
        /// </summary>
        /// <param name="targetName"></param>
        /// <param name="damage"></param>
        public async Task AnimateDamage(string targetName, int damage)
        {
            // ターゲットの場所を取得
            if (!_units.ContainsKey(targetName)) return;
            var targetPic = _units[targetName].Pic;

            // 数字を文字列に変換
            string numStr = damage.ToString();

            // 生成したPictureBoxを管理するリスト
            List<PictureBox> createdNumbers = new List<PictureBox>();

            // 文字の大きさ
            int targetSize = 40;
            // 1文字の幅
            int charWidth = targetSize;
            // 全体の幅
            int totalWidth = charWidth * numStr.Length;

            // 表示開始位置（キャラの頭上中央）
            int startX = targetPic.Left + (targetPic.Width - totalWidth) / 2;
            int startY = targetPic.Top - 10;

            // 数字画像を生成して並べる
            for (int i = 0; i < numStr.Length; i++)
            {
                // 数字を1つ取り出す
                int val = int.Parse(numStr[i].ToString());

                // 新しいPictureBoxを作る
                PictureBox p = new PictureBox();

                p.Image = _damageImages[val];

                p.Size = new Size(targetSize, targetSize);

                p.SizeMode = PictureBoxSizeMode.Zoom;
                p.BackColor = Color.Transparent;

                // 位置合わせ（1文字ずつ右にずらす）
                p.Location = new Point(startX + (i * charWidth), startY);

                // フォームに貼り付け
                _parentForm.Controls.Add(p);
                p.BringToFront();
                p.Visible = true;

                createdNumbers.Add(p);
            }

            // 浮かぶアニメーション
            for (int i = 0; i < 10; i++) // 500
            {
                foreach (var p in createdNumbers)
                {
                    p.Top -= 3; // 上へ移動
                }
                await Task.Delay(50);
            }
            _updateStatusAction();
            // 少し停止
            await Task.Delay(300); // 500+300=800

            // 削除(メモリぶっこわれ防止)
            foreach (var p in createdNumbers)
            {
                p.Visible = false;
                _parentForm.Controls.Remove(p); // 画面から消す
                p.Dispose();             // メモリから消す
            }
        }

        /// <summary>
        /// ジャンプアニメーションメソッド
        /// </summary>
        /// <param name="name"></param>
        public async void AnimateJump(string name)
        {
            _gameTimer.Stop();
            // 名前でPictureBoxを探す
            if (!_units.ContainsKey(name))
            {
                _gameTimer.Start();
                return;
            }
            var pic = _units[name].Pic;

            // 画像変更
            Image? JumpImg = GetImageFromResource($"{name}_Jump");
            if (JumpImg != null)
            {
                pic.Image = JumpImg;
            }

            // しゃがんで
            for (int i = 0; i < 3; i++) //1200
            {
                pic.Top += 20;
                await Task.Delay(400);
            }

            // ジャンプ
            for (int i = 0; i < 4; i++) //200
            {
                pic.Top -= 300;
                await Task.Delay(50);
            }

            _gameTimer.Start();
        }

        /// <summary>
        /// ラストスタンドアニメーションメソッド
        /// </summary>
        /// <param name="name"></param>
        public async void AnimateLastStand(string name)
        {
            _gameTimer.Stop();
            // 名前でPictureBoxを探す
            if (!_units.ContainsKey(name))
            {
                _gameTimer.Start();
                return;
            }
            var pic = _units[name].Pic;

            // 画像変更
            Image? LastStandImg = GetImageFromResource($"{name}_LastStand");
            if (LastStandImg != null)
            {
                pic.Image = LastStandImg;
            }

            // しゃがんで
            //for (int i = 0; i < 3; i++) //1200
            //{
            //    pic.Top += 20;
            //    await Task.Delay(400);
            //}
            await Task.Delay(1000);

            pic.Image = _units[name].DefaultImage;
            pic.Location = _units[name].HomeLocation;

            _gameTimer.Start();
        }

        /// <summary>
        /// 溜めるアニメーションメソッド
        /// </summary>
        /// <param name="name"></param>
        public async void AnimateSave(string name)
        {
            _gameTimer.Stop();
            // 名前でPictureBoxを探す
            if (!_units.ContainsKey(name))
            {
                _gameTimer.Start();
                return;
            }
            var pic = _units[name].Pic;

            // 画像変更
            Image? SaveImg = GetImageFromResource($"{name}_Save");
            if (SaveImg != null)
            {
                pic.Image = SaveImg;
            }

            await Task.Delay(900);

            // セーブアニメーション
            _saveEffect = CreateEffect("セーブ", pic);

            // 元の位置
            pic.Location = _units[name].HomeLocation;

            _gameTimer.Start();

        }

        /// <summary>
        /// 見切りアニメーションメソッド
        /// </summary>
        /// <param name="name"></param>
        public async void AnimateCounter(string name)
        {
            _gameTimer.Stop();
            // 名前でPictureBoxを探す
            if (!_units.ContainsKey(name))
            {
                _gameTimer.Start();
                return;
            }
            var pic = _units[name].Pic;

            // 画像変更
            Image? CounterImg = GetImageFromResource($"{name}_Counter");
            if (CounterImg != null)
            {
                pic.Image = CounterImg;
            }
            for (int i = 0; i < 3; i++) //300
            {
                _parentForm.Left += 10;
                await Task.Delay(50);
                _parentForm.Left -= 10;
                await Task.Delay(50);
            }

            await Task.Delay(900);

            // 元の位置
            pic.Location = _units[name].HomeLocation;

            _gameTimer.Start();

        }

        /// <summary>
        /// 斬鉄剣アニメーションメソッド
        /// </summary>
        /// <param name="name"></param>
        public async void AnimateZantetsuken(string name)
        {
            _gameTimer.Stop();
            _saveEffect!.Dispose();
            // 名前でPictureBoxを探す
            if (!_units.ContainsKey(name))
            {
                _gameTimer.Start();
                return;
            }
            var pic = _units[name].Pic;

            // 画像変更
            Image? AttackImg = GetImageFromResource($"{name}_Zan");
            if (AttackImg != null)
            {
                pic.Image = AttackImg;
            }

            // 特殊アニメーション「画面分裂」
            // ビットマップを作成
            int w = _parentForm.ClientSize.Width;
            int h = _parentForm.ClientSize.Height;
            Bitmap bmp = new Bitmap(w, h);
            _parentForm.DrawToBitmap(bmp, _parentForm.ClientRectangle);

            int splitY = h / 2; // 真ん中

            // 上半分の画像
            Bitmap topBmp = bmp.Clone(new Rectangle(0, 0, w, splitY), bmp.PixelFormat);
            // 下半分の画像
            Bitmap bottomBmp = bmp.Clone(new Rectangle(0, splitY, w, h - splitY), bmp.PixelFormat);

            // 表示用のPictureBoxを作成
            PictureBox topPic = new PictureBox();
            topPic.Image = topBmp;
            topPic.Size = new Size(w, splitY);
            topPic.Location = new Point(0, 0);
            topPic.SizeMode = PictureBoxSizeMode.Normal;

            PictureBox bottomPic = new PictureBox();
            bottomPic.Image = bottomBmp;
            bottomPic.Size = new Size(w, h - splitY);
            bottomPic.Location = new Point(0, splitY); // 下半分の位置
            bottomPic.SizeMode = PictureBoxSizeMode.Normal;

            // 「切れた隙間」を表現する白いパネル（背景）
            Panel flashPanel = new Panel();
            flashPanel.BackColor = Color.White;
            flashPanel.Size = _parentForm.ClientSize;
            flashPanel.Location = new Point(0, 0);

            // フォームに追加
            _parentForm.Controls.Add(topPic);
            _parentForm.Controls.Add(bottomPic);
            _parentForm.Controls.Add(flashPanel);

            topPic.BringToFront();
            bottomPic.BringToFront();

            // 「ズレる」アニメーション
            // @@SE追加予定

            // 少し待つ
            await Task.Delay(100);

            // 一気にズラす
            for (int i = 0; i < 5; i++)
            {
                // 上半分は「左上」へ、下半分は「右下」へズラす（斜めに切れた感じが出る）
                topPic.Top -= 2;
                topPic.Left -= 2;

                bottomPic.Top += 2;
                bottomPic.Left += 2;

                await Task.Delay(10); // 高速で動かす
            }

            // ズレた状態で静止
            await Task.Delay(1000);

            // 後始末
            _parentForm.Controls.Remove(topPic);
            _parentForm.Controls.Remove(bottomPic);
            _parentForm.Controls.Remove(flashPanel);
            topPic.Dispose();
            bottomPic.Dispose();
            flashPanel.Dispose();
            topBmp.Dispose();
            bottomBmp.Dispose();
            bmp.Dispose();

            await Task.Delay(8500);

            // 元の位置に戻る
            pic.Image = _units[name].DefaultImage;
            pic.Location = _units[name].HomeLocation;
            _gameTimer.Start();
        }
    }
}
