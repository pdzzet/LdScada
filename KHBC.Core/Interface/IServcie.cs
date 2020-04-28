using System;
using KHBC.Core.Enums;
using KHBC.Core.FrameBase;

namespace KHBC.Core.Interface
{
    /// <summary>
    /// 系统服务的接口（服务提供者）
    /// 系统后台线程
    /// </summary>
    public interface IService : IDependency
    {
        string ServiceName { get; }
        /// <summary>
        /// 订阅服务
        /// </summary>
        /// <param name="action"></param>
        void Subscribe(Action<object> action);
        /// <summary>
        /// 取消订阅服务
        /// </summary>
        /// <param name="action"></param>
        void UnSubscribe(Action<object> action);
        /// <summary>
        /// 服务推送
        /// </summary>
        //event Action<object> PushEvent;
        /// <summary>
        /// 服务开始
        /// </summary>
        void Start();
        /// <summary>
        /// 服务挂起
        /// </summary>
        void Suspend();
        /// <summary>
        /// 服务恢复
        /// </summary>
        void Resume();
        /// <summary>
        /// 服务终止
        /// </summary>
        void Abort();
        /// <summary>
        /// 线程任务
        /// </summary>
        void ThreadWork();
        /// <summary>
        /// 任务运行标识
        /// </summary>
        ServiceState State { get; set; }
        /// <summary>
        /// 运行间隔ms
        /// </summary>
        long TimeSpan { get; set; }

    }

}
