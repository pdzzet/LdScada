using KHBC.Core.Device;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using KHBC.Core.Log;
using RXOSPAPILib;

namespace KHBC.LD.OLCM.DeviceApi
{
    public class OlcmAlarmInfor
    {
    public string ALARM_NO;
    public string ALARM_CODE;
    public string ALARM_STRINGS;
    public string ALARM_DATE;
    public string ALARM_TIME;
    public string ALARM_OBJECT;
    }
    public class OkumaDeviceApi:IDevice
    {
        //事件
        public static event Action<string> Loged;
        public static event Action<Exception> ErrorLoged;

        public bool ConnectStatus { get; set; } = false;
        public bool DataChange { get; set; } = false;
        public string Message { get; set; }
        public int ErrorCode { get; set; } = 0;
        //全局变量
        public bool IsConnected => _obj != null;
        /// <summary>
        /// 0代表成功
        /// </summary>
        public int Result;
        /// <summary>
        /// 错误提示
        /// </summary>
        public string ErrMsg;
        public int ErrData;
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrStr;
        //私有资源
        private object _obj;
        private NCTYPE _ncType=NCTYPE.TYPE_LATHE;
        private readonly string _serviceName;
        private readonly string _ipAddress;
        private readonly UInt16 _port;
        Dictionary<string,object> dataInfo = new Dictionary<string, object>();
     
        public OkumaDeviceApi(string serviceName, string ipAddress, int port)
        {
            _serviceName = serviceName;
            _ipAddress = ipAddress;
            _port = Convert.ToUInt16(port);
            _obj = null;
        }
        ~OkumaDeviceApi()
        {
            Disconnect();
        }

        public void Connect()
        {
            bool isData = true;
             NCTYPE ncType = NCTYPE.TYPE_LATHE;
            ClearError();
            try
            {
                Disconnect();
                var progId = isData
                    ? ncType == NCTYPE.TYPE_MC
                        ? "RXOSPAPI.DATAM"
                        : "RXOSPAPI.DATAL"
                    : ncType == NCTYPE.TYPE_MC
                        ? "RXOSPAPI.CMDM"
                        : "RXOSPAPI.CMDL";
                OnLoged($"Connect {progId} {_ipAddress}{ncType}");
                _obj = CreateObject(progId, _ipAddress);
                _ncType = ncType;
                    if(_obj!=null)
                    { 
                    ConnectStatus = true;
                    Message = $"连接设备成功: {_ipAddress}:{_port}";
                    Logger.Main.Info($"[{_serviceName}{Message}");
                } 
            }
            catch (Exception e)
            {
                ErrMsg = "Connect Fail";
                ErrStr = e.Message;
                OnErrorLoged(e);
                Result = -1;
                Message = $"连接设备失败: {_ipAddress}:{_port}:{ e.Message}";
                Logger.Main.Info($"[{_serviceName}{Message}");
                
            }
        }
  
            /// <summary>
            /// 获取数据
            /// </summary>
            /// <param name="isVar2"></param>
            /// <param name="subSystem"></param>,
            /// 
            /// <param name="majorIndex"></param>
            /// <param name="subScript"></param>
            /// <param name="minorIndex"></param>
            /// <param name="style"></param>
            /// <returns></returns>
            public string GetByString(bool isVar2, short subSystem, int majorIndex, int subScript, int minorIndex, short style)
        {
            var data = string.Empty;
            if (_obj == null)
            {
                Result = -1;
                ErrMsg = "设备未连接";
                return data;
            }
            OnLoged($"GetByString {isVar2},{subSystem},{majorIndex},{subScript},{minorIndex},{style}");
            try
            {
                if ((subSystem == 1) || (subSystem == 33) || (subSystem == 34))
                {
                    if (_ncType == NCTYPE.TYPE_MC)
                        (_obj as DataM)?.StartUpdate();
                    else
                        (_obj as DataL)?.StartUpdate();
                }

                if (_ncType == NCTYPE.TYPE_MC)
                {
                    var dm = _obj as DataM;
                    if (dm != null)
                    {
                        data = isVar2
                            ? dm.GetByString2(subSystem, majorIndex, subScript, minorIndex, style)
                            : dm.GetByString(subSystem, (short) majorIndex, (short) subScript, (short) minorIndex,
                                style);
                        Result = dm.GetByLastError();
                        if (Result != 0)
                        {
                            ErrMsg = dm.GetErrMsg(subSystem, Result);
                            ErrData = dm.GetErrData(subSystem, Result);
                            ErrStr = dm.GetErrStr(subSystem, Result);
                        }
                    }
                }
                else
                {
                    var dl = _obj as DataL;
                    if (dl != null)
                    {
                        data = dl.GetByString(subSystem, (short) majorIndex, (short) subScript, (short) minorIndex,
                            style);
                        Result = dl.GetByLastError();
                        if (Result != 0)
                        {
                            ErrMsg = dl.GetErrMsg(subSystem, Result);
                            ErrData = dl.GetErrData(subSystem, Result);
                            ErrStr = dl.GetErrStr(subSystem, Result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrMsg = "GetByString Fail";
                ErrStr = ex.Message;
                OnErrorLoged(ex);
            }
            return data;

        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="isVar2"></param>
        /// <param name="subSystem"></param>
        /// <param name="majorIndex"></param>
        /// <param name="subScript"></param>
        /// <param name="minorIndex"></param>
        /// <param name="style"></param>
        /// <param name="strData"></param>
        public void SetByString(bool isVar2, short subSystem, int majorIndex, int subScript, int minorIndex, short style, string strData)
        {
            if (_obj == null)
            {
                Result = -1;
                ErrMsg = "设备未连接";
                return;
            }
            OnLoged($"SetByString {isVar2},{subSystem},{majorIndex},{subScript},{minorIndex},{style},{strData}");
            try
            {
                if (_ncType == NCTYPE.TYPE_MC)
                    (_obj as DataM)?.NcRunInterLockRelease(1);
                else
                    (_obj as DataL)?.NcRunInterLockRelease(1);
                if (_ncType == NCTYPE.TYPE_MC)
                {
                    var dm = _obj as DataM;
                    if (dm != null)
                    {
                        Result = isVar2
                            ? dm.SetByString2(subSystem, majorIndex, subScript, minorIndex, style, strData)
                            : dm.SetByString(subSystem, (short) majorIndex, (short) subScript, (short) minorIndex,
                                style, strData);
                        if (Result != 0)
                        {
                            ErrMsg = dm.GetErrMsg(subSystem, Result);
                            ErrData = dm.GetErrData(subSystem, Result);
                            ErrStr = dm.GetErrStr(subSystem, Result);
                        }
                    }
                }
                else
                {
                    var dl = _obj as DataL;
                    if (dl != null)
                    {
                        Result = dl.SetByString(subSystem, (short) majorIndex, (short) subScript, (short) minorIndex,
                            style, strData);
                        if (Result != 0)
                        {
                            ErrMsg = dl.GetErrMsg(subSystem, Result);
                            ErrData = dl.GetErrData(subSystem, Result);
                            ErrStr = dl.GetErrStr(subSystem, Result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrMsg = "SetByString Fail";
                ErrStr = ex.Message;
                OnErrorLoged(ex);
            }
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="isVar2"></param>
        /// <param name="subSystem"></param>
        /// <param name="majorIndex"></param>
        /// <param name="subScript"></param>
        /// <param name="minorIndex"></param>
        /// <param name="style"></param>
        /// <param name="strData"></param>
        public void AddByString(bool isVar2, short subSystem, int majorIndex, int subScript, int minorIndex, short style, string strData)
        {
            if (_obj == null)
            {
                Result = -1;
                ErrMsg = "设备未连接";
                return;
            }
            OnLoged($"AddByString {isVar2},{subSystem},{majorIndex},{subScript},{minorIndex},{style},{strData}");
            try
            {
                if (_ncType == NCTYPE.TYPE_MC)
                    (_obj as DataM)?.NcRunInterLockRelease(1);
                else
                    (_obj as DataL)?.NcRunInterLockRelease(1);

                if (_ncType == NCTYPE.TYPE_MC)
                {
                    var dm = _obj as DataM;
                    if (dm != null)
                    {
                        Result = isVar2
                            ? dm.AddByString2(subSystem, majorIndex, subScript, minorIndex, style, strData)
                            : dm.AddByString(subSystem, (short) majorIndex, (short) subScript, (short) minorIndex,
                                style, strData);
                        if (Result != 0)
                        {
                            ErrMsg = dm.GetErrMsg(subSystem, Result);
                            ErrData = dm.GetErrData(subSystem, Result);
                            ErrStr = dm.GetErrStr(subSystem, Result);
                        }
                    }
                }
                else
                {
                    var dl = _obj as DataL;
                    if (dl != null)
                    {
                        Result = dl.AddByString(subSystem, (short) majorIndex, (short) subScript, (short) minorIndex,
                            style, strData);
                        if (Result != 0)
                        {
                            ErrMsg = dl.GetErrMsg(subSystem, Result);
                            ErrData = dl.GetErrData(subSystem, Result);
                            ErrStr = dl.GetErrStr(subSystem, Result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrMsg = "AddByString Fail";
                ErrStr = ex.Message;
                OnErrorLoged(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isVar2"></param>
        /// <param name="subSystem"></param>
        /// <param name="majorIndex"></param>
        /// <param name="subScript"></param>
        /// <param name="minorIndex"></param>
        /// <param name="style"></param>
        /// <param name="strData"></param>
        public void CalByString(bool isVar2, short subSystem, int majorIndex, int subScript, int minorIndex, short style, string strData)
        {
            if (_obj == null)
            {
                Result = -1;
                ErrMsg = "设备未连接";
                return;
            }
            OnLoged($"CalByString {isVar2},{subSystem},{majorIndex},{subScript},{minorIndex},{style},{strData}");
            try
            {
                if (_ncType == NCTYPE.TYPE_MC)
                    (_obj as DataM)?.NcRunInterLockRelease(1);
                else
                    (_obj as DataL)?.NcRunInterLockRelease(1);
                if (_ncType == NCTYPE.TYPE_MC)
                {
                    var dm = _obj as DataM;
                    if (dm != null)
                    {
                        Result = isVar2
                            ? dm.CalByString2(subSystem, majorIndex, subScript, minorIndex, style, strData)
                            : dm.CalByString(subSystem, (short) majorIndex, (short) subScript, (short) minorIndex,
                                style, strData);
                        if (Result != 0)
                        {
                            ErrMsg = dm.GetErrMsg(subSystem, Result);
                            ErrData = dm.GetErrData(subSystem, Result);
                            ErrStr = dm.GetErrStr(subSystem, Result);
                        }
                    }
                }
                else
                {
                    var dl = _obj as DataL;
                    if (dl != null)
                    {
                        Result = dl.CalByString(subSystem, (short) majorIndex, (short) subScript, (short) minorIndex,
                            style, strData);
                        if (Result != 0)
                        {
                            ErrMsg = dl.GetErrMsg(subSystem, Result);
                            ErrData = dl.GetErrData(subSystem, Result);
                            ErrStr = dl.GetErrStr(subSystem, Result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrMsg = "CalByString Fail";
                ErrStr = ex.Message;
                OnErrorLoged(ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainFile"></param>
        /// <param name="subFile"></param>
        /// <param name="ssbFile">【L】 must</param>
        /// <param name="prgName"></param>
        /// <param name="spindleOrMode">【L】[Spindle 0：Std./2SP-R 1：2SP-L] 【M】[Mode 1:A 2：B 3:C]</param>
        /// <returns></returns>
        public void SelectMainProgram(string mainFile, string subFile, string ssbFile, string prgName, short spindleOrMode)
        {
            if (_obj == null)
            {
                Result = -1;
                ErrMsg = "设备未连接";
                return;
            }
            OnLoged($"SelectMainProgram {mainFile},{subFile},{ssbFile},{prgName},{spindleOrMode}");
            try
            {
                if (_ncType == NCTYPE.TYPE_LATHE)
                {
                    var cl = _obj as CmdL;
                    if (cl != null)
                    {
                        Result = cl.SelectMainProgram(mainFile, subFile, ssbFile, prgName, spindleOrMode);
                        if (Result != 0)
                        {
                            ErrMsg = cl.GetLastErrorMsg();
                            ErrData = cl.GetLastErrorData();
                            ErrStr = cl.GetLastErrorStr();
                        }
                    }
                }
                else
                {
                    var cm = _obj as CmdM;
                    if (cm != null)
                    {
                        Result = cm.SelectMainProgram(mainFile, subFile, prgName, spindleOrMode);
                        if (Result != 0)
                        {
                            ErrMsg = cm.GetLastErrorMsg();
                            ErrData = cm.GetLastErrorData();
                            ErrStr = cm.GetLastErrorStr();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrMsg = "SelectMainProgram Fail";
                ErrStr = ex.Message;
                OnErrorLoged(ex);
            }
        }



        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            if (_obj != null)
            {
                Marshal.ReleaseComObject(_obj);
                _obj = null;
            }
        }
        #region 私有方法

        private void ClearError()
        {
            Result = 0;
            ErrMsg = "";
            ErrData = 0;
            ErrStr = "";
        }
        private object CreateObject(string progId, string serverName)
        {
            var t = string.IsNullOrEmpty(serverName) ? Type.GetTypeFromProgID(progId) : Type.GetTypeFromProgID(progId, serverName, true);
            return Activator.CreateInstance(t);
        }

        private static void OnLoged(string obj)
        {
            Loged?.Invoke(obj);
        }

        private static void OnErrorLoged(Exception obj)
        {
            ErrorLoged?.Invoke(obj);
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
        #endregion
      
        public bool Read<T>(string addr, int length, ref object data)
        {
            return true;
        }
        public bool Read<T>(string addr, string proid, ref object data)
        {
            if (ConnectStatus == false)
            {
                return false;
            }
           
            var osppara = SpliAddress(addr);
            if (osppara != null)
            {
                string resultdata = GetByString(false, (short)osppara["ss_idx"], osppara["major_idx"], osppara["subscript"], osppara["minor_idx"], (short)osppara["style"]);
                SaveResult(ref dataInfo, 0, proid, resultdata);
                data = dataInfo;
                ErrorCode = 0;
                Message = "Success";
                return true;
            }
            else
                return false;   
        }

        public bool ExeCmd(string cmd)
        {
            return true;
        }

        public bool Write<T>(string addr, object data)
        {
            throw new NotImplementedException();
        }
        public Dictionary<string, int> SpliAddress(string addr= "0,19,0,0,8")
        {
            Dictionary<string,int> para = new Dictionary<string, int>();
            string[] address = addr.Split(',');
            int[] paraint={0,0,0,0,0};
            if (address.Length == 5)
            { 
                for (int i=0; i < address.Length; i++ )
                {
                 if (!Int32.TryParse(address[i], out paraint[i]))
                 {
                    Message = $"获取CNC机台信息参数设定错误:({addr})！";
                    Logger.Main.Info($"[{_serviceName}{Message}");
                 }  
                }
            }
            para["ss_idx"] = paraint[0];
            para["major_idx"] = paraint[1];
            para["subscript"] = paraint[2];
            para["minor_idx"] = paraint[3];
            para["style"] = paraint[4];
            return para;
        }
    }
}
