using KHBC.Core.BaseModels;
using KHBC.Core;
using KHBC.Core.Device;

namespace KHBC.LD.ARCM
{
    /// <summary>
    /// 线程服务
    /// </summary>
    public class ArcmService : BaseService
    {
        public override string ServiceName { get; set; } = "ARCM";

        private readonly DeviceHandler _deviceHandler;

        public ArcmService(DeviceInfo devInfo)
        {
            ServiceName = $"{SysConf.ModuleName}:{devInfo.Id}";
            _deviceHandler = new DeviceHandler(this, devInfo, new ArcmDeviceApi(ServiceName, devInfo.IpAddress, devInfo.Port));
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
