using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KHBC.Core.Extend;

namespace KHBC.Modbus
{
    public abstract class ModBusBase
    {
        public string Name { get; set; }
        public abstract bool IsConnected { get; }
        public abstract Result Open();
        public abstract void Close();
        public abstract Result<byte[]> Read(string address, ushort length = 1);

        public abstract Result<short> ReadInt16(string address);
        public abstract Result<short[]> ReadInt16(string address, ushort length);

        public abstract Result<bool> ReadBool(string address);
        public abstract Result<bool[]> ReadBool(string address, ushort length);

        public abstract Result<float> ReadFloat(string address);
        public abstract Result<float[]> ReadFloat(string address, ushort length);

        public abstract Result<bool> ReadCoil(string address);
        public abstract Result<bool[]> ReadCoil(string address, ushort length);

        public abstract Result Write(string address, byte[] value);

        public abstract Result WriteInt16(string address, short value);
        public abstract Result WriteInt16(string address, short[] values);

        public abstract Result WriteFloat(string address, float value);
        public abstract Result WriteFloat(string address, float[] values);
        public abstract Result WriteBool(string address, bool[] values);

        public abstract Result WriteCoil(string address, bool[] values);
        public abstract Result WriteCoil(string address, bool value);

        public abstract Result WriteOneRegister(string address, short value);
    }
}
