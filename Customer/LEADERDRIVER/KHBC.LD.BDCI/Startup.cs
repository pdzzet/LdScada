using KHBC.Core;
using KHBC.Core.BaseModels;
using KHBC.Core.FrameBase;
using System.Collections.Generic;

namespace KHBC.LD.BDCI
{
    /// <summary>
    /// 分布式启动
    /// </summary>
    public class Startup : BaseStartup
    {
        public override List<Registrations> Register()
        {
            return new List<Registrations>
            {
                new Registrations(null, true, typeof(IActionHandle))
            };
        }

        //private AuboHandler[] _arr;
        public override void AfterStartup()
        {
            ServiceManager.Services.Add(new BdciService());
        }

        public override void BeforeStartup()
        {
        }

    }
}
