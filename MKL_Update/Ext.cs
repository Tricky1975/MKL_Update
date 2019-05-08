// Lic:
// MKL Update
// Extension Manager
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





using System;
using System.Collections.Generic;
using UseJCR6;
using TrickyUnits;



namespace MKL_Update
{
    class Extension
    {
        static Dictionary<string, Extension> DICT = new Dictionary<string, Extension>();
        TGINI DATA = new TGINI();

        public static void VER(){
            MKL.Version("MKL Update - Ext.cs","19.03.09");
            MKL.Lic    ("MKL Update - Ext.cs","GNU General Public License 3");
        }

        public static TGINI Get(string e) {
            string ec = e.ToUpper();
            if (!DICT.ContainsKey(ec))
                DICT[ec] = new Extension();
                DICT[ec].DATA = GINI.ReadFromLines(MKL_Main.JCR.ReadLines($"EXT/{ec}"));            
            return DICT[ec].DATA;
        }
    }
}

