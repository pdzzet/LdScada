using KHBC.Core;
using KHBC.Core.BaseModels;
using System.Linq;

namespace KHBC.LD.OLCM
{
    /// <summary>
    /// 分布式启动
    /// </summary>
    public class Startup : BaseStartup
    {
        public override void AfterStartup()
        {
            
            if (SysConf.Device != null && SysConf.Device.Devices != null)
            {
                foreach (var val in SysConf.Device.Devices)
                {
                    var d = val.Value;
                    ServiceManager.Services.Add(new OlcmService(d));
                }

            }
        }
        public override void BeforeStartup()
        {
            // 初始化配置
            //OlcmConf.InitConf();
        }
    }
}
