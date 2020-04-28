using KHBC.Core;

namespace KHBC.LD.APCM.Workflow
{
	public partial class ApcmDevcieConfig
	{
		#region 事件和属性
        /// <summary>
        /// bP1SnPhLeft: 料箱到达一号位, Address=%MB2904.6, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_BP1SNPHLEFT = "bP1SnPhLeft";
        /// <summary>
        /// p1_AgvCall: 正常呼叫AGV, Address=%MB15000.0, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_P1_AGVCALL = "p1_AgvCall";
        /// <summary>
        /// p1_bAgvChange: 上报SCADA换箱操作, Address=%MB15000.1, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_P1_BAGVCHANGE = "p1_bAgvChange";
        /// <summary>
        /// p1_bTerOrderTaskCall: 订单冻结下料任务, Address=%MB15000.2, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_P1_BTERORDERTASKCALL = "p1_bTerOrderTaskCall";
        /// <summary>
        /// p1_bNullTaskDone: 空箱任务执行完成, Address=%MB15000.3, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_P1_BNULLTASKDONE = "p1_bNullTaskDone";
        /// <summary>
        /// p1_bEnReadInfo: 允许读箱体信息, Address=%MB15000.4, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_P1_BENREADINFO = "p1_bEnReadInfo";
        /// <summary>
        /// p1_info: 1号位置料箱信息, Address=%MB15020, Type=BYTE, Length=3231
        /// </summary>
        public const string PROPERTY_P1_INFO = "p1_info";
        /// <summary>
        /// bP2SnPh: 料箱到达二号位, Address=%MB2906.0, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_BP2SNPH = "bP2SnPh";
        /// <summary>
        /// p2_WorkBinInfo: 2号位一次性料箱初始任务数据, Address=%MB18251, Type=BYTE, Length=1077
        /// </summary>
        public const string PROPERTY_P2_WORKBININFO = "p2_WorkBinInfo";
        /// <summary>
        /// p2_NumInfo: 2号位置取几片, Address=%MB15001, Type=BYTE, Length=1
        /// </summary>
        public const string EVENT_P2_NUMINFO = "p2_NumInfo";
        /// <summary>
        /// p3_NgNullAgvCall: NG空箱任务呼叫, Address=%MB15002.0, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_P3_NGNULLAGVCALL = "p3_NgNullAgvCall";
        /// <summary>
        /// p4_WorkbinSpeOff: 特殊放行关闭指令, Address=%MB15003.0, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_P4_WORKBINSPEOFF = "p4_WorkbinSpeOff";
        /// <summary>
        /// P4_PlcRxDone: 4号位读取4065字节完成, Address=%MB15003.1, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_P4_PLCRXDONE = "P4_PlcRxDone";
        /// <summary>
        /// P4_PlcUpdateDone: 4号位置更新0-1075数据完成, Address=%MB15003.2, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_P4_PLCUPDATEDONE = "P4_PlcUpdateDone";
        /// <summary>
        /// P4_PlcRxDone_Spe: 4号位读取4065字节完成(特殊放行）, Address=%MB15003.3, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_P4_PLCRXDONE_SPE = "P4_PlcRxDone_Spe";
        /// <summary>
        /// P4_PlcUpdateDone_Spe: 4号位置更新0-1075数据完成(特殊放行）, Address=%MB15003.4, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_P4_PLCUPDATEDONE_SPE = "P4_PlcUpdateDone_Spe";
        /// <summary>
        /// p4_ReqPcRead: 允许读4号位置RFID所有数据, Address=%MB15003.5, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_P4_REQPCREAD = "p4_ReqPcRead";
        /// <summary>
        /// P4_NumInfo: 4号位置数量, Address=%MB15004, Type=BYTE, Length=1
        /// </summary>
        public const string PROPERTY_P4_NUMINFO = "P4_NumInfo";
        /// <summary>
        /// p4_PlcInfo: 4号位RFID所有数据, Address=%MB19328, Type=BYTE, Length=7000
        /// </summary>
        public const string PROPERTY_P4_PLCINFO = "p4_PlcInfo";
        /// <summary>
        /// p5_bEnScadaReadSpe: 允许SCADA读单个箱体信息，用于辨别是否要特殊放行, Address=%MB15005.0, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_P5_BENSCADAREADSPE = "p5_bEnScadaReadSpe";
        /// <summary>
        /// p5_SpeTaskDone: 特殊放行任务完成, Address=%MB15005.1, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_P5_SPETASKDONE = "p5_SpeTaskDone";
        /// <summary>
        /// p5_bEnScadaRead: 允许SCADA读所有箱体数据用于生成批次号, Address=%MB15005.2, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_P5_BENSCADAREAD = "p5_bEnScadaRead";
        /// <summary>
        /// p5_CentInfo: 给新的批次号下发, Address=%MB32158, Type=BYTE, Length=900
        /// </summary>
        public const string ACTION_P5_CENTINFO = "p5_CentInfo";
        /// <summary>
        /// SpeTaskEnd: 尾单特殊放行完成, Address=%MB15006.0, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_SPETASKEND = "SpeTaskEnd";
        /// <summary>
        /// Line_OpEnRead1: 线体中间允许读OP加工位置, Address=%MB15007, Type=BYTE, Length=2
        /// </summary>
        public const string PROPERTY_LINE_OPENREAD1 = "Line_OpEnRead1";
        /// <summary>
        /// Line_OpEnRead2: 线体中间允许读OP备用位, Address=%MB15009, Type=BYTE, Length=1
        /// </summary>
        public const string PROPERTY_LINE_OPENREAD2 = "Line_OpEnRead2";
        /// <summary>
        /// Line_OpInfo1: 备用位置序列号, Address=%MB27300, Type=BYTE, Length=216
        /// </summary>
        public const string PROPERTY_LINE_OPINFO1 = "Line_OpInfo1";
        /// <summary>
        /// Line_OpInfo2: 备用位置序列号, Address=%MB27516, Type=BYTE, Length=216
        /// </summary>
        public const string PROPERTY_LINE_OPINFO2 = "Line_OpInfo2";
        /// <summary>
        /// SpotCheckDone: 抽检任务已经产生, Address=%MB15010.0, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_SPOTCHECKDONE = "SpotCheckDone";
        /// <summary>
        /// SpotCall: 呼叫人工取抽检料, Address=%MB15010.1, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_SPOTCALL = "SpotCall";
        /// <summary>
        /// SpotInfo: 当前被抽检的序列号, Address=%MB27732, Type=BYTE, Length=18
        /// </summary>
        public const string PROPERTY_SPOTINFO = "SpotInfo";
        /// <summary>
        /// NgNum: NG1呼叫人工, Address=%MB27750, Type=BYTE, Length=3
        /// </summary>
        public const string PROPERTY_NGNUM = "NgNum";
        /// <summary>
        /// Thrum_RfidCp[1]: 线头上料RFID编码块监控, Address=%MB27753[0].0, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_THRUM_RFIDCP_1 = "Thrum_RfidCp[1]";
        /// <summary>
        /// Thrum_RfidCp[2]: 线头上料RFID编码块监控, Address=%MB27753[0].1, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_THRUM_RFIDCP_2 = "Thrum_RfidCp[2]";
        /// <summary>
        /// Thrum_RfidCp[3]: 线头上料RFID编码块监控, Address=%MB27753[0].2, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_THRUM_RFIDCP_3 = "Thrum_RfidCp[3]";
        /// <summary>
        /// Thrum_RfidCp[4]: 线头上料RFID编码块监控, Address=%MB27753[0].3, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_THRUM_RFIDCP_4 = "Thrum_RfidCp[4]";
        /// <summary>
        /// Hl_RfidCp: 线头发料RFID编码块监控, Address=%MB27753[0].4, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_HL_RFIDCP = "Hl_RfidCp";
        /// <summary>
        /// Cnc_RfidCp[1,1]: 线体中间上料RFID编码块监控[1,1], Address=%MB27753[0].5, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_1_1 = "Cnc_RfidCp[1,1]";
        /// <summary>
        /// Cnc_RfidCp[1,2]: 线体中间上料RFID编码块监控[1,2], Address=%MB27753[0].6, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_1_2 = "Cnc_RfidCp[1,2]";
        /// <summary>
        /// Cnc_RfidCp[1,3]: 线体中间上料RFID编码块监控[1,3], Address=%MB27753[0].7, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_1_3 = "Cnc_RfidCp[1,3]";
        /// <summary>
        /// Cnc_RfidCp[2,1]: 线体中间上料RFID编码块监控[2,1], Address=%MB27753[1].0, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_2_1 = "Cnc_RfidCp[2,1]";
        /// <summary>
        /// Cnc_RfidCp[2,2]: 线体中间上料RFID编码块监控[2,2], Address=%MB27753[1].1, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_2_2 = "Cnc_RfidCp[2,2]";
        /// <summary>
        /// Cnc_RfidCp[2,3]: 线体中间上料RFID编码块监控[2,3], Address=%MB27753[1].2, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_2_3 = "Cnc_RfidCp[2,3]";
        /// <summary>
        /// Cnc_RfidCp[3,1]: 线体中间上料RFID编码块监控[3,1], Address=%MB27753[1].3, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_3_1 = "Cnc_RfidCp[3,1]";
        /// <summary>
        /// Cnc_RfidCp[3,2]: 线体中间上料RFID编码块监控[3,2], Address=%MB27753[1].4, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_3_2 = "Cnc_RfidCp[3,2]";
        /// <summary>
        /// Cnc_RfidCp[3,3]: 线体中间上料RFID编码块监控[3,3], Address=%MB27753[1].5, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_3_3 = "Cnc_RfidCp[3,3]";
        /// <summary>
        /// Cnc_RfidCp[4,1]: 线体中间上料RFID编码块监控[4,1], Address=%MB27753[1].6, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_4_1 = "Cnc_RfidCp[4,1]";
        /// <summary>
        /// Cnc_RfidCp[4,2]: 线体中间上料RFID编码块监控[4,2], Address=%MB27753[1].7, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_4_2 = "Cnc_RfidCp[4,2]";
        /// <summary>
        /// Cnc_RfidCp[4,3]: 线体中间上料RFID编码块监控[4,3], Address=%MB27753[2].0, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_4_3 = "Cnc_RfidCp[4,3]";
        /// <summary>
        /// Cnc_RfidCp[5,1]: 线体中间上料RFID编码块监控[5,1], Address=%MB27753[2].1, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_5_1 = "Cnc_RfidCp[5,1]";
        /// <summary>
        /// Cnc_RfidCp[5,2]: 线体中间上料RFID编码块监控[5,2], Address=%MB27753[2].2, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_5_2 = "Cnc_RfidCp[5,2]";
        /// <summary>
        /// Cnc_RfidCp[5,3]: 线体中间上料RFID编码块监控[5,3], Address=%MB27753[2].3, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_5_3 = "Cnc_RfidCp[5,3]";
        /// <summary>
        /// Cnc_RfidCp[6,1]: 线体中间上料RFID编码块监控[6,1], Address=%MB27753[2].4, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_6_1 = "Cnc_RfidCp[6,1]";
        /// <summary>
        /// Cnc_RfidCp[6,2]: 线体中间上料RFID编码块监控[6,2], Address=%MB27753[2].5, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_6_2 = "Cnc_RfidCp[6,2]";
        /// <summary>
        /// Cnc_RfidCp[6,3]: 线体中间上料RFID编码块监控[6,3], Address=%MB27753[2].6, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_6_3 = "Cnc_RfidCp[6,3]";
        /// <summary>
        /// Cnc_RfidCp[7,1]: 线体中间上料RFID编码块监控[7,1], Address=%MB27753[2].7, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_7_1 = "Cnc_RfidCp[7,1]";
        /// <summary>
        /// Cnc_RfidCp[7,2]: 线体中间上料RFID编码块监控[7,2], Address=%MB27753[3].0, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_7_2 = "Cnc_RfidCp[7,2]";
        /// <summary>
        /// Cnc_RfidCp[7,3]: 线体中间上料RFID编码块监控[7,3], Address=%MB27753[3].1, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_7_3 = "Cnc_RfidCp[7,3]";
        /// <summary>
        /// Cnc_RfidCp[8,1]: 线体中间上料RFID编码块监控[8,1], Address=%MB27753[3].2, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_8_1 = "Cnc_RfidCp[8,1]";
        /// <summary>
        /// Cnc_RfidCp[8,2]: 线体中间上料RFID编码块监控[8,2], Address=%MB27753[3].3, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_8_2 = "Cnc_RfidCp[8,2]";
        /// <summary>
        /// Cnc_RfidCp[8,3]: 线体中间上料RFID编码块监控[8,3], Address=%MB27753[3].4, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_8_3 = "Cnc_RfidCp[8,3]";
        /// <summary>
        /// Cnc_RfidCp[9,1]: 线体中间上料RFID编码块监控[9,1], Address=%MB27753[3].5, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_9_1 = "Cnc_RfidCp[9,1]";
        /// <summary>
        /// Cnc_RfidCp[9,2]: 线体中间上料RFID编码块监控[9,2], Address=%MB27753[3].6, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_9_2 = "Cnc_RfidCp[9,2]";
        /// <summary>
        /// Cnc_RfidCp[9,3]: 线体中间上料RFID编码块监控[9,3], Address=%MB27753[3].7, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_9_3 = "Cnc_RfidCp[9,3]";
        /// <summary>
        /// Cnc_RfidCp[10,1]: 线体中间上料RFID编码块监控[10,1], Address=%MB27753[4].0, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_10_1 = "Cnc_RfidCp[10,1]";
        /// <summary>
        /// Cnc_RfidCp[10,2]: 线体中间上料RFID编码块监控[10,2], Address=%MB27753[4].1, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_10_2 = "Cnc_RfidCp[10,2]";
        /// <summary>
        /// Cnc_RfidCp[10,3]: 线体中间上料RFID编码块监控[10,3], Address=%MB27753[4].2, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_10_3 = "Cnc_RfidCp[10,3]";
        /// <summary>
        /// Cnc_RfidCp[11,1]: 线体中间上料RFID编码块监控[11,1], Address=%MB27753[4].3, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_11_1 = "Cnc_RfidCp[11,1]";
        /// <summary>
        /// Cnc_RfidCp[11,2]: 线体中间上料RFID编码块监控[11,2], Address=%MB27753[4].4, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_11_2 = "Cnc_RfidCp[11,2]";
        /// <summary>
        /// Cnc_RfidCp[11,3]: 线体中间上料RFID编码块监控[11,3], Address=%MB27753[4].5, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_11_3 = "Cnc_RfidCp[11,3]";
        /// <summary>
        /// Cnc_RfidCp[12,1]: 线体中间上料RFID编码块监控[12,1], Address=%MB27753[4].6, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_12_1 = "Cnc_RfidCp[12,1]";
        /// <summary>
        /// Cnc_RfidCp[12,2]: 线体中间上料RFID编码块监控[12,2], Address=%MB27753[4].7, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_12_2 = "Cnc_RfidCp[12,2]";
        /// <summary>
        /// Cnc_RfidCp[12,3]: 线体中间上料RFID编码块监控[12,3], Address=%MB27753[5].0, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_CNC_RFIDCP_12_3 = "Cnc_RfidCp[12,3]";
        /// <summary>
        /// El_RfidCp[1]: 线尾巴上料RFID编码块监控1, Address=%MB27753[5].1, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_EL_RFIDCP_1 = "El_RfidCp[1]";
        /// <summary>
        /// El_RfidCp[2]: 线尾巴上料RFID编码块监控2, Address=%MB27753[5].2, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_EL_RFIDCP_2 = "El_RfidCp[2]";
        /// <summary>
        /// El_RfidCp[3]: 线尾巴上料RFID编码块监控3, Address=%MB27753[5].3, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_EL_RFIDCP_3 = "El_RfidCp[3]";
        /// <summary>
        /// El_RfidCp[4]: 线尾巴上料RFID编码块监控4, Address=%MB27753[5].4, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_EL_RFIDCP_4 = "El_RfidCp[4]";
        /// <summary>
        /// El_RfidCp[5]: 线尾巴上料RFID编码块监控5, Address=%MB27753[5].5, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_EL_RFIDCP_5 = "El_RfidCp[5]";
        /// <summary>
        /// El_RfidCp[6]: 线尾巴上料RFID编码块监控6, Address=%MB27753[5].6, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_EL_RFIDCP_6 = "El_RfidCp[6]";
        /// <summary>
        /// El_RfidCp[7]: 线尾巴上料RFID编码块监控7, Address=%MB27753[5].7, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_EL_RFIDCP_7 = "El_RfidCp[7]";
        /// <summary>
        /// El_RfidCp[8]: 线尾巴上料RFID编码块监控8, Address=%MB27753[6].0, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_EL_RFIDCP_8 = "El_RfidCp[8]";
        /// <summary>
        /// El_RfidCp[9]: 线尾巴上料RFID编码块监控9, Address=%MB27753[6].1, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_EL_RFIDCP_9 = "El_RfidCp[9]";
        /// <summary>
        /// El_RfidCp[10]: 线尾巴上料RFID编码块监控10, Address=%MB27753[6].2, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_EL_RFIDCP_10 = "El_RfidCp[10]";
        /// <summary>
        /// Ng_RfidCp[1]: NG上料RFID编码块监控1, Address=%MB27753[6].3, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_NG_RFIDCP_1 = "Ng_RfidCp[1]";
        /// <summary>
        /// Ng_RfidCp[2]: NG上料RFID编码块监控2, Address=%MB27753[6].4, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_NG_RFIDCP_2 = "Ng_RfidCp[2]";
        /// <summary>
        /// Ng_RfidCp[3]: NG上料RFID编码块监控3, Address=%MB27753[6].5, Type=BIT, Length=1
        /// </summary>
        public const string EVENT_NG_RFIDCP_3 = "Ng_RfidCp[3]";
        /// <summary>
        /// p1_NullTask: 空箱任务, Address=%MB30000.0, Type=BIT, Length=1
        /// </summary>
        public const string ACTION_P1_NULLTASK = "p1_NullTask";
        /// <summary>
        /// p1_TerOrder: 空箱任务, Address=%MB30000.1, Type=BIT, Length=1
        /// </summary>
        public const string ACTION_P1_TERORDER = "p1_TerOrder";
        /// <summary>
        /// p4_Update: 4号位需要更新, Address=%MB30001[0].0, Type=BIT, Length=1
        /// </summary>
        public const string ACTION_P4_UPDATE = "p4_Update";
        /// <summary>
        /// p4_NoUpdate: 4号位不需要更新, Address=%MB30001[0].1, Type=BIT, Length=1
        /// </summary>
        public const string ACTION_P4_NOUPDATE = "p4_NoUpdate";
        /// <summary>
        /// p4_WorkbinSpe: 4号位特殊让走的指令(未放满放行）, Address=%MB30001[0].2, Type=BIT, Length=1
        /// </summary>
        public const string ACTION_P4_WORKBINSPE = "p4_WorkbinSpe";
        /// <summary>
        /// p4_ReqPlcRx: 请求PLC读取数据0-4065, Address=%MB30001[0].3, Type=BIT, Length=1
        /// </summary>
        public const string ACTION_P4_REQPLCRX = "p4_ReqPlcRx";
        /// <summary>
        /// p4_ReqPlcUpdate: 请求PLC更新物料号，线体号，MES-总控任务，总控-MES写任务，箱体状态, Address=%MB30001[0].4, Type=BIT, Length=1
        /// </summary>
        public const string ACTION_P4_REQPLCUPDATE = "p4_ReqPlcUpdate";
        /// <summary>
        /// p4_ReqPlcRx_Spe: 特殊放行请求PLC读取数据0-4065, Address=%MB30001[0].5, Type=BIT, Length=1
        /// </summary>
        public const string ACTION_P4_REQPLCRX_SPE = "p4_ReqPlcRx_Spe";
        /// <summary>
        /// p4_ReqPlcUpdate_Spe: 特殊放行请求PLC更新物料号，线体号，MES-总控任务，总控-MES写任务，箱体状态, Address=%MB30001[0].6, Type=BIT, Length=1
        /// </summary>
        public const string ACTION_P4_REQPLCUPDATE_SPE = "p4_ReqPlcUpdate_Spe";
        /// <summary>
        /// p4_UpdateCppInfo: 正常需要更新的具体信息字段, Address=%MB30003, Type=BYTE, Length=1077
        /// </summary>
        public const string ACTION_P4_UPDATECPPINFO = "p4_UpdateCppInfo";
        /// <summary>
        /// p4_SpeCppInfo: 特殊放行需要更新的具体信息字段, Address=%MB31080, Type=BYTE, Length=1077
        /// </summary>
        public const string ACTION_P4_SPECPPINFO = "p4_SpeCppInfo";
        /// <summary>
        /// p5_SpeTask: 5号位特殊放行任务, Address=%MB32157.0, Type=BIT, Length=1
        /// </summary>
        public const string ACTION_P5_SPETASK = "p5_SpeTask";
        /// <summary>
        /// p5_bSpeInfoReadDone: 5号位scada数据读取特殊放行完成, Address=%MB32157.1, Type=BIT, Length=1
        /// </summary>
        public const string ACTION_P5_BSPEINFOREADDONE = "p5_bSpeInfoReadDone";
        /// <summary>
        /// p5_bScadaWriteDone: 5号位SCADA传送数据完成, Address=%MB32157.2, Type=BIT, Length=1
        /// </summary>
        public const string ACTION_P5_BSCADAWRITEDONE = "p5_bScadaWriteDone";
        /// <summary>
        /// spotReqCheckTask: 请求产生抽检料, Address=%MB33058.0, Type=BIT, Length=1
        /// </summary>
        public const string ACTION_SPOTREQCHECKTASK = "spotReqCheckTask";
        /// <summary>
        /// endSpeTask: 尾巴特殊放行指令, Address=%MB33059.0, Type=BIT, Length=1
        /// </summary>
        public const string ACTION_ENDSPETASK = "endSpeTask";
        /// <summary>
        /// cncOp: 机台OP，需要实时更新，绝对不能出错, Address=%MB33060, Type=BYTE, Length=12
        /// </summary>
        public const string ACTION_CNCOP = "cncOp";

		#endregion
	}
}
