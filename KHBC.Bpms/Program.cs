using KHBC.BPMS.Log;
using System;
using System.Configuration.Install;
using System.Diagnostics;
using System.ServiceProcess;

namespace KHBC.BPMS
{
    static class Program
    {
        public const string ServiceName = "KHBC.BPMS";
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main(string[] args)
        {
            BpmsConf.InitConf();

            //获取程序配置信息
            string path = Process.GetCurrentProcess().MainModule?.FileName;

            //判断服务是否在运行
            if (!Environment.UserInteractive)
            {
                //	运行服务
                ServiceBase.Run(new BpmsService());
            }
            else
            {
                var logstr = "";

                string arg = string.Concat(args);
                if (!string.IsNullOrWhiteSpace(arg))
                {
                    //	如果为卸载命令，则执行服务卸载
                    if (string.Equals(arg, "/u", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (ExistsService(ServiceName))
                        {
                            Logger.Main.Info($"卸载服务: {path}");
                            ManagedInstallerClass.InstallHelper(new string[] { "/u", path });
                        }
                        else
                        {
                            logstr = @"服务未安装，无需卸载...";
                            Logger.Main.Info(logstr);
                            Console.WriteLine(logstr);
                        }
                        Console.ReadKey();
                    }
                    else if (string.Equals(arg, "/debug", StringComparison.CurrentCultureIgnoreCase))
                    {
                        logstr = @"服务调试中";
                        Logger.Main.Info(logstr);
                        Console.WriteLine(logstr);

                        new BpmsService().Run();
                        Console.ReadLine();
                    }
                    return;
                }
                //  如果服务不存在，则安装
                if (!ExistsService(ServiceName))
                {
                    Logger.Main.Info($"安装服务: ${path}");
                    ManagedInstallerClass.InstallHelper(new string[] { path });
                }

                var status = GetServiceStatus();
                //  如果服务已停止，则启动
                if (status == ServiceControllerStatus.Stopped)
                {
                    ServiceStart();
                    status = GetServiceStatus();
                    if (status != ServiceControllerStatus.Running)
                    {
                        logstr = @"服务启动失败";
                        Logger.Main.Info(logstr);
                        Console.WriteLine(logstr);
                    }
                }
                //	获取服务状态
                logstr = $"当前服务状态:{status}";
                Logger.Main.Info(logstr);
                Console.WriteLine(logstr);
                Console.ReadKey();
            }
        }

        /// <summary>
        /// 判断服务是否存在
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <returns></returns>
        public static bool ExistsService(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            if (services.Length > 0)
            {
                for (int i = 0; i < services.Length; i++)
                {
                    var service = services[i];
                    if (service.ServiceName.Equals(serviceName))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取服务状态
        /// </summary>
        /// <returns></returns>
        public static ServiceControllerStatus? GetServiceStatus()
        {
            using (var controller = new ServiceController(ServiceName))
            {
                try
                {
                    return controller.Status;
                }
                catch
                {
                    return null;
                }

            }
        }

        public static void ServiceStart()
        {
            using (var controller = new ServiceController(ServiceName))
            {
                try
                {
                    if (controller.Status == ServiceControllerStatus.Stopped)
                    {
                        controller.Start();
                        controller.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 10));
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceWarning(ex.Message);
                }
            }

        }


    }
}