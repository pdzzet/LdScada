using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.ModBus;
using KHBC.Core.Extend;

namespace KHBC.Modbus
{
    public class ModbusRtuClient : ModBusBase
    {
        private ModbusRtu modbus;

        public override bool IsConnected => modbus.IsOpen();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="portName">串口号</param>
        /// <param name="baudRate">波特率</param>
        /// <param name="station">站点号</param>
        /// <param name="isAddressStartWithZero"></param>
        /// <param name="dataBits">数据位</param>
        /// <param name="stopBits">停止位</param>
        /// <param name="parity">奇偶校验</param>
        /// <returns></returns>
        public ModbusRtuClient(string portName, int baudRate, byte station, bool isAddressStartWithZero = true, int dataBits = 8, int stopBits = 0, int parity = 0)
        {
            modbus = new ModbusRtu();
            modbus.SerialPortInni(sp =>
            {
                sp.PortName = portName;
                sp.BaudRate = baudRate;
                sp.DataBits = dataBits;
                sp.StopBits = stopBits == 0 ? System.IO.Ports.StopBits.None : (stopBits == 1 ? System.IO.Ports.StopBits.One : System.IO.Ports.StopBits.Two);
                sp.Parity = parity == 0 ? System.IO.Ports.Parity.None : (parity == 1 ? System.IO.Ports.Parity.Odd : System.IO.Ports.Parity.Even);
            });
            modbus.Station = station;
            modbus.AddressStartWithZero = isAddressStartWithZero;
            modbus.IsStringReverse = false;
        }

        ~ModbusRtuClient()
        {
            modbus.Close();
            modbus.Dispose();
        }

      
        public override Result Open()
        {
            modbus.Open();
            return new Result {IsSuccess =modbus.IsOpen()};
        }

        public override void Close()
        {
            modbus.Close();
        }

    
        public override Result<byte[]> Read(string address, ushort length=1)
        {
            var op = modbus.Read(address, length);
            return new Result<byte[]> {IsSuccess = op.IsSuccess, Data = op.Content, Msg = op.Message};
        }

        public override Result<short> ReadInt16(string address)
        {
            var op = modbus.ReadInt16(address);
            return new Result<short> {IsSuccess = op.IsSuccess, Data = op.Content, Msg = op.Message};
        }

        public override Result<short[]> ReadInt16(string address, ushort length)
        {
            var op = modbus.ReadInt16(address, length);
            return new Result<short[]> {IsSuccess = op.IsSuccess, Data = op.Content, Msg = op.Message};
        }

        public override Result<bool> ReadBool(string address)
        {
            var op = modbus.ReadBool(address);
            return new Result<bool> {IsSuccess = op.IsSuccess, Data = op.Content, Msg = op.Message};
        }

        public override Result<bool[]> ReadBool(string address, ushort length)
        {
            var op = modbus.ReadBool(address, length);
            return new Result<bool[]> {IsSuccess = op.IsSuccess, Data = op.Content, Msg = op.Message};
        }

        public override Result<float> ReadFloat(string address)
        {
            var op = modbus.ReadFloat(address);
            return new Result<float> {IsSuccess = op.IsSuccess, Data = op.Content, Msg = op.Message};
        }

        public override Result<float[]> ReadFloat(string address, ushort length)
        {
            var op = modbus.ReadFloat(address, length);
            return new Result<float[]> {IsSuccess = op.IsSuccess, Data = op.Content, Msg = op.Message};
        }

        public override Result<bool> ReadCoil(string address)
        {
            var op = modbus.ReadCoil(address);
            return new Result<bool> {IsSuccess = op.IsSuccess, Data = op.Content, Msg = op.Message};
        }

        public override Result<bool[]> ReadCoil(string address, ushort length)
        {
            var op = modbus.ReadCoil(address, length);
            return new Result<bool[]> {IsSuccess = op.IsSuccess, Data = op.Content, Msg = op.Message};
        }

        public override Result Write(string address, byte[] value)
        {
            var op = modbus.Write(address, value);
            return new Result {IsSuccess = op.IsSuccess, Msg = op.Message};
        }

        public override Result WriteInt16(string address, short value)
        {
            var op = modbus.Write(address, value);
            return new Result {IsSuccess = op.IsSuccess, Msg = op.Message};
        }

        public override Result WriteInt16(string address, short[] values)
        {
            var op = modbus.Write(address, values);
            return new Result {IsSuccess = op.IsSuccess, Msg = op.Message};
        }

        public override Result WriteFloat(string address, float value)
        {
            var op = modbus.Write(address, value);
            return new Result {IsSuccess = op.IsSuccess, Msg = op.Message};
        }

        public override Result WriteFloat(string address, float[] values)
        {
            var op = modbus.Write(address, values);
            return new Result {IsSuccess = op.IsSuccess, Msg = op.Message};
        }

        public override Result WriteBool(string address, bool[] values)
        {
            var op = modbus.Write(address, values);
            return new Result {IsSuccess = op.IsSuccess, Msg = op.Message};
        }

        public override Result WriteCoil(string address, bool[] values)
        {
            var op = modbus.WriteCoil(address, values);
            return new Result {IsSuccess = op.IsSuccess, Msg = op.Message};
        }

        public override Result WriteCoil(string address, bool value)
        {
            var op = modbus.Write(address, value);
            return new Result {IsSuccess = op.IsSuccess, Msg = op.Message};
        }

        public override Result WriteOneRegister(string address, short value)
        {
            var op = modbus.WriteOneRegister(address, value);
            return new Result {IsSuccess = op.IsSuccess, Msg = op.Message};
        }
    }
}