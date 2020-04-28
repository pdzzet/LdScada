using KHBC.Core.BaseModels;
using KHBC.Core;
using KHBC.Core.Device;
using KHBC.Core.Log;

namespace KHBC.LD.CCSM
{
    /// <summary>
    /// 线程服务
    /// </summary>
    public class CcsmService : BaseService
    {
        public override string ServiceName { get; set; }

        private readonly DeviceHandler _deviceHandler;

        public CcsmService(DeviceInfo devInfo)
        {
            ServiceName = $"{SysConf.ModuleName}:{devInfo.Id}";
            _deviceHandler = new DeviceHandler(this, devInfo, new CcsmDeviceApi(ServiceName, devInfo.IpAddress, devInfo.Port));
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
