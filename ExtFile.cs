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
                    System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
    }
}
