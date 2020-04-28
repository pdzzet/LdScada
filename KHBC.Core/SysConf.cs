using KHBC.Core.Device;
using KHBC.Core.Extend;
using KHBC.Core.Log;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace KHBC.Core
{
    /// <summary>
    /// 数据库配置实体
    /// </summary>
    public class DbConfModel
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        public string ConnectionString = "";
        /// <summary>
        /// 执行sql超时时间
        /// </summary>
        public int CommandTimeOut = 30000;
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DbType = "MySQL";
        /// <summary>
        /// 是否记录日志
        /// </summary>
        public bool IsLog = false;
    }

    /// <summary>
    /// REDIS配置模型
    /// </summary>
    public class RedisConfModel
    {
        public string ConnectStrings { get; set; } = "127.0.0.1:6379,password=123456";
        public int Mode { get; set; } = 1;
        public string[] Sentinels { get; set; }
    }

    public class EntityModel
    {
        public string Id;
        public string Name;
    }
    /// <summary>
    /// 系统级配置模型
    /// </summary>
    public class SysConfModel
    {
        /// <summary>
        /// RunEnv
        /// EVOC:工控机，SERVER: 服务器
        /// </summary>
        public string RunEnv { get; set; } = "SERVER";

        /// <summary>
        /// 工厂
        /// </summary>
        public EntityModel Factory = new EntityModel { Id = "F01", Name = "绿的谐波" };
        /// <summary>
        /// 车间
        /// </summary>
        public EntityModel Plant = new EntityModel { Id = "P01", Name = "车间P01" };

        public EntityModel AssemblyLine = new EntityModel { Id = "A00", Name = "柔性加工线" };

        public DbConfModel DbMES = new DbConfModel
        {
            ConnectionString = "User ID = LDMES; Password=LDMES;Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = 192.168.3.18)(PORT = 1521)))(CONNECT_DATA = (SERVICE_NAME = orcl)))",
            DbType = "Oracle"
        };
        public DbConfModel DbBC = new DbConfModel
        {
            ConnectionString = "server=192.168.3.222;Database=ldbcsys;Uid=ldbcsys;Pwd=ldbcsys",
            DbType = "MySql"
        };
        public RedisConfModel RedisLocal = new RedisConfModel();
        public RedisConfModel RedisRemote = null;
    }

    /// <summary>
    /// 系统级配置
    /// </summary>
    public static class SysConf
    {
        public static SysConfModel Main;
        public static DeviceConfig Device = null;
        public static bool RunInEvoc = true;
        public static string KeyPlant = "";
        public static string KeyAssemblyLine = "";
        /// <summary>
        /// 初始化启动配置
        /// Main为参数域，Local为布尔域
        /// </summary>
        public static void Initialize()
        {
            string cmd = Environment.CommandLine;
            string cmdStr = cmd.ToUpper();
            if (!cmdStr.Contains("MAIN"))
                return;
            cmdStr = cmdStr.Substring(cmdStr.IndexOf("MAIN") + 4).Trim();
            var cmdStrArr = cmdStr.Split(' ');
            var dict = cmdStrArr.Select(item => item.Split('='))
                .Where(kp => kp.Length >= 2)
                .ToDictionary(kp => kp[0], kp => kp[1]);
            if (dict == null)
                return;

            #region Main

            if (dict.Keys.Contains("ENVIR") && Enum.GetNames(typeof(EnvironmentMode)).Contains(dict["ENVIR"]))
                Envir = (EnvironmentMode)Enum.Parse(typeof(EnvironmentMode), dict["ENVIR"]);

            if (dict.Keys.Contains("PREFIX"))
            {
                ServerPrefix = dict["PREFIX"];
            }

            #endregion

            #region LOCAL

            var localcmd = dict.FirstOrDefault(item => item.Key == "LOCAL").Value;
            if (localcmd == null)
            {
                return;
            }
            var local = localcmd.Split('|');
            VisualGateway = local.Contains("GATEWAY");
            SaveTemplate = local.Contains("TEMPLATE");
            IsSqlLog = local.Contains("ISSQLLOG");
            #endregion
        }

        /// <summary>
        /// 系统资源初始化
        /// </summary>
        public static void Startup(string processName, string moduleName = "")
        {
            ConfigPath = Path.GetFullPath($"{BasePath}../../Cfg");
            ProcessName = processName;
            ModuleName = moduleName;

            if (!Directory.Exists(ConfigPath))
            {
                Directory.CreateDirectory(ConfigPath);
            }

            MainConfigFile = Path.Combine(ConfigPath, $"Main.json");
            DeviceConfigFile = Path.Combine(ConfigPath, $"{ProcessName}.device.xml");

            Logger.Main.Info($"加载主配置文件{SysConf.MainConfigFile}");
            Main = JsonExtension.GetDefKey(SysConf.MainConfigFile, "MAIN", new SysConfModel());

            if (File.Exists(SysConf.DeviceConfigFile))
            {
                var dc = XmlExtension.LoadXMLFile<DeviceConfig>(SysConf.DeviceConfigFile);
                if (dc.IsSuccess)
                {
                    Logger.Main.Info($"加载设备配置文件: {SysConf.DeviceConfigFile} 成功");
                    Device = dc.Data;
                    Device.Recombine();
                    //foreach (var d in Device.Devices)
                    //{
                    //    foreach (var p in d.Value.Properties)
                    //    {
                    //        System.Console.WriteLine(p.Value.Id);
                    //    }
                    //}

                }
                else
                {
                    Logger.Main.Error($"加载设备配置文件: {SysConf.DeviceConfigFile} 失败");
                }
            }

            if (SysConf.Main.RunEnv != "EVOC")
            {
                RunInEvoc = false;
            }

            KeyPlant = $"LD:{Main.Factory.Id}:{Main.Plant.Id}";
            KeyAssemblyLine = $"{KeyPlant}:{Main.AssemblyLine.Id}";

            SysBootStrapper.Initialize("");
            ServiceManager.Start();
        }

        /// <summary>
        /// 虚拟网关
        /// 开启后将从本地数据模板取值
        /// </summary>
        public static bool VisualGateway { get; private set; } = false;

        /// <summary>
        /// 保存数据模板
        /// </summary>
        public static bool SaveTemplate { get; private set; } = false;

        /// <summary>
        /// 是否记录sql日志
        /// </summary>
        public static bool IsSqlLog { get; set; }
        /// <summary>
        /// 系统环境
        /// </summary>
        public static EnvironmentMode Envir { get; private set; } = EnvironmentMode.PUB;

        /// <summary>
        /// 程序基础目录
        /// </summary>
        public static string BasePath { get; } = AppDomain.CurrentDomain.BaseDirectory;

        public static string ProcessName { get; private set; }
        public static string ModuleName { get; private set; }

        public static string MainConfigFile { get; private set; }
        public static string DeviceConfigFile { get; private set; }
        public static string ConfigPath { get; private set; }

        /// <summary>
        /// 模板目录
        /// </summary>
        public static string TemplatePath { get; } = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TempData");

        /// <summary>
        /// 网关编码
        /// </summary>
        public static Encoding GatewayEncoding { get; private set; } = Encoding.UTF8;

        /// <summary>
        /// 网关超时（秒）
        /// </summary>
        public static int TimeOut { get; private set; } = 60;

        /// <summary>
        /// 网关前缀
        /// </summary>
        public static string ServerPrefix { get; private set; }
        /// <summary>
        /// 系统版本
        /// </summary>
        public static string Version { get; set; }
        /// <summary>
        /// 机器ID
        /// </summary>
        public static string MachineId { get; set; }

        /// <summary>
        /// 系统容器
        /// </summary>
        public static IocAutofacContainer SysContainer { get; internal set; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        public static string DBConnectionString { get; private set; }

        /// <summary>
        /// 系统时间
        /// </summary>
        public static DateTime NowTime
        {
            get { return DateTime.Now; }
            //  get { return DateTime.Now.Add(diffTime); }
        }

        /// <summary>
        /// 与服务器时间的差值
        /// </summary>
        public static TimeSpan diffTime = new TimeSpan(0, 0, 0);

    }

    /// <summary>
    /// 系统环境
    /// </summary>
    public enum EnvironmentMode
    {
        /// <summary>
        /// 测试
        /// </summary>
        TEST,
        /// <summary>
        /// 开发
        /// </summary>
        DEV,
        /// <summary>
        /// 预发布
        /// </summary>
        PRE,
        /// <summary>
        ///正式
        /// </summary>
        PUB
    }

}
