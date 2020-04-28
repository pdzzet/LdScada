using System;
using System.Collections.Generic;
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

namespace KHBC.LD.APCM
{
    public class Plc01Action : IActionHandle
    {
        internal static RfidStationInfoList curentRsInfoList;
        //请求空箱上料
        public Result EmptyAgvCallEvent(ActionArgs actionArgs)
        {
            RChargingState chargingState = new RChargingState()
            {
                LINE = Constant.OrderLine,
                IS_READ = 0,
                DISPATCH_STATE = 2,
                FLAG = 1,
                NO_FROM = "1"
            };
            actionArgs.RedisClientRemote.LPush("LD:A00:MDCI:CHARGING_STATE:Q", chargingState);
            //保存上料请求到队列
            Logger.Device.Info($" invoke EmptyAgvCallEvent 请求空箱上料成功");
            return Result.Success();
        }
        //空箱任务完成
        public Result EmptyTaskDoneEvent(ActionArgs actionArgs)
        {
            Logger.Device.Info($" invoke EmptyTaskDoneEvent 空箱任务完成");
            return Result.Success();
        }


        //请求MES正常上料
        public Result CommAgvCallEvent(ActionArgs actionArgs)
        {
            RChargingState chargingState = new RChargingState()
            {
                LINE = Constant.OrderLine,
                IS_READ = 0,
                DISPATCH_STATE = 0,
                FLAG = 1,
                NO_FROM = "1"
            };

            actionArgs.RedisClientRemote.LPush("LD:A00:MDCI:CHARGING_STATE:Q", chargingState);
            Logger.Device.Info("invoke CommAgvCallEvent 请求正常上料成功");
            return Result.Success();
        }

        //记录AGV到达1号位信号
        public Result AgvArriviedEvent(ActionArgs actionArgs)
        {
            var eng = ModBusDataFactory.workFlowEngines.FirstOrDefault(x => x.ModuleName == "WFE");
            FlowBlock block = eng?._workFlow.Blocks.FirstOrDefault(v => v.Name == "PLC01UpdateInfoB");
            //模拟3个料箱信息写入RFID
            byte[] bts = InitRfidStationInfos(actionArgs);
            if (block == null) return Result.Fail();
            block.Steps[0].Value = Encoding.Unicode.GetString(bts);
            eng.EnqueueBlock(block);
            Logger.Device.Info($" invoke AgvArriviedEvent 记录AGV到达1号位信号");
            return Result.Success();
        }

        //模拟数据
        private byte[] InitRfidStationInfos(ActionArgs actionArgs)
        {
            RfidStationInfo stationInfo1 = new RfidStationInfo();
            stationInfo1.UID = "Btary20190101";
            stationInfo1.AssemblyLineId = "A01";
            stationInfo1.BatchCode = "Z19001-DR1108";

            //二进制字符串转byte
            string msbbitstr = "00000001";
            string lsbbitstr = "11111111";
            byte msbbt = Convert.ToByte(msbbitstr, 2);
            byte lsbbt = Convert.ToByte(lsbbitstr, 2);

            stationInfo1.LocaNumbInfo = new byte[7] { msbbt, lsbbt, lsbbt, lsbbt, lsbbt, lsbbt, lsbbt };
            stationInfo1.MaterialCode = "Z19001-DR1108";

            stationInfo1.McToMes = new byte[2] { 00000000, 00000000 };
            stationInfo1.MesToMc = new byte[2] { Convert.ToByte("00000000", 2), Convert.ToByte("00011111", 2) };

            stationInfo1.ProductSn01 = "16122600001";
            stationInfo1.ProductSn02 = "16122600002";
            stationInfo1.ProductSn03 = "16122600003";
            stationInfo1.ProductSn04 = "16122600004";
            stationInfo1.ProductSn05 = "16122600005";
            stationInfo1.ProductSn06 = "16122600006";
            stationInfo1.ProductSn07 = "16122600007";
            stationInfo1.ProductSn08 = "16122600008";
            stationInfo1.ProductSn09 = "16122600009";
            stationInfo1.ProductSn10 = "16122600010";
            stationInfo1.ProductSn11 = "16122600011";
            stationInfo1.ProductSn12 = "16122600012";
            stationInfo1.ProductSn13 = "16122600013";
            stationInfo1.ProductSn14 = "16122600014";
            stationInfo1.ProductSn15 = "16122600015";
            stationInfo1.ProductSn16 = "16122600016";
            stationInfo1.ProductSn17 = "16122600017";
            stationInfo1.ProductSn18 = "16122600018";
            stationInfo1.ProductSn19 = "16122600019";
            stationInfo1.ProductSn20 = "16122600020";
            stationInfo1.ProductSn21 = "16122600021";
            stationInfo1.ProductSn22 = "16122600022";
            stationInfo1.ProductSn23 = "16122600023";
            stationInfo1.ProductSn24 = "16122600024";
            stationInfo1.ProductSn25 = "16122600025";
            stationInfo1.ProductSn26 = "16122600026";
            stationInfo1.ProductSn27 = "16122600027";
            stationInfo1.ProductSn28 = "16122600028";
            stationInfo1.ProductSn29 = "16122600029";
            stationInfo1.ProductSn30 = "16122600030";
            stationInfo1.ProductSn31 = "16122600031";
            stationInfo1.ProductSn32 = "16122600032";
            stationInfo1.ProductSn33 = "16122600033";
            stationInfo1.ProductSn34 = "16122600034";
            stationInfo1.ProductSn35 = "16122600035";
            stationInfo1.ProductSn36 = "16122600036";
            stationInfo1.ProductSn37 = "16122600037";
            stationInfo1.ProductSn38 = "16122600038";
            stationInfo1.ProductSn39 = "16122600039";
            stationInfo1.ProductSn40 = "16122600040";
            stationInfo1.ProductSn41 = "16122600041";
            stationInfo1.ProductSn42 = "16122600042";
            stationInfo1.ProductSn43 = "16122600043";
            stationInfo1.ProductSn44 = "16122600044";
            stationInfo1.ProductSn45 = "16122600045";
            stationInfo1.ProductSn46 = "16122600046";
            stationInfo1.ProductSn47 = "16122600047";
            stationInfo1.ProductSn48 = "16122600048";
            stationInfo1.ProductSn49 = "16122600049";

            //0 ”00“代表空箱
            //1 ”11”待加工箱体 “12”代表加工完成箱体
            stationInfo1.Status = "11";
            //0  “00”正常箱体
            //1“11 产线上料不匹配”“12“ 订单冻结箱体””13“NG空箱“14”NG料箱
            stationInfo1.NgStatus = "00";
            
            RfidStationInfo stationInfo2 = new RfidStationInfo();
            stationInfo2.UID = "Btary20190102";
            stationInfo2.AssemblyLineId = "A01";
            stationInfo2.BatchCode = "Z19001-DR1108";
            
            stationInfo2.LocaNumbInfo = new byte[7] { msbbt, lsbbt, lsbbt, lsbbt, lsbbt, lsbbt, lsbbt };
            stationInfo2.MaterialCode = "Z19001-DR1108";

            stationInfo2.McToMes = new byte[2] { 00000000, 00000000 };
            stationInfo2.MesToMc = new byte[2] { Convert.ToByte("00000000", 2), Convert.ToByte("00011111", 2) };

            stationInfo2.ProductSn01 = "16122600001";
            stationInfo2.ProductSn02 = "16122600002";
            stationInfo2.ProductSn03 = "16122600003";
            stationInfo2.ProductSn04 = "16122600004";
            stationInfo2.ProductSn05 = "16122600005";
            stationInfo2.ProductSn06 = "16122600006";
            stationInfo2.ProductSn07 = "16122600007";
            stationInfo2.ProductSn08 = "16122600008";
            stationInfo2.ProductSn09 = "16122600009";
            stationInfo2.ProductSn10 = "16122600010";
            stationInfo2.ProductSn11 = "16122600011";
            stationInfo2.ProductSn12 = "16122600012";
            stationInfo2.ProductSn13 = "16122600013";
            stationInfo2.ProductSn14 = "16122600014";
            stationInfo2.ProductSn15 = "16122600015";
            stationInfo2.ProductSn16 = "16122600016";
            stationInfo2.ProductSn17 = "16122600017";
            stationInfo2.ProductSn18 = "16122600018";
            stationInfo2.ProductSn19 = "16122600019";
            stationInfo2.ProductSn20 = "16122600020";
            stationInfo2.ProductSn21 = "16122600021";
            stationInfo2.ProductSn22 = "16122600022";
            stationInfo2.ProductSn23 = "16122600023";
            stationInfo2.ProductSn24 = "16122600024";
            stationInfo2.ProductSn25 = "16122600025";
            stationInfo2.ProductSn26 = "16122600026";
            stationInfo2.ProductSn27 = "16122600027";
            stationInfo2.ProductSn28 = "16122600028";
            stationInfo2.ProductSn29 = "16122600029";
            stationInfo2.ProductSn30 = "16122600030";
            stationInfo2.ProductSn31 = "16122600031";
            stationInfo2.ProductSn32 = "16122600032";
            stationInfo2.ProductSn33 = "16122600033";
            stationInfo2.ProductSn34 = "16122600034";
            stationInfo2.ProductSn35 = "16122600035";
            stationInfo2.ProductSn36 = "16122600036";
            stationInfo2.ProductSn37 = "16122600037";
            stationInfo2.ProductSn38 = "16122600038";
            stationInfo2.ProductSn39 = "16122600039";
            stationInfo2.ProductSn40 = "16122600040";
            stationInfo2.ProductSn41 = "16122600041";
            stationInfo2.ProductSn42 = "16122600042";
            stationInfo2.ProductSn43 = "16122600043";
            stationInfo2.ProductSn44 = "16122600044";
            stationInfo2.ProductSn45 = "16122600045";
            stationInfo2.ProductSn46 = "16122600046";
            stationInfo2.ProductSn47 = "16122600047";
            stationInfo2.ProductSn48 = "16122600048";
            stationInfo2.ProductSn49 = "16122600049";

            //0 ”00“代表空箱
            //1 ”11”待加工箱体 “12”代表加工完成箱体
            stationInfo2.Status = "11";
            //0  “00”正常箱体
            //1“11 产线上料不匹配”“12“ 订单冻结箱体””13“NG空箱“14”NG料箱
            stationInfo2.NgStatus = "00";

            RfidStationInfo stationInfo3 = new RfidStationInfo();
            stationInfo3.UID = "Btary20190103";
            stationInfo3.AssemblyLineId = "A01";
            stationInfo3.BatchCode = "Z19001-DR1108";
            
            stationInfo3.LocaNumbInfo = new byte[7] { msbbt, lsbbt, lsbbt, lsbbt, lsbbt, lsbbt, lsbbt };
            stationInfo3.MaterialCode = "Z19001-DR1108";

            stationInfo3.McToMes = new byte[2] { 00000000, 00000000 };
            stationInfo3.MesToMc = new byte[2] { Convert.ToByte("00000000", 2), Convert.ToByte("00011111", 2) };

            stationInfo3.ProductSn01 = "16122600001";
            stationInfo3.ProductSn02 = "16122600002";
            stationInfo3.ProductSn03 = "16122600003";
            stationInfo3.ProductSn04 = "16122600004";
            stationInfo3.ProductSn05 = "16122600005";
            stationInfo3.ProductSn06 = "16122600006";
            stationInfo3.ProductSn07 = "16122600007";
            stationInfo3.ProductSn08 = "16122600008";
            stationInfo3.ProductSn09 = "16122600009";
            stationInfo3.ProductSn10 = "16122600010";
            stationInfo3.ProductSn11 = "16122600011";
            stationInfo3.ProductSn12 = "16122600012";
            stationInfo3.ProductSn13 = "16122600013";
            stationInfo3.ProductSn14 = "16122600014";
            stationInfo3.ProductSn15 = "16122600015";
            stationInfo3.ProductSn16 = "16122600016";
            stationInfo3.ProductSn17 = "16122600017";
            stationInfo3.ProductSn18 = "16122600018";
            stationInfo3.ProductSn19 = "16122600019";
            stationInfo3.ProductSn20 = "16122600020";
            stationInfo3.ProductSn21 = "16122600021";
            stationInfo3.ProductSn22 = "16122600022";
            stationInfo3.ProductSn23 = "16122600023";
            stationInfo3.ProductSn24 = "16122600024";
            stationInfo3.ProductSn25 = "16122600025";
            stationInfo3.ProductSn26 = "16122600026";
            stationInfo3.ProductSn27 = "16122600027";
            stationInfo3.ProductSn28 = "16122600028";
            stationInfo3.ProductSn29 = "16122600029";
            stationInfo3.ProductSn30 = "16122600030";
            stationInfo3.ProductSn31 = "16122600031";
            stationInfo3.ProductSn32 = "16122600032";
            stationInfo3.ProductSn33 = "16122600033";
            stationInfo3.ProductSn34 = "16122600034";
            stationInfo3.ProductSn35 = "16122600035";
            stationInfo3.ProductSn36 = "16122600036";
            stationInfo3.ProductSn37 = "16122600037";
            stationInfo3.ProductSn38 = "16122600038";
            stationInfo3.ProductSn39 = "16122600039";
            stationInfo3.ProductSn40 = "16122600040";
            stationInfo3.ProductSn41 = "16122600041";
            stationInfo3.ProductSn42 = "16122600042";
            stationInfo3.ProductSn43 = "16122600043";
            stationInfo3.ProductSn44 = "16122600044";
            stationInfo3.ProductSn45 = "16122600045";
            stationInfo3.ProductSn46 = "16122600046";
            stationInfo3.ProductSn47 = "16122600047";
            stationInfo3.ProductSn48 = "16122600048";
            stationInfo3.ProductSn49 = "16122600049";

            //0 ”00“代表空箱
            //1 ”11”待加工箱体 “12”代表加工完成箱体
            stationInfo3.Status = "11";
            //0  “00”正常箱体
            //1“11 产线上料不匹配”“12“ 订单冻结箱体””13“NG空箱“14”NG料箱
            stationInfo3.NgStatus = "00";
            
            List<RfidStationInfo> rfidStationInfos = new List<RfidStationInfo>();
            rfidStationInfos.Add(stationInfo1);
            rfidStationInfos.Add(stationInfo2);
            rfidStationInfos.Add(stationInfo3);
            byte[] bts1 = new byte[1]{01};
            byte[] bts2 = new byte[1]{02};
            byte[] bts3 = new byte[1]{03};
            byte[] bts4 = Encoding.Unicode.GetBytes(rfidStationInfos.ToJsonStr());
            byte[] bts5 = new byte[bts1.Length + bts2.Length + bts3.Length + bts4.Length];
            bts1.CopyTo(bts5, 0);
            bts2.CopyTo(bts5, bts1.Length);
            bts3.CopyTo(bts5, bts1.Length + bts2.Length);
            bts4.CopyTo(bts5, bts1.Length + bts2.Length + bts3.Length);
            return bts5;
        }
        //读取1号位料箱信息数据
        private RfidStationInfoList ParseRfidStationInfoList(byte[] bts)
        {
            string btrayId1 = Encoding.Unicode.GetString(bts.Take(1).ToArray());
            string btrayId2 = Encoding.Unicode.GetString(bts.Skip(1).Take(1).ToArray());
            string btrayId3 = Encoding.Unicode.GetString(bts.Skip(2).Take(1).ToArray());
            byte[] bts4 = bts.Skip(3).Take(bts.Length - 3).ToArray();
            List<RfidStationInfo> rfidStationInfos = JsonExtension.JsonToModel<List<RfidStationInfo>>(Encoding.Unicode.GetString(bts4));
            RfidStationInfoList infoList = new RfidStationInfoList()
            {
                BtrayId1 = btrayId1,
                BtrayId2 = btrayId2,
                BtrayId3 = btrayId3,
                RfidStationInfos = rfidStationInfos
            };
            return infoList;
        }
        //读取料箱信息，更新MES上下料信息表
        public Result PLC01ReadInfoDoneEvent(ActionArgs actionArgs)
        {
            //获取到P1Info
            byte[] bts = Encoding.ASCII.GetBytes(actionArgs.StepResult.Data.ToString());
            RfidStationInfoList list = ParseRfidStationInfoList(bts);
            curentRsInfoList = list;
            //更新上下料信息表
            RChargingState chargingState = new RChargingState()
            {
                LINE = Constant.OrderLine,
                IS_READ = 0,
                DISPATCH_STATE = 1,
                FLAG = 2,
                NO_FROM = "1",
                BTRAY_ID1 = list.RfidStationInfos[0].UID,
                BTRAY_ID2 = list.RfidStationInfos[1].UID,
                BTRAY_ID3 = list.RfidStationInfos[2].UID
            };
            actionArgs.RedisClientRemote.LPush("LD:A00:MDCI:CHARGING_STATE:Q", chargingState);
            Logger.Device.Info($" invoke UpCHARGING_STATEEvent 更新MES上下料信息表成功");
            return Result.Success();
        }
        //换箱请求下料
        public Result PLC01AgvChangeEvent(ActionArgs actionArgs)
        {
            //换箱请求下料
            RChargingState chargingState = new RChargingState()
            {
                LINE = Constant.OrderLine,
                IS_READ = 0,
                DISPATCH_STATE = 1,
                FLAG = 1,
                NO_FROM = "1"
            };
            actionArgs.RedisClientRemote.Set("LD:A00:MDCI:CHARGING_STATE:Q", chargingState);
            Logger.Device.Info($" invoke PLC01AgvChangeEvent 换箱请求下料");
            return Result.Success();
        }
    }
}
