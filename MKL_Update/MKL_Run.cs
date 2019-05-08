// Lic:
// MKL Update
// Running the entire process
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
// Version: 19.05.08
// EndLic





#undef ChangeDebug



using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TrickyUnits;
using UseJCR6;



namespace MKL_Update
{
    class MKL_Run
    {

        // Statics to get on the move
        static public void MKL_See() {
            MKL.Lic    ("MKL Update - MKL_Run.cs","GNU General Public License 3");
            MKL.Version("MKL Update - MKL_Run.cs","19.05.08");
        }



        static public void Run(string dir) {
            var odir = Directory.GetCurrentDirectory(); Directory.SetCurrentDirectory(dir);
            var locRun = new MKL_Run(dir);            
            Console.Write("\n\nProcessing: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(dir);
            Console.ResetColor();
            locRun.Go();
            Directory.SetCurrentDirectory(odir);
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
                        if (qstr.Prefixed(n, "EXT/")) StaticExt.Add(qstr.Right(n, n.Length - 4));
                }
                return StaticExt;
            }
        }



        static Dictionary<int, string> StaticLics = null;
        Dictionary<int, string> Lic {
            get {                
                if (StaticLics == null) {
                    StaticLics = new Dictionary<int, string>();
                    int c = 0;
                    foreach(var ilic in MKL_Main.JCR.Entries.Values) {
                        if (qstr.Prefixed(ilic.Entry.ToUpper(), "LIC/")) {
                            StaticLics[c] = qstr.Right(ilic.Entry,ilic.Entry.Length-4);
                            c++;
                        }
                    }
                }
                return StaticLics;
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
                Data.D(key, Console.ReadLine().Trim());
                if (Data.C(key) == "") Data.D(key, defaultval);
                Console.ForegroundColor = ConsoleColor.Gray;                
            }
            Data.SaveSource(GINIFile);
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
            Data.CL("KNOWN");
            Data.CL("SKIPFILE");
            Data.CL("SKIPDIR");
            Ask("Project", dir, "Please name the project: ");
            return true;
        }



        string ThisYear {

        get {

                return DateTime.Now.ToString("yyyy");

            }

        }



        string CrVersion => DateTime.Now.ToString("yy.MM.dd");



        string FormBlock(string f) {
            var ret = new StringBuilder("");
            var e = qstr.ExtractExt(f);
            if (Data.C($"Lic {f}") == "") {
                Console.BackgroundColor = ConsoleColor.DarkMagenta;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("License block not yet known");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
                var mx = 0;
                for (int i = 0; Lic.ContainsKey(i); i++) {
                    mx = i;
                    Console.WriteLine($" {i + 1} = {Lic[i]}");
                }
                int keuze = -1;
                do {
                    Console.Write("Please make your choice: ");
                    keuze = qstr.ToInt(Console.ReadLine()) - 1;
                } while (!Lic.ContainsKey(keuze));
                Data.D($"Lic {f}", Lic[keuze]);
                Data.SaveSource(GINIFile);
            }
            if (qstr.Prefixed(Data.C($"Lic {f}").ToUpper(),"LIC/")) {
                Error("Invalid License Value -- Fixing!");
                Data.D($"Lic {f}", qstr.StripDir(Data.C($"Lic {f}"))); // Result of an early bug, but it haunts the rest of my debugging sessions!
            }

            var mlic = Data.C($"Lic {f}");
            var cyear = Data.C($"IYEAR {f}");
            Ask($"CYEAR {f}", ThisYear, "Intial year of this project: ");
            if (cyear == "") cyear = Data.C($"IYEAR {f}");
            else if (qstr.Suffixed(cyear, ThisYear)) {
                cyear += $", {ThisYear}";
                Data.D($"IYEAR {f}", cyear);
                Data.SaveSource(GINIFile);
            }

            foreach (string fld in License.Fields(mlic)) {
                Ask($"Licfield - {f} - {mlic} - {fld}", License.Get(mlic).C($"DEFAULT[{fld}]"), fld + ": ");
            }

            var ldata = License.Get(mlic);
            var edata = Extension.Get(e);
            if (edata.C("START") == "" || edata.C("END") == "") throw new Exception($"Language '{e}' has is not complete!");
            ret.Append( $"{edata.C("START")}\n");
            foreach(string licline in ldata.List("License")) {
                string tl = licline.Replace("<\\n>","\n");
                tl = tl.Replace("[\\n]", "\n");
                tl = tl.Replace("[$thisfile]", f);
                tl = tl.Replace("<$thisfile>", f);
                tl = tl.Replace("[$years]", Data.C("IYEAR"));
                tl = tl.Replace("<$years>", Data.C("IYEAR"));
                tl = tl.Replace("[$version]", CrVersion);
                tl = tl.Replace("<$version>", CrVersion);
                foreach(string fld in License.Fields(Data.C($"Lic {f}"))) {
                    tl = tl.Replace($"[{fld}]", Data.C($"Licfield - {f} - {mlic} - {fld}"));
                    tl = tl.Replace($"<{fld}>", Data.C($"Licfield - {f} - {mlic} - {fld}"));
                }
                if (edata.C("PREF")!="") tl = $"{edata.C("PREF")} {tl}";
                ret.Append($"{tl}\n");
            }
            ret.Append( $"{edata.C("END")}\n");
            return ret.ToString();
        }



        string[] ReplMKL(string f,List<string> src) {
            var ret = new List<string>();
            var e = qstr.ExtractExt(f).ToUpper();
            foreach(string sl in src) {
                var rk = sl;
                //Console.WriteLine($"HUHI> {rk}");
                if ((Extension.Get(e).C("VPREF") != "" && qstr.Prefixed(rk.Trim(), Extension.Get(e).C("VPREF"))) || (Extension.Get(e).C("VSUFF") != "" && qstr.Prefixed(rk.Trim(), Extension.Get(e).C("VSUFF")))) {
                    string ident = "";
                    int ipos = 0;
                    while (rk[ipos] == 32 || rk[ipos] == 9) {
                        ident += qstr.Chr(rk[ipos]);
                        ipos++;
                    }
                    rk = ident + Extension.Get(e).C("VFULL");
                    rk = rk.Replace("<$version>", CrVersion); // Right(Year(), 2) + "." + Right("0" + Month(), 2) + "." + Right("0" + Day(), 2))
                    rk = rk.Replace("<$project>", Data.C("Project"));
                    rk = rk.Replace("<$dir>", qstr.ExtractDir(f));
                    rk = rk.Replace("<$file>", qstr.StripDir(f));
                    rk = rk.Replace("<$fullfille>", f);
                    rk = rk.Replace("<$license>", Data.C("Lic " + f));
                }
                if ((Extension.Get(e).C("LPREF") != "" && qstr.Prefixed(rk.Trim(), Extension.Get(e).C("LPREF"))) || (Extension.Get(e).C("LSUFF") != "" && qstr.Prefixed(rk.Trim(), Extension.Get(e).C("LSUFF")))) {
                    string ident = "";
                    int ipos = 0;
                    while (rk[ipos] == 32 || rk[ipos] == 9) {
                        ident += qstr.Chr(rk[ipos]);
                        ipos++;
                    }
                    rk = ident + Extension.Get(e).C("LFULL");
                    rk = rk.Replace("<$version>", CrVersion); // Right(Year(), 2) + "." + Right("0" + Month(), 2) + "." + Right("0" + Day(), 2))
                    rk = rk.Replace("<$project>", Data.C("Project"));
                    rk = rk.Replace("<$dir>", qstr.ExtractDir(f));
                    rk = rk.Replace("<$file>", qstr.StripDir(f));
                    rk = rk.Replace("<$fullfille>", f);
                    rk = rk.Replace("<$license>", Data.C("Lic " + f));
                }


                //Console.WriteLine($"HUHO> {rk}"); Console.ReadLine(); Console.ReadLine(); Console.ReadLine();

                ret.Add(rk);

            }

            return ret.ToArray();

        }



        void SaveFile(string f,string[] code) {

            var bt = QuickStream.WriteFile(f);

            foreach (string ln in code) bt.WriteString($"{ln}\n",true);

            bt.Close();

        }

        



        bool ReplaceBlock(string f) {
            var e = qstr.ExtractExt(f);
            var src = new List<string>();
            src.Add(FormBlock(f));
            var BT = QuickStream.ReadFile(f);
            var fullcontent = BT.ReadString((int)BT.Size);
            fullcontent = fullcontent.Replace("\r\n", "\n");
            var fcs = fullcontent.Split('\n');
            var remblock = false;
            //while (!BT.EOF) {
            foreach(string L in fcs) { 
                //var L = qstr.RemSuffix(BT.ReadLine(),"\r");
                if (L == Extension.Get(e).C("START")) remblock = true;
                if (!remblock) src.Add(L);
                if (L == Extension.Get(e).C("END")) remblock = false;
                //Console.WriteLine($"HUHUH>{L}");
                //Console.WriteLine($"Block:{remblock} -- {L}");
                //Console.ReadLine();
            }
            BT.Close();
            var tar = ReplMKL(f,src);
            SaveFile(f, tar);
            return true;
        }



        bool AddBlock(string f) {
            var e = qstr.ExtractExt(f);
            var src = new List<string>();
            src.Add(FormBlock(f));
            var BT = QuickStream.ReadFile(f);            
            while (!BT.EOF) {
                var L = BT.ReadLine();
                src.Add(L);
            }
            BT.Close();
            var tar = ReplMKL(f,src);
            SaveFile(f, tar);
            return true;
        }



        bool Changed(string f) {
            var ret = false;
            var hash = qstr.md5(QuickStream.LoadString(f));
            var info = new FileInfo(f);
            ret = ret || hash != Data.C($"HASH {f}");
            // This checkup is new, and I don't want adeptions due to changes made by outdated MKL updaters.
            if (Data.C($"SIZE {f}") != "") ret = ret || $"{info.Length}" != Data.C($"SIZE {f}");
            // This checkup is new, and I don't want adeptions due to changes made by outdated MKL updaters. Also the way this is constructed will only work properly in C#, so if this tool is ever rewritten in another language, this checkup will be ignored.
            // C#TM means "C# time", but I guess that was obvious :P
            if (Data.C($"C#TM {f}") != "") ret = ret || $"{info.CreationTime.ToString()}" != Data.C($"C#TM {f}");
#if ChangeDebug
            Console.WriteLine($"Outcome to the check is {ret}");
            Console.WriteLine($"hashcheck:  {hash}/{Data.C($"HASH {f}")}/{hash == Data.C($"HASH {f}")}");
            Console.WriteLine($"sizecheck:  {info.Length}/{Data.C($"SIZE {f}")}");
#endif 

            return ret;

        }



        void UpdateData(string f) {
            var info = new FileInfo(f);
            Data.D($"HASH {f}", qstr.md5(QuickStream.LoadString(f)));
            Data.D($"SIZE {f}", $"{info.Length}");
            Data.D($"C#TM {f}", $"{info.CreationTime.ToString()}");
            Data.SaveSource(GINIFile);
        }



        bool Act(string f) {
            string d = qstr.ExtractDir(f);
            string e = qstr.ExtractExt(f);
            if (Data.List("KNOWN").Contains(f)) return Changed(f);
            if (Data.List("SKIPFILE").Contains(f)) return false;
            if (Data.List("SKIPDIR").Contains(d)) return false;
            Console.Beep();
            do {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"**** {f} ****");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.BackgroundColor = ConsoleColor.Black;
                string remblock = "";
                var BT = QuickStream.ReadFile(f);
                var L = BT.ReadLine();
                if (L == Extension.Get(e).C("START")) {
                    remblock = L + "\n";
                    do {
                        L = BT.ReadLine();
                        if (L == Extension.Get(e).C("END")) break;
                        if (BT.EOF) {
                            Error("Unclosed license block!");
                            return false;
                        }
                        remblock += L + "\n";
                                } while (true);
                }
                BT.Close();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("");
                Console.WriteLine("1 = Replace original block and add this file to the auto-updateble files");
                Console.WriteLine("2 = Add license block and add this file to the auto-updatable files");
                Console.WriteLine("3 = Skip this file for now");
                Console.WriteLine("4 = Skip this file forever");
                if (d != "") Console.WriteLine("5 = Skip this entire directory forever");
                Console.Write("Please tell me what to do: ");
                var c = Console.ReadKey();
                Console.WriteLine("\n"); // Yes, two lines :P
                switch (c.Key) {
                    case ConsoleKey.D1:
                        Data.Add("KNOWN", f);
                        Data.SaveSource(GINIFile);
                        if (!ReplaceBlock(f)) Error("Block replacement failed!");
                        return false; // Always false, or the system will do this again for no reason!
                    case ConsoleKey.D2:
                        Data.Add("KNOWN", f);
                        Data.SaveSource(GINIFile);
                        if (!AddBlock(f)) Error("Block addition failed!");
                        return false; // Always false, or the system will do this again for no reason!
                    case ConsoleKey.D3:
                        return false;
                    case ConsoleKey.D4:
                        Data.Add("SKIPFILE", f);
                        Data.SaveSource(GINIFile);
                        return false;
                    case ConsoleKey.D5:
                        if (d == "") break;
                        Data.Add("SKIPDIR", d);
                        Data.SaveSource(GINIFile);
                        return false;
                }
            } while (true);            
        }


        void Look(string f) {

             if (Act(f)) {

                Console.WriteLine($"Updating {f}");                

                if (!ReplaceBlock(f)) Error("License block replacement failed");

                UpdateData(f);

            }

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

                    Console.Write($"{count}/{files.Length}\r");

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

            Console.WriteLine("Processing source files");

            foreach (string f in sources) {

                count++;

                if (count == f.Length || count % 50 == 0) {

                    Console.ForegroundColor = ConsoleColor.DarkBlue;

                    Console.Write($"{count}/{sources.Count}\r");

                    Console.ForegroundColor = ConsoleColor.Gray;

                }

                Look(f);

                count = 0;

            }

        }

        

    }

}


