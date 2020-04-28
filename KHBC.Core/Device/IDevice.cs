using KHBC.Core.FrameBase;
using System.Collections.Generic;

namespace KHBC.Core.Device
{
    /// <summary>
    /// 设备接口
    /// </summary>
    public interface IDevice : IDependency
    {
        bool ConnectStatus { get; set;}
        string Message { get; set;}
        int ErrorCode { get; set;}
        void Connect();
        void Disconnect();
        bool Read<T>(string addr, int length, ref object data);
        bool Read<T>(string addr, string proId, ref object data);
        bool Write<T>(string addr, object data);
        //bool Read(string addr, int length, ref bool[] data);
        //bool Read(ref Dictionary<string, object> dataInfo);
        bool ExeCmd(string cmd);
    }
}
