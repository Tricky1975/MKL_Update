using System.Collections.Generic;
using TrickyUnits;

namespace MKL_Update
{
    class License
    {
        static Dictionary<string, License> DICT;
        TGINI DATA = new TGINI();

        public static TGINI Get(string key) {
            if (!DICT.ContainsKey(key)) {
                DICT[key] = new License();
                DICT[key].DATA = GINI.ReadFromLines(MKL_Main.JCR.ReadLines($"LIC/{key}"));
            }
            return DICT[key].DATA;        
        }

        public static string[] Fields(string key) => Get(key).List("FIELD").ToArray();
    }
}
