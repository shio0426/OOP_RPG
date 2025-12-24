using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_RPG
{
    internal class HeroEntity
    {
        public string Name { get; set; } = "";
        public uint Hp { get; set; }
        public uint Mp { get; set; }
        public uint Speed { get; set; }
        public char Gender { get; set; }
        public string Profession { get; set; } = "";
    }

    internal class EnemyEntity
    {
        public string Name { get; set; } = "";
        public uint Hp { get; set; }
        public uint Mp { get; set; }
        public uint Speed { get; set; }
        public string Kind { get; set; } = "";
    }
    internal class CharacterList
    {
        public static readonly Dictionary<string, Hero> Heroes = [];
        public static readonly Dictionary<string, Enemy> Enemies = [];
        public static readonly Dictionary<string, Character> AllChars = [];

        //static CharacterList()
        //{
        //    CreateHero();
        //    CreateEnemy();
        //}

        public static void Initialize(string enemyFileName)
        {
            // 既にデータがある場合は重複しないようにクリアしておく
            Heroes.Clear();
            Enemies.Clear();
            AllChars.Clear();

            CreateHero();
            CreateEnemy(enemyFileName); // ファイル名を渡す
        }

        private static void CreateHero()
        {
            try
            {
                var csv = Csv.Load<HeroEntity>($@"{ExtFile.Path}\Hero.csv");
                foreach (var it in csv)
                {
                    var hero = new Hero(it.Name, it.Hp, it.Mp, it.Speed,
                    it.Gender, it.Profession);
                    Heroes.Add(hero.Name, hero);
                    AllChars.Add(hero.Name, hero);
                }
            }
            catch (Exception ex)
            {
                TraceLog.Write($"Error: {ex.Message}");
                Environment.Exit(1);
            }
        }

        private static void CreateEnemy(string fileName)
        {
            try
            {
                // ファイル名に"Boss"が含まれているかでボスモードを判定
                bool isBossMode = fileName.Contains("Boss");
                var csv = Csv.Load<EnemyEntity>($@"{ExtFile.Path}\{fileName}");
                foreach (var it in csv)
                {
                    Enemy enemy;

                    if (isBossMode)
                    {
                        // ボスとして生成 (ルーチンで動く)
                        enemy = new Boss(it.Name, it.Hp, it.Mp, it.Speed, it.Kind);
                    }
                    else
                    {
                        // 通常の敵として生成 (ランダムで動く)
                        enemy = new Enemy(it.Name, it.Hp, it.Mp, it.Speed, it.Kind);
                    }

                    Enemies.Add(enemy.Name, enemy);
                    AllChars.Add(enemy.Name, enemy);
                }
            }
            catch (Exception ex)
            {
                TraceLog.Write($"Error: {ex.Message}");
                Environment.Exit(1);
            }
        }
    }
}
