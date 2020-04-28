using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KHBC.LD.APCM.BEntity
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable()]
    internal struct RfidBtaryInfo : ISerializable
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.U4)]
        public uint[] OpInfo;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.U1)]
        public byte[] OpCheckRes;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo01;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo02;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo03;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo04;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo05;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo06;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo07;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo08;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo09;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo10;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo11;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo12;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo13;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo14;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo15;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo16;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo17;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo18;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo19;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo20;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo21;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo22;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo23;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo24;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo25;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo26;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo27;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo28;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo29;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo30;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo31;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo32;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo33;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo34;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo35;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo36;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo37;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo38;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo39;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo40;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo41;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo42;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo43;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo44;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo45;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo46;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo47;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo48;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] OpCheckInfo49;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 49, ArraySubType = UnmanagedType.U1)]
        public uint[] SpotCheckRes;

        #region ISerializable 成员
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion
    }
}
