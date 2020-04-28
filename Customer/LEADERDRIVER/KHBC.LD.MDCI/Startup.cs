using KHBC.Core;
using KHBC.Core.BaseModels;

namespace KHBC.LD.MDCI
{
    /// <summary>
    /// 分布式启动
    /// </summary>
    public class Startup : BaseStartup
    {
        public override void AfterStartup()
        {
            // 数据库查询线程
            ServiceManager.Services.Add(new MdciService());
        }

        public override void BeforeStartup()
        {
            MdciKeyConf.Init();
        }

    }
}
