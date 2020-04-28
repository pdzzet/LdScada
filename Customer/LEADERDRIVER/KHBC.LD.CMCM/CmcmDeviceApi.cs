using HslCommunication;
using HslCommunication.Profinet.Melsec;
using KHBC.Core.Device;
using KHBC.Core.Log;
using System;
using System.Collections.Generic;

namespace KHBC.LD.CMCM
{
    public class CmcmDeviceApi : IDevice
    {
        public bool ConnectStatus { get; set; } = false;
        public string Message { get; set; }
        public int ErrorCode { get; set; }

        private readonly MelsecMcNet _deviceApi;
        private readonly string _serviceName;
        private readonly string _ipAddress;
        private readonly int _port;

        public CmcmDeviceApi(string serviceName, string ipAddress, int port)
        {
            _serviceName = serviceName;
            _ipAddress = ipAddress;
            _port = port;

            _deviceApi = new MelsecMcNet(_ipAddress, _port);
            if (_deviceApi != null)
            {
                Logger.Main.Info($"[{_serviceName}]初始化设备成功: {_ipAddress}:{_port}");
                //ConnectStatus = true;
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
            if (typeof(T) == typeof(bool[]))
            {
                OperateResult<bool[]> rs;
                rs = _deviceApi.ReadBool(addr, (ushort)length);
                if (rs.IsSuccess)
                {
                    ErrorCode = 0;
                    Message = "Success";
                    data = rs.Content;
                    return true;
                }
                else
                {
                    Message = rs.Message;
                    Logger.Main.Warn($"[{_serviceName}] Read(\"{addr}\", BOOL, {length})失败: {rs.ErrorCode}, {rs.Message}");
                }
            }
            else if (typeof(T) == typeof(float[]))
            {
                OperateResult<float[]> rs;
                rs = _deviceApi.ReadFloat(addr, (ushort)(length * 2));
                if (rs.IsSuccess)
                {
                    ErrorCode = 0;
                    Message = "Success";
                    var arr = new float[length];
                    for (var i = 0; i < length * 2; i += 2)
                    {
                        arr[i / 2] = rs.Content[i];
                    }
                    data = arr;
                    ErrorCode = rs.ErrorCode;
                    return true;
                }
                else
                {
                    Message = rs.Message;
                    Logger.Main.Warn($"[{_serviceName}] Read(\"{addr}\", FLOAT, {length})失败: {rs.ErrorCode}, {rs.Message}");
                }
            }
            else if (typeof(T) == typeof(UInt16))
            {
                OperateResult<UInt16[]> rs;
                rs = _deviceApi.ReadUInt16(addr, (ushort)(length));
                if (rs.IsSuccess)
                {
                    ErrorCode = 0;
                    Message = "Success";
                    var arr = new UInt16[length];
                    for (var i = 0; i < length; i++)
                    {
                        arr[i] = rs.Content[i];
                    }
                    data = arr;
                    ErrorCode = rs.ErrorCode;
                    return true;
                }
                else
                {
                    Message = rs.Message;
                    Logger.Main.Warn($"[{_serviceName}] Read(\"{addr}\", FLOAT, {length})失败: {rs.ErrorCode}, {rs.Message}");
                }
            }
            ConnectStatus = false;
            return false;
        }

        public bool ExeCmd(string cmd)
        {
            return true;
        }

        public bool Read(ref Dictionary<string, object> dataInfo)
        {
            throw new System.NotImplementedException();
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

        public bool Write<T>(string addr, object data)
        {
            throw new NotImplementedException();
        }
    }
}
