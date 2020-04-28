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
    internal struct RfidStationInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
        public string UID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
        public string BatchCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 150)]
        public string MaterialCode;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 49)]
        public ProductSn[] ProductSnL;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string AssemblyLineId;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] MesToMc;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] McToMes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string Status;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public byte[] LocaNumbInfo;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string NgStatus;
    }
}
