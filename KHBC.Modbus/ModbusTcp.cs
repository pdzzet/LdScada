using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.ModBus;
using KHBC.Core.Extend;

namespace KHBC.Modbus
{
    public class ModbusTcp : ModBusBase
    {
        class ModbusTcpClient:ModbusTcpNet
        {
            public bool IsConnected => CoreSocket?.Connected??false;
        }

        private ModbusTcpClient modbus;

        public override bool IsConnected => modbus.IsConnected;
        /// <summary>
        /// 指定服务器地址，端口号，客户端自己的站号来初始化
        /// </summary>
        /// <param name="ipAddress">服务器的Ip地址</param>
        /// <param name="port">服务器的端口号</param>
        /// <param name="station">客户端自身的站号</param>
        /// <param name="isAddressStartWithZero">在Modbus服务器的设备里，大部分的设备都是从地址0开始的，有些特殊的设备是从地址1开始</param>
        public  ModbusTcp(string ipAddress, int port = 502, byte station = 0x01,  bool isAddressStartWithZero = true)
        {
            modbus = new ModbusTcpClient();
            modbus.IpAddress = ipAddress;
            modbus.Port = port;
            modbus.Station = station;
            modbus.AddressStartWithZero = isAddressStartWithZero;
        }

        ~ModbusTcp()
        {
            modbus.ConnectClose();
            modbus.Dispose();
        }


        public override Result Open()
        {
           var op= modbus.ConnectServer();
            return new Result {IsSuccess = op.IsSuccess, Msg = op.Message};
        }

        public override void Close()
        {
            modbus.ConnectClose();
        }

        public override Result<byte[]> Read(string address, ushort length=1)
        {
            var op = modbus.Read(address, length);
            return new Result<byte[]> { IsSuccess = op.IsSuccess, Data = op.Content, Msg = op.Message };
        }

        public override Result<short> ReadInt16(string address)
        {
            var op = modbus.ReadInt16(address);
            return new Result<short> { IsSuccess = op.IsSuccess, Data = op.Content, Msg = op.Message };
        }

        public override Result<short[]> ReadInt16(string address, ushort length)
        {
            var op = modbus.ReadInt16(address, length);
            return new Result<short[]> { IsSuccess = op.IsSuccess, Data = op.Content, Msg = op.Message };
        }

        public override Result<bool> ReadBool(string address)
        {
            var op = modbus.ReadBool(address);
            return new Result<bool> { IsSuccess = op.IsSuccess, Data = op.Content, Msg = op.Message };
        }

        public override Result<bool[]> ReadBool(string address, ushort length)
        {
            var op = modbus.ReadBool(address, length);
            return new Result<bool[]> { IsSuccess = op.IsSuccess, Data = op.Content, Msg = op.Message };
        }

        public override Result<float> ReadFloat(string address)
        {
            var op = modbus.ReadFloat(address);
            return new Result<float> { IsSuccess = op.IsSuccess, Data = op.Content, Msg = op.Message };
        }

        public override Result<float[]> ReadFloat(string address, ushort length)
        {
            var op = modbus.ReadFloat(address, length);
            return new Result<float[]> { IsSuccess = op.IsSuccess, Data = op.Content, Msg = op.Message };
        }

        public override Result<bool> ReadCoil(string address)
        {
            var op = modbus.ReadCoil(address);
            return new Result<bool> { IsSuccess = op.IsSuccess, Data = op.Content, Msg = op.Message };
        }

        public override Result<bool[]> ReadCoil(string address, ushort length)
        {
            var op = modbus.ReadCoil(address, length);
            return new Result<bool[]> { IsSuccess = op.IsSuccess, Data = op.Content, Msg = op.Message };
        }

        public override Result Write(string address, byte[] value)
        {
            var op = modbus.Write(address, value);
            return new Result { IsSuccess = op.IsSuccess, Msg = op.Message };
        }

        public override Result WriteInt16(string address, short value)
        {
            var op = modbus.Write(address, value);
            return new Result { IsSuccess = op.IsSuccess, Msg = op.Message };
        }

        public override Result WriteInt16(string address, short[] values)
        {
            var op = modbus.Write(address, values);
            return new Result { IsSuccess = op.IsSuccess, Msg = op.Message };
        }

        public override Result WriteFloat(string address, float value)
        {
            var op = modbus.Write(address, value);
            return new Result { IsSuccess = op.IsSuccess, Msg = op.Message };
        }

        public override Result WriteFloat(string address, float[] values)
        {
            var op = modbus.Write(address, values);
            return new Result { IsSuccess = op.IsSuccess, Msg = op.Message };
        }

        public override Result WriteBool(string address, bool[] values)
        {
            var op = modbus.Write(address, values);
            return new Result { IsSuccess = op.IsSuccess, Msg = op.Message };
        }

        public override Result WriteCoil(string address, bool[] values)
        {
            var op = modbus.WriteCoil(address, values);
            return new Result { IsSuccess = op.IsSuccess, Msg = op.Message };
        }

        public override Result WriteCoil(string address, bool value)
        {
            var op = modbus.Write(address, value);
            return new Result { IsSuccess = op.IsSuccess, Msg = op.Message };
        }

        public override Result WriteOneRegister(string address, short value)
        {
            var op = modbus.WriteOneRegister(address, value);
            return new Result { IsSuccess = op.IsSuccess, Msg = op.Message };
        }
    }
}
