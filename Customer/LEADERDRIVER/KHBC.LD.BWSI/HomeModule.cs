using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Nancy;
using KHBC.Core.Extend;
using KHBC.Core;

namespace KHBC.LD.BWSI
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            //Get["/"] = parameters => Response.AsFile("wwwroot/index.html", "text/html");

            Get["/api"] = parameters => "api";

            Get["/api/{name}", true] = async (_, token) => await ResponseHandleAsync(_.name);
            Post["/api/{name}", true] = async (_, token) => await ResponseHandleAsync(_.name);

            //Get["/api/{controller}/{action}", true] = async (_, token) => await ResponseHandleAsync(_.controller, _.action);
            //Post["/api/{controller}/{action}", true] = async (_, token) => await ResponseHandleAsync(_.controller, _.action);

            Get["/api/download/{name}"] = _ => Download(_.name);
            Post["/api/download/{name}"] = _ => Download(_.name);
        }


        private dynamic Download(string name)
        {
            string path = Path.Combine(SysConf.BasePath, "OutputFiles", name);
            if (!File.Exists(path))
            {
                return Response.AsText("不存在文件", "text/html;charset=UTF-8");
            }
            return Response.AsFile(path);
        }

        public async Task<dynamic> ResponseHandleAsync(string str)
        {
            var timer = new Stopwatch();
            timer.Start();
            var res = await Task.Factory.StartNew(() =>
            {
                if (ServiceHost.ApiDict.ContainsKey(str))
                {
                    return ServiceHost.ApiDict[str].DoHttpWork(Request).ToJsonStr();
                }
                var def = Result.Fail("请求接口不存在");
                return def.ToJsonStr();
            });
            timer.Stop();
            ServiceHost.OnLoged($"请求名：【{str}】");
            ServiceHost.OnLoged($"返回[耗时{timer.ElapsedMilliseconds}ms]：{res}");
            return Response.AsText(res, "text/html;charset=UTF-8");
        }


        //public async Task<dynamic> ResponseHandleAsync(string ctl, string action)
        //{
        //    var timer = new Stopwatch();
        //    timer.Start();
        //    var res = await Task.Factory.StartNew(() =>
        //    {
        //        var cn = ctl.ToUpper();
        //        var ac = action.ToUpper();

        //        if (ServiceHost.ApiDict.ContainsKey(cn))
        //        {
        //            if (ServiceHost.ActionDict[cn].ContainsKey(ac))
        //            {
        //                var obj = ServiceHost.ApiDict[cn];
        //                var method = ServiceHost.ActionDict[cn][ac];
        //                return method.Invoke(obj, new object[] { Request }).ToJsonStr();
        //            }
        //            return Result.Fail("请求接口方法不存在").ToJsonStr();
        //        }
        //        return Result.Fail("请求接口不存在").ToJsonStr();
        //    });
        //    timer.Stop();
        //    ServiceHost.OnLoged($"请求名：【{ctl}/{action}】");
        //    ServiceHost.OnLoged($"返回[耗时{timer.ElapsedMilliseconds}ms]：{res}");
        //    return Response.AsText(res.ToString(), "text/html;charset=UTF-8");
        //}
    }
}