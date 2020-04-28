using KHBC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHBC.LD.APCM
{
    public static class ApcmKeyConf
    {
        public static string CmdQueue;
        public static string BdciQueue;
        public static string MdciQueue;
        public static string DataCollection;

        public static void Init()
        {
            var deviceFullName = $"{SysConf.KeyAssemblyLine}:{SysConf.ModuleName}"; 
            CmdQueue = $"{SysConf.KeyAssemblyLine}:{SysConf.ModuleName}:CMD:Q";
            BdciQueue = $"{SysConf.KeyPlant}:BDCI:Q";
            MdciQueue = $"{SysConf.KeyPlant}:MDCI:Q";
            DataCollection = $"{deviceFullName}:DC";
        }
    }
}
