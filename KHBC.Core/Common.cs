using System.Collections.Generic;
using Autofac;
using KHBC.Core.FrameBase;

namespace KHBC.Core
{
    /// <summary>
    /// 系统公用
    /// </summary>
    public class Common
    {
        /// <summary>
        /// 公用取实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetInstance<T>() where T : IDependency
        {
            return SysConf.SysContainer.Container.Resolve<T>();
        }
        /// <summary>
        /// 公用按名字取实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetInstance<T>(string name) where T : IDependency
        {
            return SysConf.SysContainer.Container.ResolveNamed<T>(name);
        }
        /// <summary>
        /// 公用获取所有实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetAllInstance<T>() where T : IDependency
        {
            return SysConf.SysContainer.ResolveAll<T>();
        }

    }
}
