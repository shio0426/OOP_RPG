using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_RPG
{
    internal class ExtFile
    {
        public static readonly string Path = 
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
            @"\source\repos\OOP_RPG\data";
    }
}
