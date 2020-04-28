using System.Collections.Generic;

namespace KHBC.Core.FrameBase
{
    // <summary>
    /// 项目的启动类，分布式加载
    /// </summary>
    public interface IStartup : IDependency
    {
        int Order { get; }
        /// <summary>
        /// 1系统启动前（此时容器还未注册）
        /// </summary>
        void BeforeStartup();

        /// <summary>
        /// 2补充注册容器  （此时容器还在注册中）
        /// </summary>
        List<Registrations> Register();
        /// <summary>
        /// 3系统启动后加载资源（此时容器注册完毕）
        /// </summary>
        void AfterStartup();

    }
}
