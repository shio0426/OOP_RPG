using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_RPG
{

    internal class Command
    {
        public string Action { get; private set; } = "";
        public string Target { get; private set; } = "";
        public string Kind { get; private set; } = "";
        public int TakeHp { get; private set; }
        public int UseMp { get; private set; }

        public bool IsBossOnly { get; private set; }
    }

    internal static class CommandList
    {
        public static readonly Dictionary<string, Command> Commands = [];
        static CommandList()
        {
            LoadCommand();
        }

        public static void LoadCommand()
        {
            try
            {
                var csv = Csv.Load<Command>($@"{ExtFile.Path}\Command.csv");
                foreach (var it in csv)
                {
                    Commands.Add(it.Action, it);
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
