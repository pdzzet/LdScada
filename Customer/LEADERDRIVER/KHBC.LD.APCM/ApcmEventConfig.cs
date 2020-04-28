using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHBC.LD.APCM
{
    public class ApcmEventArgs
    {
        public string EventSrc;
        public string Action;
        public string Address;
        public object Data;
        public string DataKey;
    }

    public class ApcmEventConfig
    {
        public static readonly ConcurrentQueue<ApcmEventArgs> CQ = new ConcurrentQueue<ApcmEventArgs>();
        public static ConcurrentDictionary<string, List<BlockingCollection<ApcmEventArgs>>> Events = new ConcurrentDictionary<string, List<BlockingCollection<ApcmEventArgs>>>();
        public static readonly Dictionary<string, BlockingCollection<ApcmEventArgs>> BQ = new Dictionary<string, BlockingCollection<ApcmEventArgs>>
        {
            ["WF"] = new BlockingCollection<ApcmEventArgs>(),
            ["QR"] = new BlockingCollection<ApcmEventArgs>(),
        };

        /// <summary>
        /// 注册事件
        /// 注意：注册过程在初始化过程进行，因此不需要考虑线程安全问题
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="queue">对应的Queue</param>
        public static void RegEvent(string eventName, BlockingCollection<ApcmEventArgs> queue)
        {
            if (!Events.ContainsKey(eventName))
            {
                Events[eventName] = new List<BlockingCollection<ApcmEventArgs>>();
            }
            if (!Events[eventName].Contains(queue))
            {
                Events[eventName].Add(queue);
            }
        }

        public static void RemoveEvent(string eventName, BlockingCollection<ApcmEventArgs> queue)
        {
            if (Events.ContainsKey(eventName) && Events[eventName].Contains(queue))
            {
                Events[eventName].Remove(queue);
            }
        }

        public static void TriggerEvent(string evtSrc, string eventName, object value)
        {
            if (Events.ContainsKey(eventName))
            {
                var evt = new ApcmEventArgs
                {
                    EventSrc = evtSrc,
                    Action = "AVC",
                    Data = new Dictionary<string, object>
                    {
                        [eventName] = value
                    }
                };

                foreach (var bc in Events[eventName])
                {
                    bc.Add(evt);
                }
            }
        }
    }
}
