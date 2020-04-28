using KHBC.Core.BaseModels;
using KHBC.Core;
using KHBC.Core.Device;

namespace KHBC.LD.CMCM
{
    /// <summary>
    /// 线程服务
    /// </summary>
    public class CmcmService : BaseService
    {
        public override string ServiceName { get; set; } = "CMCM";

        private readonly DeviceHandler _deviceHandler;

        public CmcmService(DeviceInfo devInfo)
        {
            ServiceName = $"{SysConf.ModuleName}:{devInfo.Id}";
            _deviceHandler = new DeviceHandler(this, devInfo, new CmcmDeviceApi(ServiceName, devInfo.IpAddress, devInfo.Port));
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
