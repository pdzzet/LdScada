using CSRedis;
using KHBC.Core.Extend;
using KHBC.Core;
using KHBC.Core.Log;
using Nancy;
using Nancy.Extensions;
using System;

namespace KHBC.LD.BWSI.Controller
{
    class RedisController : IApiController
    {
        public string Name => "redis";
        private CSRedisClient RedisClientLocal;

        public Result DoHttpWork(Request req)
        {
            var key = req.Query["key"];
            if (key == null)
            {
                return Result.Fail("参数错误");
            }

            try
            {
                RedisClientLocal = new CSRedisClient(SysConf.Main.RedisLocal.ConnectStrings);
                Logger.Main.Info($"[{Program.ModuleName}]初始化LOCAL REDIS CLIENT成功: \"{SysConf.Main.RedisLocal.ConnectStrings}\"");
            }
            catch (Exception ex)
            {
                Logger.Main.Error($"[{Program.ModuleName}]初始化LOCAL REDIS CLIENT失败: \"{SysConf.Main.RedisLocal.ConnectStrings}\", {ex.Message}");
            }

            if (req.Method == "GET")
            {
                if (!RedisClientLocal.Exists(key))
                {
                    return Result.Fail($"Key: {key} 不存在");
                }

                return Result.Success(RedisClientLocal.Get(key));
            }
            else if (req.Method == "POST")
            {
                string content = req.Body.AsString();
                return Result.Success(RedisClientLocal.Set(key, content));
            }
            return Result.Fail($"不支持{req.Method}方法");
        }
    }
}
