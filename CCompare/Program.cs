using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCompare
{
    class Program
    {
        static void Main(string[] args)
        {

            ContrastCompare.CompareHelper c = new ContrastCompare.CompareHelper();

            if (args.Length != 3)
            {
                Console.WriteLine("Usage: CCompare compareFile1 compareFile2 outFile");
            } else
            {
                c.Compare(args[0], args[1], args[2]);
            }

        }
    }
}
