using KHBC.Core;
using KHBC.Core.Device;
using KHBC.LD.APCM.BEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace KHBC.LD.APCM.Workflow
{
    public class ApcmWorkflowP02 : ApcmWorkflowService
    {
        public ApcmWorkflowP02(DeviceInfo devInfo)
        {
            DevInfo = devInfo;
            ServiceName = $"{SysConf.ModuleName}:WF_P02";

            #region 二号位事件
            RegEvent(ApcmDevcieConfig.EVENT_BP2SNPH, OnEvent_P2_In);
            RegEvent(ApcmDevcieConfig.PROPERTY_P2_WORKBININFO, OnEvent_P2_WorkbinInfoReadDone);
            RegEvent(ApcmDevcieConfig.EVENT_P2_NUMINFO, OnEvent_P2_NumInfo);
            #endregion
        }

        #region 二号位事件
        public void OnEvent_P2_In(ApcmEventArgs evt)
        {
            Read(ApcmDevcieConfig.PROPERTY_P2_WORKBININFO);
        }

        public void OnEvent_P2_WorkbinInfoReadDone(ApcmEventArgs evt)
        {
            RfidStationInfo rfidStationInfo = new RfidStationInfo();
            rfidStationInfo = (RfidStationInfo)(((Dictionary<string, object>)evt.Data)[ApcmDevcieConfig.PROPERTY_P2_WORKBININFO]);
            Set($"{ApcmKeyConf.DataCollection}:{ApcmDevcieConfig.PROPERTY_P2_WORKBININFO}:DATA", rfidStationInfo);
            TriggerEvent(ApcmDevcieConfig.EVENT_CUSTOM_UPDATE_P1_INFO, evt);
        }

        public void OnEvent_P2_NumInfo(ApcmEventArgs evt)
        {
            //查看2号料位是否有料箱
            RfidStationInfo rfidStationInfo = Get<RfidStationInfo>($"{ApcmKeyConf.DataCollection}:{ApcmDevcieConfig.PROPERTY_P2_WORKBININFO}:DATA");
            if (rfidStationInfo.LocaNumbInfo == null)
            {
                return;
            }
            //TODO 1.读取2号位取到第几片
            int p2Num = int.Parse(((Dictionary<string, object>)evt.Data)[ApcmDevcieConfig.EVENT_P2_NUMINFO].ToString());
            //TODO 2.更新2号料表信息
            int btsIndex = p2Num / 8;
            int btIndex = p2Num % 8 - 1;
            string btstr = Convert.ToString(rfidStationInfo.LocaNumbInfo[btsIndex], 2);
            char[] chars = btstr.ToCharArray();
            for (int i = 0; i < 8; i++)
            {
                chars[btIndex] = '0';
            }
            btstr = chars.ToString();
            byte tarbt = Convert.ToByte(btstr, 2);
            rfidStationInfo.LocaNumbInfo[btsIndex] = tarbt;
            Write(ApcmDevcieConfig.PROPERTY_P2_WORKBININFO, Encoding.ASCII.GetBytes(rfidStationInfo.ToJsonStr()));
            //存放当前取第几片料
            Set($"{ApcmKeyConf.DataCollection}:{ApcmDevcieConfig.EVENT_P2_NUMINFO}:DATA", p2Num);
        }
        #endregion
    }
}
