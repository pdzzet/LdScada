using CSRedis;
using KHBC.Core.BaseModels;
using KHBC.Core;
using KHBC.Core.Log;
using KHBC.DataAccess;
using KHBC.LD.BDCI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KHBC.LD.BDCI
{
    public class BdciService : BaseService
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public sealed override string ServiceName { get; set; }
        public static Repository BcRepository;
        /// <summary>
        /// KEY: 远程数据库记录队列
        /// </summary>
        private readonly string _cmdQueueKey;

        private readonly Dictionary<string, IActionHandle> _actionHandleDict = new Dictionary<string, IActionHandle>();
        private readonly Dictionary<string, Dictionary<string, MethodInfo>> _dbActionMethodDict = new Dictionary<string, Dictionary<string, MethodInfo>>();
        public static Dictionary<string, Dictionary<string, DevicePropertyDefine>> DevicePropertyDefineMap = new Dictionary<string, Dictionary<string, DevicePropertyDefine>>();
        public BdciService()
        {
            var handlers = Common.GetAllInstance<IActionHandle>();
            foreach (var handle in handlers)
            {
                var handleName = handle.Name.ToUpper();
                if (!_dbActionMethodDict.ContainsKey(handleName))
                {
                    _dbActionMethodDict[handleName] = new Dictionary<string, MethodInfo>();
                }

                var t = handle.GetType();
                var methods = t.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(x => !x.IsSpecialName);
                foreach (var m in methods)
                {
                    _dbActionMethodDict[handleName].Add(m.Name.ToUpper(), m);
                }

                _actionHandleDict[handleName] = handle;
            }

            ServiceName = $"{SysConf.ModuleName}";
            _cmdQueueKey = $"{SysConf.KeyPlant}:{SysConf.ModuleName}:Q";
            BcRepository = new Repository(SysConf.Main.DbBC);
            try
            {
                var propDefList = BcRepository.GetList<DevicePropertyDefine>();
                foreach (var p in propDefList)
                {
                    if (!DevicePropertyDefineMap.ContainsKey(p.ModuleName))
                    {
                        DevicePropertyDefineMap[p.ModuleName] = new Dictionary<string, DevicePropertyDefine>();
                    }

                    DevicePropertyDefineMap[p.ModuleName][p.Name] = p;
                }
            }
            catch (Exception ex)
            {
                Logger.Main.Error($"[{ServiceName}] 查询数据库失败{ex.Message}");
            }
        }

        /// <summary>
        /// 工作线程
        /// 不允许这在这里再次循环
        /// </summary>
        protected override void DoWork()
        {
            try
            {
                var obj = BPop<MessageDataObject>(3, _cmdQueueKey);
                if (obj != null)
                {
                    var actionName = obj.ActionName?.ToUpper();

                    switch (actionName)
                    {
                        case "ADD":
                            DeviceDataInfoController.Add(obj);
                            break;
                            //Tony added 2020-4-10
                        case "UPDATE":
                            DeviceDataInfoController.Update(obj);
                            break;
                    }

                    var tableName = "DEV_" + obj.DestModule?.ToUpper();
                    if (_dbActionMethodDict.ContainsKey(tableName))
                    {
                        var acionDict = _dbActionMethodDict[tableName];
                        if (acionDict.ContainsKey(actionName))
                        {
                            var ret = acionDict[actionName].Invoke(_actionHandleDict[tableName], new object[] { obj });
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
