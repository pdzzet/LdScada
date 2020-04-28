using System;
using System.Threading;
using KHBC.Core.Enums;
using KHBC.Core.Interface;
using KHBC.Core;
using KHBC.Core.Log;

namespace KHBC.Core.BaseModels
{
    /// <summary>
    /// 基础服务
    /// </summary>
    public abstract class BaseService : IService
    {
        public abstract string ServiceName { get; set; }

        public long TimeSpan { get; set; } = 0;
        public ServiceState State { get; set; } = ServiceState.Normal;

        private KhQueueClient _queueClient { get; set; } = null;
        private string KeyHeartbeat;
        private string KeyService;

        //public event Action<object> PushEvent;
        /// <summary>
        /// 线程实例
        /// </summary>
        protected Thread Thr;
        /// <summary>
        /// 线程信号实例
        /// </summary>
        protected AutoResetEvent autoResetEvent = new AutoResetEvent(false);

        public BaseService()
        {
        }

        public void ConnectQueue()
        {
            if (_queueClient == null)
            {
                _queueClient = new KhQueueClient(ServiceName);
            }
        }

        public T Get<T>(string key)
        {
            ConnectQueue();
            if (_queueClient != null)
            {
                return _queueClient.Get<T>(key);
            }

            return default;
        }

        public bool Set(string key, object value)
        {
            ConnectQueue();
            if (_queueClient != null)
            {
                return _queueClient.Set(key, value);
            }

            return false;
        }

        public void SetAsync(string key, object value)
        {
            ConnectQueue();
            if (_queueClient != null)
            {
                _queueClient.SetAsync(key, value);
            }
        }

        public long Push<T>(string key, params T[] value)
        {
            ConnectQueue();
            if (_queueClient != null)
            {
                return _queueClient.Push<T>(key, value);
                
            }
            return -1;
        }

        public T Pop<T>(string key)
        {
            ConnectQueue();
            if (_queueClient != null)
            {
                return _queueClient.Pop<T>(key);
            }
            return default;
        }

        public T BPop<T>(int timeout, params string[] keys)
        {
            ConnectQueue();
            if (_queueClient != null)
            {
                return _queueClient.BPop<T>(timeout, keys);
            }
            return default;
        }

        public virtual void Start()
        {
            State = ServiceState.Start;
            if (Thr == null || State == ServiceState.Abort)
            {
                Thr = new Thread(ThreadWork);
                Thr.IsBackground = true;
                Thr.Start();
            }
            Logger.Main.Info($"服务：{ServiceName} 线程ID:{Thr.ManagedThreadId} Start");
        }

        public void Suspend()
        {
            State = ServiceState.Suspend;
            Logger.Main.Info($"服务：{ServiceName} 线程ID:{Thr.ManagedThreadId} Suspend");
        }

        public void Subscribe(Action<object> action)
        {
            //PushEvent += action;
        }

        public void UnSubscribe(Action<object> action)
        {
            //PushEvent -= action;
        }

        public void Resume()
        {
            State = ServiceState.Resume;
            autoResetEvent.Set();
            Logger.Main.Info($"服务：{ServiceName} 线程ID:{Thr.ManagedThreadId} Resume");
        }

        public void Abort()
        {
            State = ServiceState.Abort;
            Logger.Main.Info($"{ServiceName} Abort");
        }

        private void Heartbeat()
        {
            SetAsync(KeyHeartbeat, DateTime.Now);
        }

        /// <summary>
        /// 后台任务
        /// </summary>
        public virtual void ThreadWork()
        {
            KeyService = SysConf.RunInEvoc ? $"{SysConf.KeyAssemblyLine}:BPMS:{ServiceName}" : $"{SysConf.KeyPlant}:BPMS:{ServiceName}";
            KeyHeartbeat = $"{KeyService}:UpdateTime";
            SetAsync($"{KeyService}:StartupTime", DateTime.Now);
            SetAsync($"{KeyService}:Status", "Running");

            while (true)
            {
                if (State == ServiceState.Suspend)
                {
                    autoResetEvent.WaitOne();
                }
                else if (State == ServiceState.Abort)
                {
                    break;
                }

                var time1 = DateTime.Now.Ticks;
                DoWork();
                Heartbeat();
                var time2 = DateTime.Now.Ticks;
                var exeTime = (time2 - time1) / 10000;
                if (TimeSpan > 0 && exeTime < TimeSpan)
                {
                    Thread.Sleep((int)(TimeSpan - exeTime));
                }
            }

            SetAsync($"{KeyService}:Status", "Exit");
            Logger.Main.Info($"[{ServiceName}][{Thread.CurrentThread}] exit");
        }

        protected abstract void DoWork();
    }
}
