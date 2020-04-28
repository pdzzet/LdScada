using KHBC.Core.BaseModels;
using KHBC.Core;
using KHBC.Core.Device;

namespace KHBC.LD.OMCM
{
    /// <summary>
    /// 线程服务
    /// </summary>
    public class OmcmService : BaseService
    {
        public override string ServiceName { get; set; } = "OMCM";

        private readonly DeviceHandler _deviceHandler;

        public OmcmService(DeviceInfo devInfo)
        {
            ServiceName = $"{SysConf.ModuleName}:{devInfo.Id}";
            _deviceHandler = new DeviceHandler(this, devInfo, new OmcmDeviceApi(ServiceName, devInfo.IpAddress, devInfo.Port));
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
