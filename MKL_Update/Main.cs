using System;
using System.Collections.Generic;
using TrickyUnits;

namespace MKL_Update
{
    class MKL_Main
    {
        static FlagParse MyArgs;

        private static void InitArgs() {
            MyArgs.CrBool("version", false);
            MyArgs.CrBool("help", false);
            MyArgs.CrBool("h", false);
            if (!MyArgs.Parse(true)) throw new Exception("Invalid input");
            if (MyArgs.GetBool("version")) {
                Console.WriteLine(MKL.All(true));
                Environment.Exit(0);
            }
        }

        static void Main(string[] args)
        {
            MKL.Lic("", "");
            MKL.Version("", "10.10.10");
            MyArgs = new FlagParse(args);
            InitArgs();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("MaKe License - Update");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"Version: {MKL.Newest}");
            Console.ReadKey();
        }
    }
}
