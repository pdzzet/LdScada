using KHBC.Core.Device;
using System;
using HslCommunication.Profinet.Siemens;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KHBC.Core.Log;

namespace KHBC.LD.CWSM
{
    public class CwsmDeviceApi : IDevice
    {
        public bool ConnectStatus { get; set; } = false;
        public string Message { get; set; }
        public int ErrorCode { get; set; }

        private readonly SiemensS7Net _deviceApi;
        private readonly string _serviceName;
        private readonly string _ipAddress;
        private readonly int _port;

        public CwsmDeviceApi(string serviceName, string ipAddress, int port)
        {
            _serviceName = serviceName;
            _ipAddress = ipAddress;
            _port = port;

            _deviceApi = new SiemensS7Net(SiemensPLCS.S200Smart, ipAddress);
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
            var rs = _deviceApi.Read(addr, (ushort)length);
            if (rs.IsSuccess)
            {
                ErrorCode = 0;
                Message = "Success";
                data = rs.Content;
                return true;
            }

            ErrorCode = rs.ErrorCode;
            Message = rs.Message;
            Logger.Main.Warn($"[{_serviceName}] Read(\"{addr}\", {length})失败: {rs.ErrorCode}, {rs.Message}");
            ConnectStatus = false;
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
                //ConnectStatus = true;
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
