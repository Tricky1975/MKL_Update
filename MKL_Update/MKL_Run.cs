using System;
using System.Collections.Generic;
using System.IO;
using TrickyUnits;
using UseJCR6;

namespace MKL_Update
{
    class MKL_Run
    {

        // Statics to get on the move
        static public void MKL_See() {
            MKL.Lic("Q", "GPL");
            MKL.Version("Q", "9.10.11");
        }

        static public void Run(string dir) {
            var locRun = new MKL_Run(dir);
            Console.Write("\n\nProcessing: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(dir);
            Console.ResetColor();
            locRun.Go();
        }


        // Actual class
        static List<string> StaticExt = null;
        List<string> Ext {
            get {
                if (StaticExt == null) {
#if DEBUG
                    Console.WriteLine("DEBUG: Fetching supported extensions");
#endif
                    StaticExt = new List<string>();
                    foreach (string n in MKL_Main.JCR.Entries.Keys)
                        if (qstr.Prefixed(n, "EXT/")) StaticExt.Add(qstr.Right(n,n.Length-4));
                }
                return StaticExt;
            }
        }
        readonly string dir;

        MKL_Run(string dodir) { dir = dodir; }
        TGINI Data = new TGINI();
        string GINIFile => $"{dir}/License.MKL.gini";

        void Error(string e) {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("ERROR! ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(e);
            Console.ResetColor();
            Console.Beep(2000, 100);
            Console.Beep(1000, 500);
        }

        void Ask(string key, string defaultval, string q) {
            while (Data.C(key) == "") {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(q);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Data.D(key, Console.ReadLine());
                Console.ForegroundColor = ConsoleColor.Gray;
                Data.SaveSource(GINIFile);
            }
        }
        

        bool GetGINI() {
            if (File.Exists(qstr.RemSuffix(GINIFile, ".gini") + ".ini")) {
                Console.Beep();
                Console.WriteLine("Renaming old ini file to GINI!");
                File.Move(qstr.RemSuffix(GINIFile, ".gini") + ".ini",GINIFile);
            }
            if (!File.Exists(GINIFile)) {
                for (int i = 0; i < 10; i++) {
                    Console.Beep(2000, 1000);
                    Console.Beep(1000, 1000);
                }
                Console.WriteLine("This directory has not been initized as a MKL based project!");
                Console.Write("Do you want it to be a MKL based project ? <Y/N> ");
                if (Console.ReadLine().ToUpper() != "Y") return false;
                QuickStream.SaveString(GINIFile, "[rem]\nVoid Dark(){ cprintf(\"Void the darkness\"); }\n");
            }
            Data = GINI.ReadFromFile(GINIFile);
            Ask("Project", dir, "Please name the project: ");
            return true;
        }

        void Go() {
            string[] files;
            if (!GetGINI()) return;
            List<string> sources = new List<string>();
            int count = 0;
            try {
                files = FileList.GetTree(dir);
                Directory.CreateDirectory($"{dir}/MKL_Backup");
            } catch (Exception e){
                Error(e.Message);
                return;
            }
            Console.WriteLine("Re-arranging backups");
            if (File.Exists($"{dir}/MKL_Backup/10.JCR")) File.Delete($"{dir}/MKL_Backup/10.JCR");
            for(int i = 9; i > 0; i--) {
                if (File.Exists($"{dir}/MKL_Backup/{i}.JCR")) File.Move($"{dir}/MKL_Backup/{i}.JCR",$"{dir}/MKL_Backup/{i+1}.JCR");
            }
            Console.WriteLine("Backing up source files");
            var jout = new TJCRCreate($"{dir}/MKL_Backup/1.JCR", "lzma");
            foreach(string f in files) {
                count++;
                if (count==f.Length || count%50==0) {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write($"{count}/{f.Length}\r");
                    Console.ForegroundColor=ConsoleColor.Gray;
                }
                var s = f.Split('.');
                if (s.Length > 0) {
                    var e = s[s.Length - 1];
#if DEBUG
                    Console.WriteLine($"DEBUG: {count}: {f} {e} {Ext.Contains(e.ToUpper())}");
#endif
                    if (Ext.Contains(e.ToUpper())) {
                        sources.Add(f);
                        jout.AddFile($"{dir}/{f}", f, "lzma", "", "This is a backup created by MKL_Update");
                    }
                }
            }
            jout.Close();
        }
        
    }
}
