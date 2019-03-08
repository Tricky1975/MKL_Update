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

        public static TGINI Get(string e)
        {
            string ec = e.ToUpper();
            if (!DICT.ContainsKey(ec))
                DICT[ec] = new Extension();
                DICT[ec].DATA = GINI.ReadFromLines(MKL_Main.JCR.ReadLines($"EXT/{ec}"));            
            return DICT[ec].DATA;
        }
    }
}
