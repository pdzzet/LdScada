using KHBC.Core.BaseModels;
using KHBC.Core;
using KHBC.Core.Device;
using System;

namespace KHBC.LD.FCCM
{
    /// <summary>
    /// 线程服务
    /// </summary>
    public class FccmService : BaseService
    {
        public override string ServiceName { get; set; }

        private readonly DeviceHandler _deviceHandler;

        public FccmService(DeviceInfo devInfo)
        {
            ServiceName = $"{SysConf.ModuleName}:{devInfo.Id}";
            _deviceHandler = new DeviceHandler(this, devInfo, new FccmDeviceApi(ServiceName, devInfo.IpAddress, devInfo.Port));
            TimeSpan = devInfo.PollingTime;
        }

        /// <summary>
        /// 线程运行
        /// </summary>
        protected override void DoWork()
        {
            _deviceHandler.Process();
        }
    }
}
