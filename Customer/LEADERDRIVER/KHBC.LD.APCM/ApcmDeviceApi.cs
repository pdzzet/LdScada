using HslCommunication;
using HslCommunication.ModBus;
using KHBC.Core.Device;
using KHBC.Core.Log;
using System;
using System.Text.RegularExpressions;

namespace KHBC.LD.APCM
{
    public class ApcmDeviceApi : IDevice
    {
        public bool ConnectStatus { get; set; } = false;
        public string Message { get; set; }
        public int ErrorCode { get; set; }

        private readonly ModbusTcpNet _deviceApi;
        private readonly string _serviceName;
        private readonly string _ipAddress;
        private readonly int _port;

        public ApcmDeviceApi(string serviceName, string ipAddress, int port)
        {
            _serviceName = serviceName;
            _ipAddress = ipAddress;
            _port = port;

            _deviceApi = new ModbusTcpNet(_ipAddress, _port);
            if ((_deviceApi != null)&& (_deviceApi.ConnectServer().IsSuccess))
            {

                Logger.Main.Info($"[{_serviceName}]初始化设备成功: {_ipAddress}:{_port}");
                ConnectStatus = true;
                
            }
            else
            {
                Logger.Main.Info($"[{_serviceName}]初始化设备失败: {_ipAddress}:{_port}");
            }
        }
        public bool Read<T>(string addr, string length, ref object data)
            {
            return true;
            }
        public bool Read<T>(string addr, int length, ref object data)
        {
            if (typeof(T) == typeof(byte[]))
            {
                var adr = Regex.Replace(addr, @"^[%a-zA-Z]+", string.Empty);
                OperateResult<byte[]> rs;
                rs = _deviceApi.Read(adr, (ushort)(length / 2 + length % 2));
                if (rs.IsSuccess)
                {
                    ErrorCode = 0;
                    Message = "Success";
                    data = rs.Content;
                    Logger.Main.Warn($"[{_serviceName}] Read(\"{addr}\", BOOL, {length})成功: {rs.ErrorCode}, {rs.Message}");
                    return true;

                }
                else
                {
                    Message = rs.Message;
                    Logger.Main.Warn($"[{_serviceName}] Read(\"{addr}\", BOOL, {length})失败: {rs.ErrorCode}, {rs.Message}");
                }
            }

            ConnectStatus = false;
            return false;
        }

        public bool Write<T>(string addr, object data)
        {
            if (typeof(T) == typeof(byte[]))
            {
                var adr = Regex.Replace(addr, @"^[%a-zA-Z]+", string.Empty);
                var byteData = (byte[])data;
                var len = byteData.Length;
                var hasTail = byteData.Length % 2;
                var rs = _deviceApi.Write(adr, byteData);

                if (rs.IsSuccess)
                {
                    if (hasTail > 0)
                    {
                        var padAddr = Convert.ToInt32(adr) + (len / 2);
                        var rx = _deviceApi.Read(padAddr.ToString(), 1);
                        if (rx.IsSuccess)
                        {
                            var tailData = rx.Content;
                            tailData[0] = byteData[len - 1];
                            rs = _deviceApi.Write(padAddr.ToString(), tailData);
                            if (rs.IsSuccess)
                            {
                                ErrorCode = 0;
                                Logger.Main.Warn($"[{_serviceName}] padAddr(\"{addr}\", BYTE, 1)成功: {rs.ErrorCode}, {rs.Message}");
                                return true;
                            }
                        }

                        Logger.Main.Warn($"[{_serviceName}] padAddr(\"{addr}\", BYTE, 1)失败: {rs.ErrorCode}, {rs.Message}");
                    }
                    else
                    {
                        ErrorCode = 0;
                        Logger.Main.Warn($"[{_serviceName}] padAddr(\"{addr}\", BYTE, 1)成功: {rs.ErrorCode}, {rs.Message}");
                        return true;
                    }
                }

                ErrorCode = rs.ErrorCode;
                Message = rs.Message;
                Logger.Main.Warn($"[{_serviceName}] Write(\"{addr}\", BYTE, {byteData.Length})失败: {rs.ErrorCode}, {rs.Message}");

                ConnectStatus = false;
            }

            return false;
        }

        public bool ExeCmd(string cmd)
        {
            return true;
        }

        public void Connect()
        {
            if (ConnectStatus)
            {
                return;
            }

            var rs = _deviceApi.ConnectServer();
            if (rs.IsSuccess)
            {
                Logger.Main.Info($"[{_serviceName}]重新连接设备成功: {_ipAddress}:{_port}");
                ConnectStatus = true;
            }
            else
            {
                Logger.Main.Info($"[{_serviceName}]重新连接设备失败: {_ipAddress}:{_port}");
            }
        }

        public void Disconnect()
        {
        }
    }
}
