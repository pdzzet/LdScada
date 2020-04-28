using KHBC.Core.Device;
using KHBC.Core.Log;
using System;
using System.Collections.Generic;
using static KHBC.LD.FCCM.DeviceApi.Focas1;

namespace KHBC.LD.FCCM
{
    public class FccmDeviceApi : IDevice
    {
        public  bool ConnectStatus { get; set; } = false;
        public bool DataChange { get; set; } = false;
        public string Message { get; set; }
        public int ErrorCode { get; set; } = 0;

        private readonly string _serviceName;
        public  ushort _hander;
        private readonly string _ipAddress;
        private readonly UInt16 _port;

        public FccmDeviceApi(string serviceName, string ipAddress, int port)
        {
            _serviceName = serviceName;
            _ipAddress = ipAddress;
            _port = Convert.ToUInt16(port);
        }

        ~FccmDeviceApi()
        {
            Disconnect();
        }

        public void Connect()
        {
            var timeout = 3000;
            Disconnect();
            var ret = cnc_allclibhndl3(_ipAddress, _port, timeout, out _hander);
            
            if (ret== 0)
            {

                Message = $"连接设备成功: {_ipAddress}:{_port}, timeout={timeout}";
                Logger.Main.Info($"[{_serviceName}{Message}");
                ConnectStatus = true;
            }
            else
            {
                Message = $"连接设备失败: {_ipAddress}:{_port}, timeout={timeout}";
                Logger.Main.Error($"[{_serviceName}{Message}");
                ErrorCode = ret;
            }
        }

        public void Disconnect()
        {
            if (_hander > 0)
            {
                cnc_freelibhndl(_hander);
                _hander = 0;
            }
        }

        private void ErrorLog(int err, string msg)
        {
            Logger.Main.Error($"[{_serviceName}: err={err}, {msg}");
        }

        private void SaveResult(ref Dictionary<string, object> dataInfo, int ret, string name, object value)
        {
            if (ret == 0)
            {
                dataInfo[name] = value;
            }
            else
            {
                ErrorLog(ret, name);
            }
        }


        public void GetCncCtrlAxisSpindleInfo1(ref Dictionary<string, object> dataInfo)
        {
            /* read actual axis feedrate(F) */
            var cnc_actf_data = new ODBACT();
            var ret = cnc_actf(_hander, cnc_actf_data);
            SaveResult(ref dataInfo, ret, "cnc_actf", cnc_actf_data);

            /* read absolute axis position */
            var cnc_absolute_data = new ODBAXIS();
            ret = cnc_absolute(_hander, ALL_AXES, 16, cnc_absolute_data);
            SaveResult(ref dataInfo, ret, "cnc_absolute", cnc_absolute_data);

            /* read machine axis position */
            var cnc_machine_data = new ODBAXIS();
            ret = cnc_machine(_hander, ALL_AXES, 16, cnc_machine_data);
            SaveResult(ref dataInfo, ret, "cnc_machine", cnc_machine_data);

            /* read relative axis position */
            var cnc_relative_data = new ODBAXIS();
            ret = cnc_relative(_hander, ALL_AXES, 16, cnc_relative_data);
            SaveResult(ref dataInfo, ret, "cnc_relative", cnc_relative_data);

            /* read distance to go */
            var cnc_distance_data = new ODBAXIS();
            ret = cnc_distance(_hander, ALL_AXES, 16, cnc_distance_data);
            SaveResult(ref dataInfo, ret, "cnc_distance", cnc_distance_data);

            /* read skip position */
            var cnc_skip_data = new ODBAXIS();
            ret = cnc_skip(_hander, ALL_AXES, 16, cnc_skip_data);
            SaveResult(ref dataInfo, ret, "cnc_skip", cnc_skip_data);

            /* read servo delay value */
            var cnc_srvdelay_data = new ODBAXIS();
            ret = cnc_srvdelay(_hander, ALL_AXES, 16, cnc_srvdelay_data);
            SaveResult(ref dataInfo, ret, "cnc_srvdelay", cnc_srvdelay_data);

            /* read acceleration/deceleration delay value */
            var cnc_accdecdly_data = new ODBAXIS();
            ret = cnc_accdecdly(_hander, ALL_AXES, 16, cnc_accdecdly_data);
            SaveResult(ref dataInfo, ret, "cnc_accdecdly", cnc_accdecdly_data);

            /* read acceleration/deceleration delay value */
            var cnc_absolute2_data = new ODBAXIS();
            ret = cnc_absolute2(_hander, ALL_AXES, 16, cnc_absolute2_data);
            SaveResult(ref dataInfo, ret, "cnc_absolute2", cnc_absolute2_data);

            /* read acceleration/deceleration delay value */
            var cnc_relative2_data = new ODBAXIS();
            ret = cnc_relative2(_hander, ALL_AXES, 16, cnc_relative2_data);
            SaveResult(ref dataInfo, ret, "cnc_relative2", cnc_relative2_data);

            ret = cnc_rdloopgain(_hander, out int cnc_rdloopgain_data);
            SaveResult(ref dataInfo, ret, "cnc_rdloopgain", cnc_rdloopgain_data);

            ret = cnc_rdcurrent(_hander, out short cnc_rdcurrent_data);
            SaveResult(ref dataInfo, ret, "cnc_rdcurrent", cnc_rdcurrent_data);

            ret = cnc_rdsrvspeed(_hander, out int cnc_rdsrvspeed_data);
            SaveResult(ref dataInfo, ret, "cnc_rdsrvspeed", cnc_rdsrvspeed_data);

            ret = cnc_rdopmode(_hander, out short cnc_rdopmode_data);
            SaveResult(ref dataInfo, ret, "cnc_rdopmode", cnc_rdopmode_data);

            ret = cnc_rdposerrs(_hander, out int cnc_rdposerrs_data);
            SaveResult(ref dataInfo, ret, "cnc_rdposerrs", cnc_rdposerrs_data);

            var cnc_rdposerrs2_data = new ODBPSER();
            ret = cnc_rdposerrs2(_hander, cnc_rdposerrs2_data);
            SaveResult(ref dataInfo, ret, "cnc_rdposerrs2", cnc_rdposerrs2_data);

            ret = cnc_rdposerrz(_hander, out int cnc_rdposerrz_data);
            SaveResult(ref dataInfo, ret, "cnc_rdposerrz", cnc_rdposerrz_data);

            ret = cnc_rdsynerrsy(_hander, out int cnc_rdsynerrsy_data);
            SaveResult(ref dataInfo, ret, "cnc_rdsynerrsy", cnc_rdsynerrsy_data);

            ret = cnc_rdsynerrrg(_hander, out int cnc_rdsynerrrg_data);
            SaveResult(ref dataInfo, ret, "cnc_rdsynerrrg", cnc_rdsynerrrg_data);

            var cnc_rdspdlalm_data = new Object();
            ret = cnc_rdspdlalm(_hander, cnc_rdspdlalm_data);
            SaveResult(ref dataInfo, ret, "cnc_rdspdlalm", cnc_rdspdlalm_data);

            var cnc_rdctrldi_data = new ODBSPDI();
            ret = cnc_rdctrldi(_hander, cnc_rdctrldi_data);
            SaveResult(ref dataInfo, ret, "cnc_rdctrldi", cnc_rdctrldi_data);

            var cnc_rdctrldo_data = new ODBSPDO();
            ret = cnc_rdctrldo(_hander, cnc_rdctrldo_data);
            SaveResult(ref dataInfo, ret, "cnc_rdctrldo", cnc_rdctrldo_data);

            ret = cnc_rdnspdl(_hander, out short cnc_rdnspdl_data);
            SaveResult(ref dataInfo, ret, "cnc_rdnspdl", cnc_rdnspdl_data);

            var cnc_rdwaveprm_data = new IODBWAVE();
            ret = cnc_rdwaveprm(_hander, cnc_rdwaveprm_data);
            SaveResult(ref dataInfo, ret, "cnc_rdwaveprm", cnc_rdwaveprm_data);

            var cnc_rdwaveprm2_data = new IODBWVPRM();
            ret = cnc_rdwaveprm2(_hander, cnc_rdwaveprm2_data);
            SaveResult(ref dataInfo, ret, "cnc_rdwaveprm2", cnc_rdwaveprm2_data);
        }
       
        /// <summary>
        /// 获取CNC相关信息
        /// </summary>
        /// <param name="dataInfo">输出: 相关信息字典</param>
        /// <returns>true: 成功, false: 失败</returns>
        public void GetCncDeviceInfo(ref Dictionary<string, object> dataInfo)
        {

            ODBDY2_1 cnc_rddynamic2_data = new ODBDY2_1();
            
            #region CNC动态数据
            // |--------------------------------------------------------------|----------------------- |
            // |                           数据                               | 单独实现对应功能的函数 |
            // |--------------------------------------------------------------|----------------------- |
            // |报警状态（Alarm status）                                      | cnc_alarm2             |
            // |正在运行的程序号（Program number in executing）               | cnc_rdprgnum           |
            // |主程序的程序号（Program number of the main program）          | cnc_rdprgnum           |
            // |正在运行的NC程序的序号（Sequence number）                     | cnc_rdseqnum           |
            // |实际进给率（Actual feed rate）                                | cnc_actf               |
            // |主轴实际转速（Actual spindle speed）                          | cnc_acts               |
            // |绝对位置坐标（Absolute position data of controlled axis (2)） | cnc_absolute2          |
            // |机械坐标（Machine position data of controlled axis）          | cnc_machine            |
            // |相对位置坐标（Relative position data of controlled axis (2)） | cnc_relative2          |
            // |剩余移动量（Amount of distance to go of controlled axis）     | cnc_distance           |
            // |--------------------------------------------------------------|----------------------- |
            short retconn= cnc_allclibhndl3(_ipAddress, Convert.ToUInt16(_port), 3000, out _hander);
            //Tony 2020-3-31 test
            short ret = cnc_rddynamic2(_hander, 2, 28 + 4 * 4 * 1, cnc_rddynamic2_data);
            //ret = cnc_rddynamic2(_hander, 2, 28 + 4 * 4 * 1, cnc_rddynamic2_data);
            //ret = cnc_rddynamic2(_hander, ALL_AXES, 24 + 4 * 4 * MAX_AXIS, cnc_rddynamic2_data);
            SaveResult(ref dataInfo, ret, "cnc_rddynamic2", cnc_rddynamic2_data);
            #endregion

            #region 关于轴的信息
            // 1. Position value
            var odbaxdt1 = new ODBAXDT();
            short len1 = 3;
            ret = cnc_rdaxisdata(_hander, 1, new short[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 4, ref len1, odbaxdt1);
            SaveResult(ref dataInfo, ret, "cnc_rdaxisdata_type1", odbaxdt1);

            // 2. Servo   /Tony 2020-3-31 test -8
            var odbaxdt2 = new ODBAXDT();
            short len2 = 3;
            ret = cnc_rdaxisdata(_hander, 2, new short[] { 0, 1, 2 }, 3, ref len2, odbaxdt2);
            SaveResult(ref dataInfo, ret, "cnc_rdaxisdata_type2", odbaxdt2);
           
            // 3. Spindle  Tony 2020-3-31 test ret =4
            var odbaxdt3 = new ODBAXDT();
            short len3 = 3;
            ret = cnc_rdaxisdata(_hander, 3, new short[] { 0, 1, 2, 3, 4 }, 5, ref len3, odbaxdt3);
            SaveResult(ref dataInfo, ret, "cnc_rdaxisdata_type3", odbaxdt3);

            //4. Selected spindle
            var odbaxdt4 = new ODBAXDT();
            short len4 = 3;
            ret = cnc_rdaxisdata(_hander, 4, new short[] { 0, 1, 2, 3, 4 }, 4, ref len4, odbaxdt4);
            SaveResult(ref dataInfo, ret, "cnc_rdaxisdata_type4", odbaxdt4);

            // 5. Speed Tony 2020-3-31 test ret=4
            var odbaxdt5 = new ODBAXDT();
            short len5 = 3;
            ret = cnc_rdaxisdata(_hander, 5, new short[] { 0, 1, 2, 3, 4, 5 }, 4, ref len5, odbaxdt5);
            SaveResult(ref dataInfo, ret, "cnc_rdaxisdata_type5", odbaxdt5);
            #endregion
        }
        public bool Read<T>(string addr, string length, ref object data)
        {
            return true;
        }
        public bool Read<T>(string addr, int length, ref object data)
        {
            if (ConnectStatus == false)
            {
                return false;
            }

            var dataInfo = new Dictionary<string, object>();
            GetCncDeviceInfo(ref dataInfo);
            data = dataInfo;

            ErrorCode = 0;
            Message = "Success";

            return true;
        }

        public bool ExeCmd(string cmd)
        {
            return true;
        }

        public bool Write<T>(string addr, object data)
        {
            throw new NotImplementedException();
        }
    }
}
