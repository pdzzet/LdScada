using KHBC.Core.Extend;
using KHBC.Core;
using Nancy;
using Nancy.Extensions;
using System;
using System.IO;
using System.Text;

namespace KHBC.LD.BWSI.Controller
{
    class ConfigController : IApiController
    {
        public string Name => "config";

        public Result DoHttpWork(Request req)
        {
            var file = req.Query["file"];
            string path = Path.Combine(SysConf.ConfigPath, file);

            if (req.Method == "GET")
            {
                if (!File.Exists(path))
                {
                    return Result.Fail("文件不存在");
                }
                string content = File.ReadAllText(path, Encoding.UTF8);
                return Result.Success(content);
            }
            else if (req.Method == "POST")
            {
                string content = req.Body.AsString();
                try
                {
                    File.WriteAllText(path, content);
                }
                catch (Exception ex)
                {
                    return Result.Fail($"写文件失败：{ex.Message}");
                }

                return Result.Success();
            }

            return Result.Fail($"不支持{req.Method}方法");

        }
    }
}
