using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KHBC.LD.APCM.BEntity
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable()]
    internal struct P5CentInfoList
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public P5CentInfo[] p5CentInfos;
    }
}
