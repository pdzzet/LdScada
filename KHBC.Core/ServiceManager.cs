using System.Collections.Generic;
using KHBC.Core.Interface;
using KHBC.Core.Log;

namespace KHBC.Core
{
    /// <summary>
    /// 服务代理者
    /// </summary>
    public class ServiceManager
    {
        /// <summary>
        /// 系统服务列表
        /// </summary>
        public static List<IService> Services { get; private set; } = new List<IService>();

        public static void Init(List<IService> list)
        {
            if (Services == null)
            {
                Services = list;
            }
            Logger.Main.Info($"服务初始化,共{Services.Count}个");
        }

        /// <summary>
        /// 查找服务
        /// </summary>
        /// <param name="name"></param>
        public static IService FindService(string name)
        {
            var s = Services.Find(x => x.ServiceName == name);
            return s;
        }

        /// <summary>
        /// 中止服务
        /// </summary>
        public static void Abort()
        {
            foreach (var item in Services)
            {
                item.Abort();
            }
            Logger.Main.Info($"服务全部已中止,共{Services.Count}个");
        }

        /// <summary>
        /// 恢复服务
        /// </summary>
        public static void Resume()
        {
            foreach (var item in Services)
            {
                item.Resume();
            }
            Logger.Main.Info($"服务全部已恢复,共{Services.Count}个");
        }

        /// <summary>
        /// 服务全部挂起
        /// </summary>
        public static void Suspend()
        {
            foreach (var item in Services)
            {
                item.Suspend();
            }
            Logger.Main.Info($"服务全部已挂起,共{Services.Count}个");
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public static void Start()
        {
            foreach (var item in Services)
            {
                item.Start();
            }
            Logger.Main.Info($"服务全部已启动,共{Services.Count}个");
        }
    }
}
