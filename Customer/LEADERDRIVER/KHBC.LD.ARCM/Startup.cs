using KHBC.Core;
using KHBC.Core.BaseModels;

namespace KHBC.LD.ARCM
{
    /// <summary>
    /// 分布式启动
    /// </summary>
    public class Startup : BaseStartup
    {
        public override void AfterStartup()
        {
            //加入设备线程
            if (SysConf.Device != null && SysConf.Device.Devices != null)
            {
                foreach (var val in SysConf.Device.Devices)
                {
                    var d = val.Value;
                    ServiceManager.Services.Add(new ArcmService(d));
                }
            }
        }

        public override void BeforeStartup()
        {
        }
    }
}
