using KHBC.Core.BaseModels;
using KHBC.Core;
using KHBC.Core.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHBC.LD.APCM.Workflow
{
    public class ApcmWorkflowCNC : ApcmWorkflowService
    {
        public override string ServiceName { get; set; }

        public ApcmWorkflowCNC(DeviceInfo devInfo)
        {
            DevInfo = devInfo;
            ServiceName = $"{SysConf.ModuleName}:WF_CNC";

            #region 产线事件
            #region 线体中间上料RFID编码块监控
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_1_1, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_1_2, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_1_3, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_2_1, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_2_2, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_2_3, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_3_1, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_3_2, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_3_3, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_4_1, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_4_2, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_4_3, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_5_1, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_5_2, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_5_3, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_6_1, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_6_2, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_6_3, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_7_1, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_7_2, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_7_3, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_8_1, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_8_2, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_8_3, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_9_1, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_9_2, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_9_3, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_10_1, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_10_2, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_10_3, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_11_1, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_11_2, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_11_3, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_12_1, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_12_2, OnEvent_Cnc_RfidCp);
            RegEvent(ApcmDevcieConfig.EVENT_CNC_RFIDCP_12_3, OnEvent_Cnc_RfidCp);
            #endregion
            #endregion
        }

        #region 产线事件

        #region 线体中间上料RFID编码块监控
        public void OnEvent_Cnc_RfidCp(ApcmEventArgs evt)
        {
        }
        #endregion
        #endregion
    }
}
