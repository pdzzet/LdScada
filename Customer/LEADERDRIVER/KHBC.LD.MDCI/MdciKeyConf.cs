using KHBC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHBC.LD.MDCI
{
    public static class MdciKeyConf
    {
        public static string CmdQueue { get; set; }

        public static void Init()
        {
            CmdQueue = $"{SysConf.KeyPlant}:{SysConf.ModuleName}:Q";
        }
    }
}
