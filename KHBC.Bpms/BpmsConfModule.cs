using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CSRedis;
using KHBC.BPMS.Log;

namespace KHBC.BPMS
{
    public class BpmsConf
    {
        internal static BpmsConfModule Conf;

        internal static CSRedisClient RedisClientLocal { get; private set; }
        internal static CSRedisClient RedisClientRemote { get; private set; }
        public static string BasePath { get; } = AppDomain.CurrentDomain.BaseDirectory;
        public static string MainConfigFile { get; private set; }
        public static string ConfigPath { get; private set; }
        public static string AppPath { get; private set; }
        public static bool RunInEvoc { get; private set; } = true;

        internal static void InitConf()
        {
            AppPath = Path.GetFullPath($"{BasePath}../../Bin");
            ConfigPath = Path.GetFullPath($"{BasePath}../../Cfg");
            MainConfigFile = System.IO.Path.Combine(ConfigPath, $"{Program.ServiceName}.main.json");
            if (!System.IO.Directory.Exists(ConfigPath))
            {
                System.IO.Directory.CreateDirectory(ConfigPath);
            }

            Logger.Main.Info($"加载配置文件{MainConfigFile}");
            Conf = JsonExtension.GetDefKey(MainConfigFile, "BPMS", new BpmsConfModule());

            if (Conf.RunEnv != "EVOC")
            {
                RunInEvoc = false;
            }

            RedisClientLocal = new CSRedisClient(Conf.RedisLocal);
            Logger.Main.Info($"初始化LOCAL  REDIS CLIENT: \"{Conf.RedisLocal}\"");
            RedisClientRemote = new CSRedisClient(Conf.RedisRemote);
            Logger.Main.Info($"初始化REMOTE REDIS CLIENT: \"{Conf.RedisRemote}\"");
        }
    }

    public class BpmsConfModule
    {
        public string RunEnv { get; set; } = "EVOC"; // EVOC:工控机，SERVER: 服务器

        public string ProductionLineId { get; set; } = "A01"; // 整厂为A00
        public string RedisLocal { get; set; } = "127.0.0.1:6379,password=123456";
        public string RedisRemote { get; set; } = "192.168.3.222:6379,password=123456";
        public string[] ProcessList { get; set; } =
        {
            "KHBC.LD.APCM",
            "KHBC.LD.ARCM",
            "KHBC.LD.BDCI",
            "KHBC.LD.BWSI",
            "KHBC.LD.CCSM",
            "KHBC.LD.CLSM",
            "KHBC.LD.CWSM",
            "KHBC.LD.FCCM",
            "KHBC.LD.MDCI",
            "KHBC.LD.OLCM",
            "KHBC.LD.OTCM",
            "KHBC.LD.PWSI"
        };
    }
}
