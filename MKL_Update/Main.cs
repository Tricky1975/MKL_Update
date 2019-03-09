// Lic:
// MKL Update
// Main program
// 
// 
// 
// (c) Jeroen P. Broks, 
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
// Please note that some references to data like pictures or audio, do not automatically
// fall under this licenses. Mostly this is noted in the respective files.
// 
// Version: 19.03.09
// EndLic



#undef argdebug


using System;
using TrickyUnits;
using UseJCR6;





namespace MKL_Update
{
    class MKL_Main
    {
        static FlagParse MyArgs;
        static public string MyExe => System.Reflection.Assembly.GetEntryAssembly().Location;
        static public string MyJCR => qstr.Left(MyExe, MyExe.Length - 3) + "jcr";
        static TJCRDIR _JCR=null;
        static public TJCRDIR JCR { get
            {
                if (_JCR == null) {
#if DEBUG
                    Console.WriteLine("Loading JCR6 directory");
#endif
                    _JCR = JCR6.Dir(MyJCR);
                }
                if (_JCR == null) throw new Exception($"JCR6 could not load the required data in {MyJCR}! Error thrown: {JCR6.JERROR}");
                return _JCR;
            }
        }



        private static void InitArgs() {
            JCR6_lzma.Init();
            MyArgs.CrBool("version", false);
            MyArgs.CrBool("help", false);
            MyArgs.CrBool("h", false);
            if (!MyArgs.Parse(true)) throw new Exception("Invalid input");
            if (MyArgs.GetBool("version")) {
                Head();
                Console.WriteLine(MKL.All(true));
                Environment.Exit(0);
            }
            if (MyArgs.GetBool("h") || MyArgs.GetBool("help")) {
                Head();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("MKL_Update ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[<dir1> [<dir2> [<dir3>....]]]");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("\tUpdates the MKL data and license blocks in given directories. If none given, the current directory will be done");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("MKL_Update ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("-h");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("MKL_Update ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("-help");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("\tShows this help text");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("MKL_Update ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("-version");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("\tShows detailed version information");
                Console.WriteLine("\n\n\nThe next programming languages are supported by MKL_Update");
                foreach (string k in JCR.Entries.Keys) if (qstr.Prefixed(k, "EXT/")) {
                        TGINI T = GINI.ReadFromLines(JCR.ReadLines(k));
                        // Console.WriteLine($"{k} => {qstr.Prefixed(k, "EXT/")}"); // debug line
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.Write(k.ToLower());
                        for (int i = k.Length; i < 30; i++) Console.Write(" ");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine($" {T.C("LANGUAGE")}");
                    }
                Console.ResetColor();
                Environment.Exit(0);
            }
        }

        static void Head() {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("MaKe License - Update");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"Version: {MKL.Newest}");
            Console.Write("\n\nCurrect directory: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
            Console.ResetColor();
            Console.Write("Lauched file:      ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(MyExe);
            Console.ResetColor();
            Console.Write("JCR Date file:     ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(MyJCR);
            Console.ResetColor();
        }

        static void Main(string[] args)
        {
            JCR6_lzma.Init();
            MKL_Run.MKL_See();
            MKL.Lic("", "");
            MKL.Version("MKL Update - Main.cs","19.03.09");
#if argdebug
            for (int i = 0; i < args.Length; i++) Console.Write(qstr.sprintf("%d:%s", i, args[i])); 
#endif
            MyArgs = new FlagParse(args);
            InitArgs();
            Head();
            string[] dirs;
            if (MyArgs.Args.Length == 0) dirs = new string[] { System.IO.Directory.GetCurrentDirectory() }; else dirs = MyArgs.Args;
            foreach (string q in dirs) {
                Console.Write("Will process:      ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(q);
                Console.ResetColor();
            }
            foreach (string dir in dirs) MKL_Run.Run(dir);

#if DEBUG
            // Only meant for running in debug mode, as in release mode this doesn't matter.
            // Visual Studio closes the window immediately and I may need the last output, you see!
            Console.WriteLine("Hit any key"); 
            Console.ReadKey();

#else
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Ok"); // Homage to my old P2000T experiences!
            Console.ForegroundColor = ConsoleColor.Gray;
#endif
        }
    }
}

