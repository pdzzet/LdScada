using System;
using System.Collections.Generic;
using System.Reflection;
using Nancy.Hosting.Self;

namespace KHBC.LD.BWSI
{
    /// <summary>
    /// http宿主
    /// </summary>
    public class ServiceHost
    {
        internal static HttpConfModel ConfModel;
        private static NancyHost _host;
        internal static event Action<string> Loged;
        internal static event Action<Exception> ErrorLoged;

        /// <summary>
        /// 路由指定实例
        /// </summary>
        public static Dictionary<string, IApiController> ApiDict = new Dictionary<string, IApiController>();
        /// <summary>
        /// 
        /// </summary>
        //public static Dictionary<string, Dictionary<string, MethodInfo>> ActionDict = new Dictionary<string, Dictionary<string, MethodInfo>>();


        public static void Start()
        {
            Loged?.Invoke("Start....");
            string hostUri = $"http://localhost:{ConfModel.Port}";
            _host = new NancyHost(new Uri(hostUri));
            _host.Start();

        }
        public static void Stop()
        {
            Loged?.Invoke("Stop....");
            if (_host != null)
            {
                _host.Stop();
            }
        }

        public static void ReStart()
        {
            Loged?.Invoke("ReStart....");
            try
            {
                Stop();
            }
            catch (Exception)
            {
                // ignored
            }
            System.Threading.Thread.Sleep(3000);
            Start();
        }

        public static void OnLoged(string obj)
        {
            Loged?.Invoke(obj);
        }

        internal static void OnErrorLoged(Exception obj)
        {
            ErrorLoged?.Invoke(obj);
        }
    }

    public class HttpConfModel
    {
        public int Port { get; set; } = 8888;
        public bool IsLog { get; set; } = false;
    }
}
