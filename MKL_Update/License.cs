// Lic:
// MKL Update
// License Manager
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




using System;
using System.Collections.Generic;
using TrickyUnits;



namespace MKL_Update {

    class License    {
        static Dictionary<string, License> DICT = new Dictionary<string, License>();
        TGINI DATA = new TGINI();



        public static void VER() {
            MKL.Version("MKL Update - License.cs","19.05.08");
            MKL.Lic    ("MKL Update - License.cs","GNU General Public License 3");
        }



        public static TGINI Get(string key) {
            if (!DICT.ContainsKey(key)) {
                DICT[key] = new License();
                DICT[key].DATA = GINI.ReadFromLines(MKL_Main.JCR.ReadLines($"LIC/{key}"));
                if (UseJCR6.JCR6.JERROR != "") throw new Exception(UseJCR6.JCR6.JERROR);
            }
            return DICT[key].DATA;        
        }

        public static string[] Fields(string key) => Get(key).List("FIELDS").ToArray();
    }
}


