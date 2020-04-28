using KHBC.Core;
using KHBC.Core.Device;
using KHBC.LD.APCM.BEntity;
using System;
using System.Collections.Generic;

namespace KHBC.LD.APCM.Workflow
{
    public class ApcmWorkflowP04 : ApcmWorkflowService
    {
        public ApcmWorkflowP04(DeviceInfo devInfo)
        {
            DevInfo = devInfo;
            ServiceName = $"{SysConf.ModuleName}:WF_P04";

            #region 四号位事件
            RegEvent(ApcmDevcieConfig.EVENT_P4_WORKBINSPEOFF, OnEvent_P4_WorkbinSpeOff);
            RegEvent(ApcmDevcieConfig.EVENT_P4_REQPCREAD, OnEvent_P4_ReqPcRead);
            RegEvent(ApcmDevcieConfig.EVENT_P4_PLCRXDONE, OnEvent_P4_PlcRxDone);
            RegEvent(ApcmDevcieConfig.EVENT_P4_PLCUPDATEDONE, OnEvent_P4_PlcUpdateDone);
            RegEvent(ApcmDevcieConfig.EVENT_P4_PLCRXDONE_SPE, OnEvent_P4_PlcRxDone_Spe);
            RegEvent(ApcmDevcieConfig.EVENT_P4_PLCUPDATEDONE_SPE, OnEvent_P4_PlcUpdateDone_Spe);
            RegEvent(ApcmDevcieConfig.PROPERTY_P4_NUMINFO, OnEvent_P4_NumInfoReadDone);
            RegEvent(ApcmDevcieConfig.PROPERTY_P4_PLCINFO, OnEventP4_PlcInfoReadDone);

            RegEvent(ApcmDevcieConfig.EVENT_CUSTOM_P4_WORKBINSPE, OnEvent_CUSTOM_P4_WORKBINSPE);
            RegEvent(ApcmDevcieConfig.ACTION_P4_UPDATECPPINFO, OnEvent_P4_UPDATECPPINFO);
            RegEvent(ApcmDevcieConfig.ACTION_P4_SPECPPINFO, OnEvent_P4_SPECPPINFO);
            #endregion
        }

       
        #region 四号位事件
        public void OnEvent_P4_NumInfoReadDone(ApcmEventArgs evt)
        {
            int P4Num = int.Parse(((Dictionary<string, object>)evt.Data)[ApcmDevcieConfig.PROPERTY_P4_NUMINFO].ToString());
            //更新本地缓存
            Set($"{ApcmKeyConf.DataCollection}:{ApcmDevcieConfig.PROPERTY_P4_NUMINFO}:DATA", P4Num);
        }
        public void OnEvent_CUSTOM_P4_WORKBINSPE(ApcmEventArgs evt)
        {
            int spe = int.Parse(((Dictionary<string, object>)evt.Data)[ApcmDevcieConfig.ACTION_P4_WORKBINSPE].ToString());
            //更新本地缓存
            Set($"{ApcmKeyConf.DataCollection}:{ApcmDevcieConfig.ACTION_P4_WORKBINSPE}:DATA", spe);
        }
        
        public void OnEvent_P4_ReqPcRead(ApcmEventArgs evt)
        {
            Read(ApcmDevcieConfig.PROPERTY_P4_PLCINFO);
        }
        public void OnEventP4_PlcInfoReadDone(ApcmEventArgs evt)
        {
            P4PlcInfo currentP4PlcInfo = new P4PlcInfo();
            currentP4PlcInfo = (P4PlcInfo)StructTransform.BytesToStruct((byte[])((Dictionary<string, object>)evt.Data)[ApcmDevcieConfig.PROPERTY_P4_PLCINFO], currentP4PlcInfo.GetType());
            //更新本地缓存
            Set($"{ApcmKeyConf.DataCollection}:{ApcmDevcieConfig.PROPERTY_P4_PLCINFO}:DATA", currentP4PlcInfo);
            //4号位需要更新
            Write(ApcmDevcieConfig.ACTION_P4_UPDATE, 1);
            //更新具体信息
            RfidStationInfo stationInfo = RefreshMaterialCode(currentP4PlcInfo);
            int Spe = Get<int>($"{ApcmKeyConf.DataCollection}:{ApcmDevcieConfig.ACTION_P4_WORKBINSPE}:DATA");
            if (Spe == 0)
            {
                Write(ApcmDevcieConfig.ACTION_P4_UPDATECPPINFO, StructTransform.StructToBytes(stationInfo));
            }
            else
            {
                Write(ApcmDevcieConfig.ACTION_P4_SPECPPINFO, StructTransform.StructToBytes(stationInfo));
            }
        }
        public void OnEvent_P4_UPDATECPPINFO(ApcmEventArgs evt)
        {
            Write(ApcmDevcieConfig.ACTION_P4_REQPLCRX, 1);
        }

        public void OnEvent_P4_SPECPPINFO(ApcmEventArgs evt)
        {
            Write(ApcmDevcieConfig.ACTION_P4_REQPLCUPDATE_SPE, 1);
        }
        public void OnEvent_P4_PlcRxDone(ApcmEventArgs evt)
        {
            Write(ApcmDevcieConfig.ACTION_P4_REQPLCUPDATE, 1);
        }

        public void OnEvent_P4_PlcUpdateDone(ApcmEventArgs evt)
        {
            //更新料箱明细表
            UpdateMesBtaryInfos();
        }

        public void OnEvent_P4_PlcRxDone_Spe(ApcmEventArgs evt)
        {
            Read(ApcmDevcieConfig.PROPERTY_P4_PLCINFO);
        }

        public void OnEvent_P4_PlcUpdateDone_Spe(ApcmEventArgs evt)
        { 
            //更新料箱明细表
            UpdateMesBtaryInfos();
        }

        private void UpdateMesBtaryInfos()
        {
            //TODO获取当前4号位料箱明细信息；
            P4PlcInfo currentP4PlcInfo = Get<P4PlcInfo>($"{ApcmKeyConf.DataCollection}:{ApcmDevcieConfig.PROPERTY_P4_PLCINFO}:DATA");
            //更新料箱明细表
            byte[] bts = currentP4PlcInfo.LocaNumbInfo;
            //获取料箱内件数
            int length = 0;
            char[] LocaNumbInfob2 = DataConvertHelper.BytesToCharArrayb2(bts);
            foreach(char c in LocaNumbInfob2)
            {
                if (int.Parse(c.ToString()) == 1)
                {
                    length++;
                }
            }
            for (int i=0;i< length; i++)
            {
                PushToMes(ApcmDevcieConfig.EVENT_MES_BTRAY_INFO, new Dictionary<string, object>
                {
                    ["BTRAY_ID"] = currentP4PlcInfo.UID,
                    ["MATERIAL_CODE"] = currentP4PlcInfo.MaterialCode,
                    ["PRODUCT_SN"] = currentP4PlcInfo.ProductSnL[i],
                    ["SEQ_NO"] = i,
                    ["FLAG"] = 1,
                });
            }
        }

        public void OnEvent_P4_WorkbinSpeOff(ApcmEventArgs evt)
        {
            //更新本地缓存
            Set($"{ApcmKeyConf.DataCollection}:{ApcmDevcieConfig.ACTION_P4_WORKBINSPE}:DATA", 0);
        }


        private RfidStationInfo RefreshMaterialCode(P4PlcInfo p4PlcInfo)
        {
            //获取订单的产品物料号
            string MaterialCode = Get<string>($"LD:{SysConf.KeyAssemblyLine}:MCIM:MATERIAL_CODE");
            p4PlcInfo.MaterialCode = MaterialCode;
            //更新本地缓存
            Set($"{ApcmKeyConf.DataCollection}:{ApcmDevcieConfig.PROPERTY_P4_PLCINFO}:DATA", p4PlcInfo);
            RfidStationInfo stationInfo = new RfidStationInfo();
            stationInfo.AssemblyLineId = p4PlcInfo.AssemblyLineId;
            stationInfo.UID = p4PlcInfo.UID;
            stationInfo.MaterialCode = p4PlcInfo.MaterialCode;
            stationInfo.BatchCode = p4PlcInfo.BatchCode;
            stationInfo.LocaNumbInfo = p4PlcInfo.LocaNumbInfo;
            stationInfo.McToMes = p4PlcInfo.McToMes;
            stationInfo.MesToMc = p4PlcInfo.MesToMc;
            stationInfo.NgStatus = p4PlcInfo.NgStatus;
            stationInfo.ProductSnL = p4PlcInfo.ProductSnL;
            stationInfo.Status = p4PlcInfo.Status;
            return stationInfo;
        }
        #endregion
    }
}
