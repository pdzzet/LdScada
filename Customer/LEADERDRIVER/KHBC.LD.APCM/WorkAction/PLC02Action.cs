using System;
using System.Linq;
using System.Text;
using KHBC.Core.Extend;
using KHBC.Core;
using KHBC.Core.Log;
using KHBC.LD.APCM.BEntity;
using KHBC.LD.DTO;
using KHBC.Modbus;
using KHBC.Modbus.WorkFlows;
using KHBC.LD.APCM.DataConvert;
using System.Collections.Generic;

namespace KHBC.LD.APCM
{
    public class PLC02Action : IActionHandle
    {
        internal static RfidStationInfo curentRfidStation;
        //料箱转移到2号位
        public Result PLC02WorkbinInEvent(ActionArgs actionArgs)
        {
            //模拟触发2号位RFIDReadInfo
            var eng = ModBusDataFactory.workFlowEngines.FirstOrDefault(x => x.ModuleName == "WFE");
            FlowBlock block = eng._workFlow.Blocks.FirstOrDefault(v => v.Name == "PLC02UpdateInfoB");
            //模拟料箱信息初始化写入RFID
            byte[] bts = InitStationInfo(actionArgs);
            block.Steps[0].Value = Encoding.ASCII.GetString(bts);
            eng.EnqueueBlock(block);
            Logger.Device.Info($" invoke PLC02WorkbinInEvent 料箱转移到2号位成功");
            return Result.Success();
        }

        private byte[] InitStationInfo(ActionArgs actionArgs)
        {
            //获取package
            int package = Constant.Package;
            //模拟2号位读取料箱信息
            RfidStationInfo station = new RfidStationInfo();
            station.UID = "Btary20190101";
            station.AssemblyLineId = "A01";
            station.BatchCode = "Z19001-DR1108";
            //二进制字符串转byte
            string msbbitstr = "00000001";
            string lsbbitstr = "11111111";
            byte msbbt = Convert.ToByte(msbbitstr, 2);
            byte lsbbt = Convert.ToByte(lsbbitstr, 2);

            station.LocaNumbInfo = new byte[7] { msbbt, lsbbt, lsbbt, lsbbt, lsbbt, lsbbt, lsbbt };
            station.MaterialCode = "Z19001-DR1108";

            station.McToMes = new byte[2] { 00000000, 00000000 };
            station.MesToMc = new byte[2] { Convert.ToByte("00000000", 2), Convert.ToByte("00011111", 2) };

            station.ProductSn01 = "16122600001";
            station.ProductSn02 = "16122600002";
            station.ProductSn03 = "16122600003";
            station.ProductSn04 = "16122600004";
            station.ProductSn05 = "16122600005";
            station.ProductSn06 = "16122600006";
            station.ProductSn07 = "16122600007";
            station.ProductSn08 = "16122600008";
            station.ProductSn09 = "16122600009";
            station.ProductSn10 = "16122600010";
            station.ProductSn11 = "16122600011";
            station.ProductSn12 = "16122600012";
            station.ProductSn13 = "16122600013";
            station.ProductSn14 = "16122600014";
            station.ProductSn15 = "16122600015";
            station.ProductSn16 = "16122600016";
            station.ProductSn17 = "16122600017";
            station.ProductSn18 = "16122600018";
            station.ProductSn19 = "16122600019";
            station.ProductSn20 = "16122600020";
            station.ProductSn21 = "16122600021";
            station.ProductSn22 = "16122600022";
            station.ProductSn23 = "16122600023";
            station.ProductSn24 = "16122600024";
            station.ProductSn25 = "16122600025";
            station.ProductSn26 = "16122600026";
            station.ProductSn27 = "16122600027";
            station.ProductSn28 = "16122600028";
            station.ProductSn29 = "16122600029";
            station.ProductSn30 = "16122600030";
            station.ProductSn31 = "16122600031";
            station.ProductSn32 = "16122600032";
            station.ProductSn33 = "16122600033";
            station.ProductSn34 = "16122600034";
            station.ProductSn35 = "16122600035";
            station.ProductSn36 = "16122600036";
            station.ProductSn37 = "16122600037";
            station.ProductSn38 = "16122600038";
            station.ProductSn39 = "16122600039";
            station.ProductSn40 = "16122600040";
            station.ProductSn41 = "16122600041";
            station.ProductSn42 = "16122600042";
            station.ProductSn43 = "16122600043";
            station.ProductSn44 = "16122600044";
            station.ProductSn45 = "16122600045";
            station.ProductSn46 = "16122600046";
            station.ProductSn47 = "16122600047";
            station.ProductSn48 = "16122600048";
            station.ProductSn49 = "16122600049";

            //0 ”00“代表空箱
            //1 ”11”待加工箱体 “12”代表加工完成箱体
            station.Status = "11";
            //0  “00”正常箱体
            //1“11 产线上料不匹配”“12“ 订单冻结箱体””13“NG空箱“14”NG料箱
            station.NgStatus = "00";
            byte[] bts = new byte[1077];
            bts = Encoding.ASCII.GetBytes(station.ToJsonStr());
            return bts;
        }

        //读取2号位信息
        public Result PLC02ReadInfoDoneEvent(ActionArgs actionArgs)
        {
            //TODO 1.读取2号位料表信息
            RfidStationInfo rfidStationInfo = (RfidStationInfo)actionArgs.StepResult.Data;
            curentRfidStation = rfidStationInfo;
            //TODO 2.更新1号料表信息
            UpdateP1Info(rfidStationInfo);
            Logger.Device.Info($" invoke PLC02ReadInfoEvent 2号位读取数据");
            return Result.Success();
        }

        private void UpdateP1Info(RfidStationInfo rfidStationInfo)
        {
            if (Plc01Action.curentRsInfoList.RfidStationInfos.Contains(rfidStationInfo))
            {
                Plc01Action.curentRsInfoList.RfidStationInfos.Remove(rfidStationInfo);
            }
            var eng = ModBusDataFactory.workFlowEngines.FirstOrDefault(x => x.ModuleName == "WFE");
            FlowBlock block = eng?._workFlow.Blocks.FirstOrDefault(v => v.Name == "PLC01UpdateInfoB");
            block.Steps[0].Value = Plc01Action.curentRsInfoList.ToJsonStr();
            eng.EnqueueBlock(block);
        }

        //读取2号位取到第几片
        public Result PLC02ReadNumDoneEvent(ActionArgs actionArgs)
        {
            //TODO 1.读取2号位取到第几片
            int p2Num = (int)actionArgs.StepResult.Data;
            //TODO 2.更新2号料表信息
            int btsIndex = p2Num / 8 - 1;
            int btIndex = p2Num % 8 - 1;
            string btstr = Convert.ToString(curentRfidStation.LocaNumbInfo[btsIndex],2);
            char[] chars = btstr.ToCharArray();
            for (int i = 0; i < 8; i++)
            {
                chars[btIndex] = '0';
            }
            btstr = chars.ToString();
            byte tarbt = Convert.ToByte(btstr,2);
            curentRfidStation.LocaNumbInfo[btsIndex]= tarbt;
            var eng = ModBusDataFactory.workFlowEngines.FirstOrDefault(x => x.ModuleName == "WFE");
            FlowBlock block = eng._workFlow.Blocks.FirstOrDefault(v => v.Name == "PLC02UpdateInfoB");
            block.Steps[0].Value = curentRfidStation.ToJsonStr();
            eng.EnqueueBlock(block);
            //存放当前取第几片料
            actionArgs.RedisClientLocal.Set($"LD:{Constant.OrderLine}:APCM:P2NUM", p2Num);
            Constant.p2Num = p2Num;
            Logger.Device.Info($" invoke PLC02ReadInfoEvent 2号位读取数据初始化");
            return Result.Success();
        }


        //料箱转移出到2号位
        public Result PLC02WorkbinOutEvent(ActionArgs actionArgs)
        {
            Logger.Device.Info($" invoke PLC02WorkbinOutEvent 料箱转移出2号位成功");
            return Result.Success();
        }
    }
}
