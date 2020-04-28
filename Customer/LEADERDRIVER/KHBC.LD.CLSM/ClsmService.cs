using KHBC.Core.BaseModels;
using KHBC.Core;
using KHBC.Core.Device;

namespace KHBC.LD.CLSM
{
    /// <summary>
    /// 线程服务
    /// </summary>
    public class ClsmService : BaseService
    {
        public override string ServiceName { get; set; } = "CLSM";

        private readonly DeviceHandler _deviceHandler;

        public ClsmService(DeviceInfo devInfo)
        {
            ServiceName = $"{SysConf.ModuleName}:{devInfo.Id}";
            _deviceHandler = new DeviceHandler(this, devInfo, new ClsmDeviceApi(ServiceName, devInfo.IpAddress, devInfo.Port));
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
