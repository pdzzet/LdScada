using KHBC.Core;
using KHBC.Core.BaseModels;
using KHBC.Core.Device;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace KHBC.LD.APCM.Workflow
{
    public class ApcmWorkflowService : BaseService
    {
        #region 基本变量定义
        public override string ServiceName { get; set; }
        public DeviceInfo DevInfo;
        public delegate void WorkflowEventHandler(ApcmEventArgs evt);
        private readonly BlockingCollection<ApcmEventArgs> _bc = new BlockingCollection<ApcmEventArgs>();
        private Dictionary<string, WorkflowEventHandler> Events = new Dictionary<string, WorkflowEventHandler>();
        #endregion

        #region 公共函数
        public ApcmWorkflowService()
        {
        }

        protected void Read(string name)
        {
            ApcmEventConfig.CQ.Enqueue(new ApcmEventArgs
            {
                EventSrc = "WF",
                Address = name,
                Action = "READ"
            });
        }

        protected void Write(string name, object value)
        {
            ApcmEventConfig.CQ.Enqueue(new ApcmEventArgs
            {
                EventSrc = "WF",
                Address = name,
                Action = "WRITE",
                Data = value
            });
        }

        protected void RegEvent(string eventName, WorkflowEventHandler eventHandler)
        {
            if (!Events.ContainsKey(eventName))
            {
                Events[eventName] = eventHandler;
                ApcmEventConfig.RegEvent(eventName, _bc);
            }
        }

        protected void RemoveEvent(string eventName, WorkflowEventHandler eventHandler)
        {
            if (Events.ContainsKey(eventName))
            {
                ApcmEventConfig.RemoveEvent(eventName, _bc);
                Events.Remove(eventName);
            }
        }

        protected void TriggerEvent(string eventName, object value)
        {
            ApcmEventConfig.TriggerEvent(ServiceName, eventName, value);
        }

        protected void PushToMes(string destModule, Dictionary<string, object> obj)
        {
            var msg = InitMessageObject(destModule, "Add");
            msg.Data.Add(DevInfo.Id, obj);
            Push(ApcmKeyConf.MdciQueue, msg);
        }

        /// <summary>
        /// 初始化进程间消息通信对象
        /// </summary>
        /// <param name="destModule">目标模块</param>
        /// <param name="actionName">Action名称</param>
        /// <returns></returns>
        protected MessageDataObject InitMessageObject(string destModule, string actionName)
        {
            var obj = new MessageDataObject
            {
                ModuleName = SysConf.ModuleName,
                DestModule = destModule,
                ActionName = actionName,
                ServiceName = ServiceName,
                PlantId = SysConf.Main.Plant.Id,
                AssemblyLineId = SysConf.Main.AssemblyLine.Id
            };

            return obj;
        }

        /// <summary>
        /// 线程运行
        /// </summary>
        protected override void DoWork()
        {
            // 从内部Queue接收命令
            var ret = _bc.TryTake(out ApcmEventArgs evt, 3000);
            if (ret)
            {
                if (evt.Action == "AVC")
                {
                    if (evt.Data != null)
                    {
                        foreach (var d in (Dictionary<string, object>)evt.Data)
                        {
                            if (Events.ContainsKey(d.Key))
                            {
                                Events[d.Key](evt);
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
