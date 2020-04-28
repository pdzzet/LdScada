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
    internal struct P5CentInfo
    {
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
        //public string UID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
        public string BatchCode;
    }
}
