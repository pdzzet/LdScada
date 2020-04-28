using KHBC.Core;
using KHBC.Core.Device;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KHBC.LD.APCM.Workflow
{
    public class ApcmWorkflowP00 : ApcmWorkflowService
    {
        public override string ServiceName { get; set; }

        static Dictionary<int, bool> oPEnableRead1 = new Dictionary<int, bool>();
        static Dictionary<int, bool> oPEnableRead2 = new Dictionary<int, bool>();
        public ApcmWorkflowP00(DeviceInfo devInfo)
        {
            DevInfo = devInfo;
            ServiceName = $"{SysConf.ModuleName}:WF_P00";

            #region 产线事件
            RegEvent(ApcmDevcieConfig.EVENT_SPETASKEND, OnEvent_SpeTaskEnd);
            RegEvent(ApcmDevcieConfig.PROPERTY_LINE_OPENREAD1, OnEvent_Line_OpEnRead1Done);
            RegEvent(ApcmDevcieConfig.PROPERTY_LINE_OPENREAD2, OnEvent_Line_OpEnRead2Done);
            RegEvent(ApcmDevcieConfig.PROPERTY_LINE_OPINFO1, OnEvent_Line_OpInfo1ReadDone);
            RegEvent(ApcmDevcieConfig.PROPERTY_LINE_OPINFO2, OnEvent_Line_OpInfo2ReadDone);


            RegEvent(ApcmDevcieConfig.EVENT_SPOTCHECKDONE, OnEvent_SpotCheckDone);
            RegEvent(ApcmDevcieConfig.EVENT_SPOTCALL, OnEvent_SpotCall);
            RegEvent(ApcmDevcieConfig.PROPERTY_SPOTINFO, OnEvent_SpotInfoReadDone);
            RegEvent(ApcmDevcieConfig.PROPERTY_NGNUM, OnEvent_NgNumReadDone);
            #region 线头上料RFID编码块监控事件
            RegEvent(ApcmDevcieConfig.EVENT_THRUM_RFIDCP_1, OnEvent_Thrum_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_THRUM_RFIDCP_2, OnEvent_Thrum_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_THRUM_RFIDCP_3, OnEvent_Thrum_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_THRUM_RFIDCP_4, OnEvent_Thrum_RfidCp);
            #endregion

            RegEvent(ApcmDevcieConfig.EVENT_HL_RFIDCP, OnEvent_Hl_RfidCp);

            #region 线尾巴上料RFID编码块监控
            RegEvent(ApcmDevcieConfig.EVENT_EL_RFIDCP_1, OnEvent_El_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_EL_RFIDCP_2, OnEvent_El_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_EL_RFIDCP_3, OnEvent_El_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_EL_RFIDCP_4, OnEvent_El_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_EL_RFIDCP_5, OnEvent_El_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_EL_RFIDCP_6, OnEvent_El_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_EL_RFIDCP_7, OnEvent_El_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_EL_RFIDCP_8, OnEvent_El_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_EL_RFIDCP_9, OnEvent_El_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_EL_RFIDCP_10, OnEvent_El_RfidCp);
            #endregion

            #region NG上料RFID编码块监控
            RegEvent(ApcmDevcieConfig.EVENT_NG_RFIDCP_1, OnEvent_Ng_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_NG_RFIDCP_2, OnEvent_Ng_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_NG_RFIDCP_3, OnEvent_Ng_RfidCp);
            #endregion

            #endregion
        }

        
        #region 产线事件
        public void OnEvent_SpeTaskEnd(ApcmEventArgs evt)
        {
        }

        public void OnEvent_Line_OpEnRead1Done(ApcmEventArgs evt)
        {
            byte[] bts = (byte[])(((Dictionary<string, object>)evt.Data)[ApcmDevcieConfig.PROPERTY_LINE_OPENREAD1]);
            string bit00 = Convert.ToString(bts[0], 2);
            char[] chars00 = bit00.ToArray();
            int i = 0;
            foreach (char c in chars00)
            {
                i++;
                oPEnableRead1.Add(i, bool.Parse(c.ToString()));
            }
            string bit01 = Convert.ToString(bts[1], 2);
            char[] chars01 = bit01.ToArray();
            foreach (char c in chars01)
            {
                i++;
                oPEnableRead1.Add(i, bool.Parse(c.ToString()));
            }
        }
        public void OnEvent_Line_OpEnRead2Done(ApcmEventArgs evt)
        {
            byte[] bts = (byte[])(((Dictionary<string, object>)evt.Data)[ApcmDevcieConfig.PROPERTY_LINE_OPENREAD2]);
            string bit00 = Convert.ToString(bts[0], 2);
            char[] chars00 = bit00.ToArray();
            int i = 0;
            foreach (char c in chars00)
            {
                i++;
                oPEnableRead2.Add(i, bool.Parse(c.ToString()));
            }
        }
        public void OnEvent_Line_OpInfo1ReadDone(ApcmEventArgs evt)
        {

        }
        public void OnEvent_Line_OpInfo2ReadDone(ApcmEventArgs evt)
        {
        }
        public void OnEvent_SpotCheckDone(ApcmEventArgs evt)
        {
        }
        public void OnEvent_SpotCall(ApcmEventArgs evt)
        {
        }
        public void OnEvent_NgNumReadDone(ApcmEventArgs evt)
        {
            int NgNum = int.Parse(((Dictionary<string, object>)evt.Data)[ApcmDevcieConfig.PROPERTY_NGNUM].ToString());
            //int Package = Get<int>($"LD:{SysConf.KeyAssemblyLine}:MCIM:PACKAGE");
            //if (NgNum >= Package)
            //{
            //    //触发设置空箱任务呼叫信号
            //    Set(ApcmDevcieConfig.EVENT_P3_NGNULLAGVCALL, 1);
            //}
        }
        public void OnEvent_SpotInfoReadDone(ApcmEventArgs evt)
        {
        }
        public void OnEvent_Thrum_RfidCp(ApcmEventArgs evt)
        {
        }
        public void OnEvent_Hl_RfidCp(ApcmEventArgs evt)
        {
        }

        public void OnEvent_El_RfidCp(ApcmEventArgs evt)
        {
        }
        public void OnEvent_Ng_RfidCp(ApcmEventArgs evt)
        {
        }

        #endregion
    }
}
