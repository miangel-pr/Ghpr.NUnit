using Ghpr.NUnit.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghpr.NUnit.Console
{
    class Program
    {
        public static void Main(string[] args)
        {
            var lines = System.IO.File.ReadAllLines(@"Resources\\01Test.txt");

            GhprEventListener mel = new GhprEventListener();

            foreach (var line in lines)
            {
                mel.OnTestEvent(line);
            }
        }
    }
}
