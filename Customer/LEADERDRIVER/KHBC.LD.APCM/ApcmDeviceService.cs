using KHBC.Core.BaseModels;
using KHBC.Core;
using KHBC.Core.Device;

namespace KHBC.LD.APCM
{
    /// <summary>
    /// 线程服务
    /// </summary>
    public class ApcmDeviceService : BaseService
    {
        public override string ServiceName { get; set; }

        private readonly DeviceHandler _deviceHandler;

        public ApcmDeviceService(DeviceInfo devInfo)
        {
            ServiceName = $"{SysConf.ModuleName}:EQ:{devInfo.Id}";
            _deviceHandler = new DeviceHandler(this, devInfo, new ApcmDeviceApi(ServiceName, devInfo.IpAddress, devInfo.Port));
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
