using CSRedis;
using KHBC.Core.Log;
using System;

namespace KHBC.Core
{
    public class KhQueueClient
    {
        private readonly CSRedisClient _redisClient;
        private readonly string _serviceName;
        public KhQueueClient(string serviceName)
        {
            _serviceName = serviceName;
            try
            {
                _redisClient = new CSRedisClient(SysConf.Main.RedisLocal.ConnectStrings);
                Logger.Main.Info($"[{_serviceName}]初始化 Redis Client 成功: \"{SysConf.Main.RedisLocal.ConnectStrings}\"");
            }
            catch (Exception ex)
            {
                Logger.Main.Error($"[{_serviceName}]初始化 Redis Client 失败: \"{SysConf.Main.RedisLocal.ConnectStrings}\", {ex.Message}");
                throw (ex);
            }
        }

        public T Get<T>(string key)
        {
            return _redisClient.Get<T>(key);
        }

        public bool Set(string key, object value)
        {
            return _redisClient.Set(key, value);
        }

        public void SetAsync(string key, object value)
        {
            _redisClient.SetAsync(key, value);
        }

        public long Push<T>(string key, params T[] value)
        {

            return _redisClient.RPush(key, value);
        }

        public T Pop<T>(string key)
        {
            return _redisClient.LPop<T>(key);
        }

        public T BPop<T>(int timeout, params string[] keys)
        {
            return _redisClient.BLPop<T>(timeout, keys);
        }
    }
}
