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
    public struct RfidPalletInfo : ISerializable
    {
        //16进制
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string ProdcutSn;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U2)]
        public ushort AssemblyLineId;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U2)]
        public ushort WaitOp;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U2)]
        public uint[] DoneOp;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.U4)]
        public uint[] Station;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] OpCheckInfo;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] StationCheckInfo;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] StandbyCheckInfo;


        #region ISerializable 成员
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion
    }
}
