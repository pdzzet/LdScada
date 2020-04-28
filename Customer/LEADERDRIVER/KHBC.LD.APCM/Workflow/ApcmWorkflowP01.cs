using KHBC.Core;
using KHBC.Core.Device;
using KHBC.LD.APCM.BEntity;
using System;
using System.Collections.Generic;

namespace KHBC.LD.APCM.Workflow
{
    public class ApcmWorkflowP01 : ApcmWorkflowService
    {
        public override string ServiceName { get; set; }
        public ApcmWorkflowP01(DeviceInfo devInfo)
        {
            DevInfo = devInfo;
            ServiceName = $"{SysConf.ModuleName}:WF_P01";

            #region 一号位事件
            RegEvent(ApcmDevcieConfig.EVENT_BP1SNPHLEFT, OnEvent_P1_In);
            RegEvent(ApcmDevcieConfig.EVENT_P1_AGVCALL, OnEvent_P1_AgvCall);
            RegEvent(ApcmDevcieConfig.EVENT_P1_BAGVCHANGE, OnEvent_P1_AgvChange);
            RegEvent(ApcmDevcieConfig.EVENT_P1_BTERORDERTASKCALL, OnEvent_P1_TerOrderTaskCall);
            RegEvent(ApcmDevcieConfig.EVENT_P1_BNULLTASKDONE, OnEvent_P1_NullTaskDone);
            RegEvent(ApcmDevcieConfig.EVENT_P1_BENREADINFO, OnEvent_P1_EnReadInfo);
            RegEvent(ApcmDevcieConfig.PROPERTY_P1_INFO, OnEvent_P1_InfoReadDone);

            RegEvent(ApcmDevcieConfig.EVENT_CUSTOM_UPDATE_P1_INFO, OnEVENT_CUSTOM_UPDATE_P1_INFO);
            #endregion
        }

        #region 一号位事件
        public void OnEvent_P1_In(ApcmEventArgs evt)
        {

        }

        public void OnEvent_P1_AgvCall(ApcmEventArgs evt)
        {
            PushToMes(ApcmDevcieConfig.EVENT_MES_CHARGING_STATE, new Dictionary<string, object>
            {
                ["LINE"] = SysConf.Main.AssemblyLine.Id,
                ["IS_READ"] = 0,
                ["DISPATCH_STATE"] = 0,
                ["FLAG"] = 1,
                ["NO_FROM"] = "1"
            });
        }

        public void OnEvent_P1_AgvChange(ApcmEventArgs evt)
        {
            PushToMes(ApcmDevcieConfig.EVENT_MES_CHARGING_STATE, new Dictionary<string, object>
            {
                ["LINE"] = SysConf.Main.AssemblyLine.Id,
                ["IS_READ"] = 0,
                ["DISPATCH_STATE"] = 1,
                ["FLAG"] = 1,
                ["NO_FROM"] = "1"
            });
        }



        public void OnEvent_P1_TerOrderTaskCall(ApcmEventArgs evt)
        {
            Write(ApcmDevcieConfig.ACTION_P1_TERORDER, 1);
        }

        public void OnEvent_P1_EnReadInfo(ApcmEventArgs evt)
        {
            Read(ApcmDevcieConfig.PROPERTY_P1_INFO);
        }

        public void OnEvent_P1_InfoReadDone(ApcmEventArgs evt)
        {
            RfidStationInfoList list = ParseRfidStationInfoList((byte[])((Dictionary<string, object>)evt.Data)[ApcmDevcieConfig.PROPERTY_P1_INFO]);
            Set($"{ApcmKeyConf.DataCollection}:{ApcmDevcieConfig.PROPERTY_P1_INFO}:DATA", list);
            //更新上下料信息表
            PushToMes(ApcmDevcieConfig.EVENT_MES_CHARGING_STATE, new Dictionary<string, object>
            {
                ["LINE"] = SysConf.Main.AssemblyLine.Id,
                ["IS_READ"] = 0,
                ["DISPATCH_STATE"] = 1,
                ["FLAG"] = 2,
                ["NO_FROM"] = "1",
                ["BTRAY_ID1"] = list.RfidStationInfos[0].UID,
                ["BTRAY_ID2"] = list.RfidStationInfos[1].UID,
                ["BTRAY_ID3"] = list.RfidStationInfos[2].UID
            });
        }
        //读取1号位料箱信息数据
        private RfidStationInfoList ParseRfidStationInfoList(byte[] bts)
        {
            RfidStationInfoList stationInfoList = new RfidStationInfoList();
            object obj = StructTransform.BytesToStruct(bts, stationInfoList.GetType());
            return (RfidStationInfoList)obj;
        }

        public void OnEvent_P1_NullTaskDone(ApcmEventArgs evt)
        {

        }

        public void OnEVENT_CUSTOM_UPDATE_P1_INFO(ApcmEventArgs evt)
        {
            RfidStationInfoList stationInfoList = Get<RfidStationInfoList>($"{ApcmKeyConf.DataCollection}:{ApcmDevcieConfig.PROPERTY_P1_INFO}:DATA");
            RfidStationInfo rfidStationInfo = Get<RfidStationInfo>($"{ApcmKeyConf.DataCollection}:{ApcmDevcieConfig.PROPERTY_P2_WORKBININFO}:DATA");
            int i = 0;
            foreach (RfidStationInfo info in stationInfoList.RfidStationInfos)
            {
                if (info.Equals(stationInfoList.RfidStationInfos[i]))
                {
                    OrderRfidStationInfos(i, stationInfoList);
                }
                i++;
            }
            Write(ApcmDevcieConfig.PROPERTY_P1_INFO, StructTransform.StructToBytes(stationInfoList));
        }

        //1号位更新排序
        private RfidStationInfoList OrderRfidStationInfos(int i, RfidStationInfoList stationInfoList)
        {
            if (i == 0)
            {
                stationInfoList.RfidStationInfos[0] = stationInfoList.RfidStationInfos[1];
                stationInfoList.RfidStationInfos[1] = stationInfoList.RfidStationInfos[2];
                stationInfoList.RfidStationInfos[2] = new RfidStationInfo();
            }
            else if (i == 1)
            {
                stationInfoList.RfidStationInfos[1] = stationInfoList.RfidStationInfos[2];
                stationInfoList.RfidStationInfos[2] = new RfidStationInfo();
            }
            else if (i == 2)
            {
                stationInfoList.RfidStationInfos[i] = new RfidStationInfo();
            }
            return stationInfoList;
        }
        #endregion
    }
}
