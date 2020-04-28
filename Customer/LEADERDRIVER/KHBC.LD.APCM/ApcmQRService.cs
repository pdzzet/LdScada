using KHBC.Core.BaseModels;
using KHBC.Core;
using System;

namespace KHBC.LD.APCM
{
    class ApcmQRService: BaseService
    {
        public override string ServiceName { get; set; }
        public ApcmQRService(string serviceName)
        {
            ServiceName = $"{serviceName}:QR";
        }

        /// <summary>
        /// 线程运行
        /// </summary>
        protected override void DoWork()
        {
            try
            {
                // 从消息队列接收命令
                var obj = BPop<MessageDataObject>(1, ApcmKeyConf.CmdQueue);
                if (obj != null)
                {
                    var actionName = obj.ActionName?.ToUpper();

                    //if (_dbActionMethodDict.ContainsKey(tableName))
                    //{
                    //    var acionDict = _dbActionMethodDict[tableName];
                    //    if (acionDict.ContainsKey(actionName))
                    //    {
                    //        var ret = acionDict[actionName].Invoke(_actionHandleDict[tableName], new object[] { obj });
                    //    }
                    //}
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
