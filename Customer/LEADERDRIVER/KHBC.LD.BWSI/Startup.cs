using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KHBC.Core;
using KHBC.Core.BaseModels;
using KHBC.Core.FrameBase;
using KHBC.Core.Log;

namespace KHBC.LD.BWSI
{
    public class Startup : BaseStartup
    {
        public override void AfterStartup()
        {
            //获取并缓存所有方法对象和控制器对象
            //var controllers = Common.GetAllInstance<IApiController>();
            //foreach (var ctl in controllers)
            //{
            //    ServiceHost.ApiDict[ctl.Name.ToUpper()] = ctl;
            //    var t = ctl.GetType();
            //    var methods = t.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(x => !x.IsSpecialName);
            //    var dict = new Dictionary<string, MethodInfo>();
            //    foreach (var m in methods)
            //    {
            //        dict[m.Name.ToUpper()] = m;
            //    }
            //    ServiceHost.ActionDict[ctl.Name.ToUpper()] = dict;
            //}

            var controllers = SysConf.SysContainer.Assemblies.SelectMany(x => x.Types.Where(p => typeof(IApiController).IsAssignableFrom(p) && !p.IsAbstract)).ToArray();
            if (controllers.Any())
            {
                foreach (var item in controllers)
                {
                    var api = (IApiController)System.Activator.CreateInstance(item);
                    ServiceHost.ApiDict[api.Name] = api;
                }
            }
            ServiceHost.Start();
            Logger.Main.Info($"[HttpHost] startup [port={ServiceHost.ConfModel.Port}]");
        }

        public override List<Registrations> Register()
        {
            return new List<Registrations>
            {
                new Registrations(null,true,typeof(IApiController))
            };
        }

        public override void BeforeStartup()
        {
            //加载本地配置
            ServiceHost.ConfModel = JsonExtension.GetDefKey(SysConf.MainConfigFile, "BWSI", new HttpConfModel());
            if (ServiceHost.ConfModel.IsLog)
            {
                ServiceHost.Loged += Logger.Net.Info;
                ServiceHost.ErrorLoged += Logger.Net.Error;
            }
        }
    }
}
