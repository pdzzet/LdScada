using CSRedis;
using KHBC.Core.BaseModels;
using KHBC.Core;
using KHBC.Core.Log;
using KHBC.LD.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KHBC.Modbus
{
    public class MsgRecvService : BaseService
    {
        /// <summary>
        /// 设备实例
        /// </summary>
        public string ModuleName { get; set; } = "MRS";
        public sealed override string ServiceName { get; set; }

        private CSRedisClient RedisClientLocal;
        private CSRedisClient RedisClientRemote;

        public MsgRecvService()
        {
            ServiceName = ModuleName;
        }

        public override void ThreadWork()
        {
            try
            {
                RedisClientLocal = new CSRedisClient(SysConf.Main.RedisLocal.ConnectStrings);
                Logger.Main.Info($"[{ServiceName}]初始化LOCAL REDIS CLIENT成功: \"{SysConf.Main.RedisLocal.ConnectStrings}\"");
            }
            catch (Exception ex)
            {
                Logger.Main.Error($"[{ServiceName}]初始化LOCAL REDIS CLIENT失败: \"{SysConf.Main.RedisLocal.ConnectStrings}\", {ex.Message}");
            }

            try
            {
                RedisClientRemote = new CSRedisClient(SysConf.Main.RedisRemote.ConnectStrings);
                Logger.Main.Info($"[{ServiceName}]初始化REMOTE REDIS CLIENT成功: \"{SysConf.Main.RedisRemote.ConnectStrings}\"");
            }
            catch (Exception ex)
            {
                Logger.Main.Error($"[{ServiceName}]初始化REMOTE REDIS CLIENT失败: {ex.Message}");
            }

            base.ThreadWork();
        }

        protected override void DoWork()
        {
            if (ModBusDataFactory.LineReady == 0)
            {
                //获取到工位派工单
                string orders = RedisClientRemote?.Get("LD:A01:MCIM:wc001");
                RedisClientLocal.Set("LD:A01:MCIM:wc001", orders);
                var orderc = RedisClientLocal.Get("LD:A01:MCIM:wc001");
                List<RMesDispatch> orderl = JsonExtension.JsonToModel<List<RMesDispatch>>(orderc);
                //暂存当前工单的产线标记
                ModBusDataFactory.OrderLine = orderl[0].LINE;
                foreach (RMesDispatch dispatch in orderl)
                {
                    ModBusDataFactory.dispatrchs.Enqueue(dispatch);
                }
                //从BC获取质量任务单,下发给检测机

                //从BC获取物料编号对应的料箱规格存放在本地
                //Repository _mesRepository = new Repository(SysConf.Main.DbMES);
                //var pROD_INFO = _mesRepository.Single<LD.MDCI.MESModels.TB_PROD_INFO>(w =>
                //    w.MATERIAL_CODE == orderl[0].MATERIAL_CODE);
                var rProdInfo = new RProdInfo();
                Dictionary<string, int> PackageInfo = InitizePackageInfo();
                int Package = PackageInfo[rProdInfo.PACKAGE];
                RedisClientLocal.Set($"LD:{ModBusDataFactory.OrderLine}:APCM:PACKAGE", Package);
                //拿到单子后不再取单子，机器变为忙碌状态；到加工完毕后改为空闲
                //ModbusConfModel confModel = ModbusConf.CnfModel.FirstOrDefault(v => v.Name == "PLC");
                RedisClientLocal.Set($"LD:{ModBusDataFactory.OrderLine}:APCM:READY", 1);
                ModBusDataFactory.LineReady = 1;
            }
        }

        private Dictionary<string, int> InitizePackageInfo()
        {
            Dictionary<string, int> packageInfo = new Dictionary<string, int>();
            packageInfo.Add("3*3", 9);
            packageInfo.Add("4*4", 16);
            packageInfo.Add("5*5", 25);
            packageInfo.Add("6*6", 36);
            packageInfo.Add("7*7", 49);
            return packageInfo;
        }
    }
}
