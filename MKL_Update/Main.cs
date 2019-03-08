#undef argdebug

using System;
using System.Collections.Generic;
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
            MKL.Version("", "10.10.10");
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
#endif

        }
    }
}
