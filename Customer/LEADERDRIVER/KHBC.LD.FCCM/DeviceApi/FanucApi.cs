using System;
using System.Collections.Generic;
using static KHBC.LD.FCCM.DeviceApi.Focas1;

namespace KHBC.LD.FCCM.DeviceApi
{
    public class FanucDeviceApi
    {
        public static event Action<string> Loged;
        public static event Action<Exception> ErrorLoged;
        //连接抛出的句柄
        private ushort _hander;
        //全局变量
        public bool IsConnected => _hander != 0;
        /// <summary>
        /// 代表成功
        /// </summary>
        public bool IsSuccess;
        /// <summary>
        /// 错误提示
        /// </summary>
        public string ErrMsg;


        #region 私有变量
        private string _ip;
        private int _port;
        private int _timeout;

        //type:1朱轴压力,-1俩者都有,0主轴监控速度表

        private ODBSPDLNAME _spName = new ODBSPDLNAME();
        private ODBSPDLName _sp = new ODBSPDLName();
        private ODBACT _pindle = new ODBACT();
        private ODBSPN _odbspn = new ODBSPN();
        private ODBSEQ _odbseq = new ODBSEQ();
        private ODBPRO _odbpro = new ODBPRO();
        private ODBALM _odbAlm = new ODBALM();
        private ALMINFO_1 _almInfo1 = new ALMINFO_1();
        //private ODBALMMSG _odbAlmMeg = new ODBALMMSG();
        private OPMSG _oPmsg = new OPMSG();
        private ODBDY2_1 _c = new ODBDY2_1();//动态信息显示
        private ODBAXIS _axis = new ODBAXIS();
        //private ODBM _odbm = new ODBM();//宏类
        private IODBPI _pitch = new IODBPI();
        //private ODBPDFDRV _odbpdfdrv = new ODBPDFDRV();
        private IODBTIMER _iodbtimer = new IODBTIMER();
        //伺服轴
        private ODBSVLOAD _sv = new ODBSVLOAD();
        private ODBSYS _sys = new ODBSYS();
        //private IODBPRM2 _ioDbprm = new IODBPRM2();
        private ODBST _obst = new ODBST();
        private ODBUSEGRP _grp = new ODBUSEGRP();
        //private IODBTGI _btgi = new IODBTGI();
        private ODBTG _btg = new ODBTG();
        private ODBPMCALM _odnpmcalm = new ODBPMCALM();
        private ODBSPEED _speed = new ODBSPEED();

        private ODBEXEPRG _exeprg = new ODBEXEPRG();
        private PRGDIR3 _pRgdir3 = new PRGDIR3();
        private IODBPMC0 _pmc = new IODBPMC0();
        private ODBAHIS _odbahis = new ODBAHIS();

        private ALM_HIS1 _almhis = new ALM_HIS1();
        private ODBHIS _oDbhis = new ODBHIS();
        private ODBTOFS _tof = new ODBTOFS();
        private ODBTLINF _inf = new ODBTLINF();
        private ODBTLIFE5 _tool = new ODBTLIFE5();//读取刀片组的序号
        //private ODBTLIFE2 _tool1 = new ODBTLIFE2();//读取刀片组的全部数量
        //private ODBTLIFE3 _tool2 = new ODBTLIFE3();//刀具的数量
        private ODBTLIFE4 _tool4 = new ODBTLIFE4();

        #endregion

        #region  读取主轴基础参数

        /// <summary>
        /// 主轴名称
        /// </summary>
        /// <param name="a">轴的数量</param>
        /// <returns></returns>
        public string ReadSpindleName(short a)
        {
            if (IsConnected)
            {
                short ret = cnc_rdspdlname(_hander, ref a, _spName);
                if (ret == 0)
                {
                    IsSuccess = true;
                    return FanucComm.GetString(_spName.data1.name);
                }
                IsSuccess = false;
                ErrMsg = $"{_ip}{_port}：主轴名称读取失败:{ret}";
                OnErrorLoged(new Exception(ErrMsg));
            }
            IsSuccess = false;
            ErrMsg = $"{_ip}{_port}：主轴名称读取失败，机器处于断开状态";
            OnErrorLoged(new Exception(ErrMsg));
            return "";
        }

        /// <summary>
        /// 获取主轴的速度
        /// </summary>
        public int ReadSpindleSpeed()
        {
            if (IsConnected)
            {
                short ret = cnc_acts(_hander, _pindle);
                if (ret == 0)
                {
                    IsSuccess = true;
                    return _pindle.data;
                }
                IsSuccess = false;
                ErrMsg = $"{_ip}{_port}：主轴速度读取失败:{ret}";
                OnErrorLoged(new Exception(ErrMsg));
            }
            IsSuccess = false;
            ErrMsg = $"{_ip}{_port}：主轴速度读取失败，机器处于断开状态";
            OnErrorLoged(new Exception(ErrMsg));
            return -1;
        }

        /// <summary>
        /// 主轴实际电流
        /// </summary>
        /// <returns></returns>
        public int ReadRealSpindleElectric()
        {
            if (IsConnected)
            {
                short ret = cnc_rdcurrent(_hander, out short a);
                if (ret == 0)
                {
                    IsSuccess = true;
                    return a;
                }
                IsSuccess = false;
                ErrMsg = $"{_ip}{_port}：主轴实际电流读取失败:{ret}";
                OnErrorLoged(new Exception(ErrMsg));
            }
            IsSuccess = false;
            ErrMsg = $"{_ip}{_port}：主轴实际电流读取失败，机器处于断开状态";
            OnErrorLoged(new Exception(ErrMsg));
            return -1;
        }


        /// <summary>
        /// 主轴负载
        /// </summary>
        /// <param name="a">轴的数量</param>
        /// <returns></returns>
        public int ReadSpindleLoad(short a)
        {
            if (IsConnected)
            {
                short ret = cnc_rdspload(_hander, a, _odbspn);
                if (ret == 0)
                {
                    IsSuccess = true;
                    return _odbspn.data[0];
                }
                IsSuccess = false;
                ErrMsg = $"{_ip}{_port}：主轴负载读取失败:{ret}";
                OnErrorLoged(new Exception(ErrMsg));
            }
            IsSuccess = false;
            ErrMsg = $"{_ip}{_port}：主轴负载读取失败，机器处于断开状态";
            OnErrorLoged(new Exception(ErrMsg));
            return -1;
        }

        /// <summary>
        /// 主轴负载表
        /// </summary>
        /// <param name="a">轴的数量</param>
        /// <returns></returns>
        public int[] ReadSpindleLoads(short a)
        {
            if (IsConnected)
            {

                int[] sploads = new int[a];
                short ret = cnc_rdspmeter(_hander, 1, ref a, _sp);
                if (ret == 0)
                {
                    IsSuccess = true;
                    string spl;
                    for (int i = 0; i < a; i++)
                    {
                        spl = "spload" + (i + 1);
                        ODBSPLOAD_data spLoad = (ODBSPLOAD_data)FanucComm.GetField(_sp, spl);
                        sploads[i] = spLoad.spload.data;
                    }
                    return sploads;
                }
                IsSuccess = false;
                ErrMsg = $"{_ip}{_port}：主轴负载表读取失败:{ret}";
                OnErrorLoged(new Exception(ErrMsg));
            }
            IsSuccess = false;
            ErrMsg = $"{_ip}{_port}：主轴负载表读取失败，机器连接断开状态";
            OnErrorLoged(new Exception(ErrMsg));
            return new int[0];
        }

        /// <summary>
        /// 读取主轴设置的操作模式,ok
        /// </summary>
        /// <returns></returns>
        public int[] ReadPaMode()
        {
            if (IsConnected)
            {
                IsSuccess = true;
                short[] array = new short[] { 1, 2, 3 };
                int[] pm = new int[array.Length];
                short ret = cnc_rdopmode(_hander, out array[0]);
                if (ret == 0)
                {
                    for (int i = 0; i < array.Length; i++)
                        pm[i] = array[i];
                    return pm;
                }
                IsSuccess = false;
                ErrMsg = $"{_ip}{_port}：主轴操作模式读取失败:{ret}";
                OnErrorLoged(new Exception(ErrMsg));
            }
            IsSuccess = false;
            ErrMsg = $"{_ip}{_port}：主轴操作模式读取失败，机器连接断开状态";
            OnErrorLoged(new Exception(ErrMsg));
            return new int[0];
        }

        /// <summary>
        /// 读取主轴数量,ok
        /// </summary>
        /// <returns></returns>
        public int ReadSpindleAmount()
        {
            if (IsConnected)
            {
                short array;
                short ret = cnc_rdcurrent(_hander, out array);
                if (ret == 0)
                {
                    IsSuccess = true;
                    return array;
                }
                IsSuccess = false;
                ErrMsg = $"{_ip}{_port}：主轴数量读取失败:{ret}";
                OnErrorLoged(new Exception(ErrMsg));
            }
            IsSuccess = false;
            ErrMsg = $"{_ip}{_port}：主轴数量读取失败，机器连接断开状态";
            OnErrorLoged(new Exception(ErrMsg));
            return -1;
        }

        #endregion

        #region  读取伺服轴基础参数
        /// <summary>
        /// 伺服轴名称
        /// </summary>
        /// <param name="a">伺服轴数量</param>
        /// <returns></returns>
        public string ReadServoAxisName(short a)
        {
            if (IsConnected)
            {
                //short ret = cnc_rdsvmeter(_hander, ref a, sv);
                //if (ret == 0)
                //{
                //    return FanucComm.GetString(sv.svload1.name);
                //}
            }
            return "";

        }
        /// <summary>
        /// 伺服轴实际电流
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public int ReadRealServoAxisElectric(short account)
        {
            if (IsConnected)
            {
                //short ret = cnc_rdsvmeter(_hander, ref account, sv);
                //if (ret == 0)
                //{
                //    //伺服的加载值
                //    return sv.svload1.data;
                //}
                //else
                //{
                //    OnErrorLogged(new Exception(" : {ret}"));
                //}
            }
            return -1;
        }

        /// <summary>
        /// 伺服轴负载
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public int ReadServoAxisLoad(short account)
        {
            if (IsConnected)
            {
                short ret = cnc_rdsvmeter(_hander, ref account, _sv);
                if (ret == 0)
                {
                    IsSuccess = true;
                    //伺服的加载值 第一个
                    return _sv.svload1.data;
                }
                IsSuccess = false;
                ErrMsg = $"{_ip}{_port}：伺服轴负载读取失败:{ret}";
                OnErrorLoged(new Exception(ErrMsg));
            }
            IsSuccess = false;
            ErrMsg = $"{_ip}{_port}：伺服轴负载读取失败，机器连接断开状态";
            OnErrorLoged(new Exception(ErrMsg));
            return -1;
        }

        /// <summary>
        /// 读取伺服负载表
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public int[] ReadServoAxisLoads(short a)
        {
            if (IsConnected)
            {
                int[] saloads = new int[a];
                short ret = cnc_rdsvmeter(_hander, ref a, _sv);
                if (ret == 0)
                {
                    IsSuccess = true;
                    string svl;
                    for (int i = 0; i < a; i++)
                    {
                        svl = "svload" + (i + 1);
                        LOADELM lm = (LOADELM)FanucComm.GetField(_sv, svl);
                        saloads[i] = lm.data;
                    }
                    //伺服的加载值
                    return saloads;
                }
                IsSuccess = false;
                ErrMsg = $"{_ip}{_port}：伺服负载表读取失败:{ret}";
                OnErrorLoged(new Exception(ErrMsg));
            }
            IsSuccess = false;
            ErrMsg = $"{_ip}{_port}：伺服负载表读取失败，机器连接断开状态";
            OnErrorLoged(new Exception(ErrMsg));
            return new int[0];
        }
        /// <summary>
        /// 伺服调整环路增益,ok
        /// </summary>
        /// <returns></returns>
        public int[] ReadLoopGain()
        {
            if (IsConnected)
            {
                //out 只是表示传递此变量的地址
                int[] array = new int[] { 1, 2, 3 };
                int[] lg = new int[array.Length];
                short ret = cnc_rdloopgain(_hander, out array[0]);
                if (ret == 0)
                {
                    IsSuccess = true;
                    for (int i = 0; i < array.Length; i++)
                        lg[i] = array[i];
                    return lg;
                }
                IsSuccess = false;
                ErrMsg = $"伺服调整环路增益读取失败 :{ret}";
                OnErrorLoged(new Exception(ErrMsg));
            }
            IsSuccess = false;
            ErrMsg = $"{_ip}{_port}：伺服调整环路增益读取失败，机器连接断开状态";
            OnErrorLoged(new Exception(ErrMsg));
            return new int[0];
        }


        /// <summary>
        /// 读取伺服调整的电流,ok
        /// </summary>
        /// <returns></returns>
        public short[] ReadServoAxisCurrentEc()
        {
            if (IsConnected)
            {
                ////可以先读取最大轴数号
                cnc_sysinfo(_hander, _sys);
                int n = Convert.ToInt32(_sys.axes.Length);
                short[] array = new short[n];
                short ret = cnc_rdcurrent(_hander, out array[0]);
                if (ret == 0)
                {
                    IsSuccess = true;
                    return array;
                }
                IsSuccess = false;
                ErrMsg = $"伺服调整的电流读取失败 :{ret}";
                OnErrorLoged(new Exception(ErrMsg));
            }
            IsSuccess = false;
            ErrMsg = $"{_ip}{_port}：伺服调整的电流读取失败，机器连接断开状态";
            OnErrorLoged(new Exception(ErrMsg));
            return new short[0];
        }

        /// <summary>
        /// 读取伺服调整的速度,ok
        /// </summary>
        /// <returns></returns>
        public int ReadServoAxisSpeed()
        {
            if (IsConnected)
            {
                cnc_sysinfo(_hander, _sys);
                //int n = Convert.ToInt32(_sys.axes.Length);
                int a;
                short ret = cnc_rdsrvspeed(_hander, out a);
                if (ret == 0)
                {
                    IsSuccess = true;
                    return a;
                }
                IsSuccess = false;
                ErrMsg = $"伺服调整的速度读取失败 :{ret}";
                OnErrorLoged(new Exception(ErrMsg));
            }
            IsSuccess = false;
            ErrMsg = $"{_ip}{_port}：伺服调整的速度读取失败，机器连接断开状态";
            OnErrorLoged(new Exception(ErrMsg));
            return -1;
        }


        /// <summary>
        /// 读取动态的数据的函数
        /// </summary>
        /// <returns></returns>
        public string Readdynamic()
        {
            if (IsConnected)
            {
                short ret = cnc_rddynamic2(_hander, 2, 28 + 4 * 4 * 1, _c);
                if (ret == 0)
                {
                    IsSuccess = true;
                    return $"进给速率: {_c.actf}; " +
                           $"主轴速率: {_c.acts} ;" +
                           $"警报:{_c.alarm}; " +
                           $"轴的数量:{_c.axis} ;" +
                           $"可能用:{_c.dummy}; " +
                           $"绝对 位置{_c.pos.absolute[0] + _c.pos.absolute[1]};" +
                           $"相对 位置：{_c.pos.relative[0] + _c.pos.relative[1]};" +
                           $"机器 位置:{_c.pos.machine[0] + _c.pos.machine[1]}; " +
                           $"剩余 位置：{_c.pos.distance[0] + _c.pos.distance[1]};" +
                           $"当前的程序 {_c.prgmnum}; " +
                           $"主要的程序:{_c.prgmnum}; " +
                           $"顺序号:{_c.seqnum}";
                }
                IsSuccess = false;
                ErrMsg = $"动态数据读取失败 :{ret}";
                OnErrorLoged(new Exception(ErrMsg));
            }
            IsSuccess = false;
            ErrMsg = $"{_ip}{_port}：动态数据读取失败，机器连接断开状态";
            OnErrorLoged(new Exception(ErrMsg));
            return "";
        }

        /// <summary>
        /// 读取速度信息
        /// </summary>
        public void ReadSpeed()
        {

            short ret = cnc_rdspeed(_hander, -1, _speed);
            if (ret == 0)
            {
                //speed.actf.name + "   " + speed.actf.data + "   " + speed.actf.suff + " " + speed.actf.unit + "  " + speed.actf.disp;
                //speed.acts.name + "   " + speed.acts.data + "   " + speed.acts.suff;
            }
        }
        /// <summary>
        /// 读取轴跳过的位置
        /// </summary>
        /// <returns></returns>
        public int ReadSkipPosition()
        {
            if (IsConnected)
            {
                short ret = cnc_skip(_hander, 1, 8, _axis);
                if (ret == 0)
                {
                    IsSuccess = true;
                    return _axis.data[0];
                }
                IsSuccess = false;
                ErrMsg = $"轴跳过的位置读取失败 :{ret}";
                OnErrorLoged(new Exception(ErrMsg));
            }
            IsSuccess = false;
            ErrMsg = $"{_ip}{_port}：轴跳过的位置读取失败，机器连接断开状态";
            OnErrorLoged(new Exception(ErrMsg));
            return -1;
        }
        /// <summary>
        /// 伺服延迟
        /// </summary>
        /// <returns></returns>
        public int ReadServeDelay()
        {
            if (IsConnected)
            {
                short ret = cnc_srvdelay(_hander, 1, 8, _axis);
                if (ret == 0)
                {
                    IsSuccess = true;
                    return _axis.data[0];
                }
                IsSuccess = false;
                ErrMsg = $"伺服延迟读取失败 :{ret}";
                OnErrorLoged(new Exception(ErrMsg));
            }
            IsSuccess = false;
            ErrMsg = $"{_ip}{_port}：伺服延迟读取失败，机器连接断开状态";
            OnErrorLoged(new Exception(ErrMsg));
            return -1;
        }
        /// <summary>
        /// 加速和减速延迟
        /// </summary>
        public int ReadAddDaddDelay()
        {
            if (IsConnected)
            {
                short ret = cnc_accdecdly(_hander, 1, 8, _axis);
                if (ret == 0)
                {
                    IsSuccess = true;
                    return _axis.data[0];
                }
                IsSuccess = false;
                ErrMsg = $"加减速延迟读取失败 :{ret}";
                OnErrorLoged(new Exception(ErrMsg));
            }
            IsSuccess = false;
            ErrMsg = $"{_ip}{_port}：加减速延迟读取失败，机器连接断开状态";
            OnErrorLoged(new Exception(ErrMsg));
            return -1;
        }


        //读取计时器
        public string ReadTimmer()
        {
            if (IsConnected)
            {
                int ret = cnc_gettimer(_hander, _iodbtimer);
                if (ret == EW_OK)
                {
                    IsSuccess = true;
                    TIMER_DATE timerdate = _iodbtimer.date;
                    TIMER_TIME timertime = _iodbtimer.time;
                    return
                        $"{timerdate.year}-{timerdate.month}-{timerdate.date} {timertime.hour}:{timertime.minute}:{timertime.second}";
                }
                IsSuccess = false;
                ErrMsg = $"计时器读取失败 :{ret}";
                OnErrorLoged(new Exception(ErrMsg));
            }
            IsSuccess = false;
            ErrMsg = $"{_ip}{_port}：计时器读取失败，机器连接断开状态";
            OnErrorLoged(new Exception(ErrMsg));
            return "";
        }

        #endregion

        #region  读取CNC基础参数

        /// <summary>
        /// 目前执行单节
        /// </summary>
        /// <returns></returns>
        public string ReadCurrentImpSection()
        {
            return "";
        }

        /// <summary>
        /// 主程序号码
        /// </summary>
        /// <returns></returns>
        public string ReadMainProNumber()
        {
            if (IsConnected)
            {
                IsSuccess = true;
                cnc_exeprgname(_hander, _exeprg);
                short num = 2;
                int oNum = _exeprg.o_num;
                short num1 = 1;
                cnc_rdprogdir3(_hander, num, ref oNum, ref num1, _pRgdir3);
                return _pRgdir3.dir1.comment;
            }
            IsSuccess = false;
            ErrMsg = $"{_ip}{_port}：主程序号码读取失败，机器连接断开状态";
            OnErrorLoged(new Exception(ErrMsg));
            return "";
        }
        /// <summary>
        /// 读取执行中的序列号
        /// </summary>
        /// <returns></returns>
        public string ReadExecuteProgNumber()
        {
            if (IsConnected)
            {
                short ret = cnc_rdseqnum(_hander, _odbseq);
                if (ret == EW_OK)
                {
                    IsSuccess = true;
                    return _odbseq.data.ToString();
                }
                IsSuccess = false;
                ErrMsg = $"{_ip}{_port}：执行中的序列号读取失败:{ret}";
                OnErrorLoged(new Exception(ErrMsg));
            }
            IsSuccess = false;
            ErrMsg = $"{_ip}{_port}：执行中的序列号读取失败，机器连接断开状态";
            OnErrorLoged(new Exception(ErrMsg));
            return "";
        }
        //读取正在执行的程序号
        public string ReadCurrentExecuteProgNumber()
        {
            if (IsConnected)
            {
                IsSuccess = true;
                cnc_rdprgnum(_hander, _odbpro);
                return _odbpro.data.ToString();
            }
            IsSuccess = false;
            ErrMsg = $"{_ip}{_port}：正在执行的程序号读取失败，机器连接断开状态";
            OnErrorLoged(new Exception(ErrMsg));
            return "";
        }
        #endregion

        #region  读取CNC辅助参数
        /// <summary>
        /// 刀具号码
        /// </summary>
        /// <returns></returns>
        public int ReadKnifeNumber()
        {
            if (IsConnected)
            {

                cnc_rdtlusegrp(_hander, _grp);
                short a = Convert.ToInt16(_grp.use);
                if (a == 0)
                {
                    IsSuccess = false;
                    ErrMsg = $"{_ip}{_port}：刀具号码读取失败，刀具编号获取失败";
                    OnErrorLoged(new Exception(ErrMsg));
                    return -1;
                }
                short ret = cnc_rdtoolgrp(_hander, a, 20 + 20 * 1, _btg);
                if (ret == 0)
                {
                    IsSuccess = true;
                    return _btg.data.data1.tool_num;
                }
                IsSuccess = false;
                ErrMsg = $"{_ip}{_port}：刀具号码读取失败：{ret}";
                OnErrorLoged(new Exception(ErrMsg));
            }
            IsSuccess = false;
            ErrMsg = $"{_ip}{_port}：刀具号码读取失败，机器连接断开状态";
            OnErrorLoged(new Exception(ErrMsg));
            return -1;
        }

        private void OnErrorLogged(Exception ex)
        {
        }

        /// <summary>
        /// 刀具寿命 哈希值 
        /// </summary>
        /// <returns></returns>
        public string ReadKnifeLife()
        {
            if (IsConnected)
            {
                cnc_rdtlusegrp(_hander, _grp);
                short a = Convert.ToInt16(_grp.use);
                if (a == 0)
                {
                    IsSuccess = false;
                    ErrMsg = $"{_ip}{_port}：刀具寿命读取失败，刀具编号获取失败";
                    OnErrorLoged(new Exception(ErrMsg));
                    return "";
                }
                short ret = cnc_rdtoolgrp(_hander, a, 20 + 20 * 1, _btg);
                if (ret == 0)
                {
                    IsSuccess = true;
                    return _btg.life.ToString();
                }
                IsSuccess = false;
                ErrMsg = $"{_ip}{_port}：刀具寿命读取失败:{ret}";
                OnErrorLoged(new Exception(ErrMsg));
            }
            IsSuccess = false;
            ErrMsg = $"{_ip}{_port}：刀具寿命读取失败，机器连接断开状态";
            OnErrorLoged(new Exception(ErrMsg));
            return "";
        }
        /// <summary>
        /// 主轴实际转速
        /// </summary>
        /// <returns></returns>
        public int ReadRealSpindleSpeed()
        {
            if (IsConnected)
            {
                short ret = cnc_acts(_hander, _pindle);
                if (ret == 0)
                {
                    return _pindle.data;
                }
                OnErrorLoged(new Exception($"ReadRealSpindleSpeed读取失败 :{ret}"));
            }
            return -1;
        }
        /// <summary>
        /// 主轴命令转速
        /// </summary>
        /// <returns></returns>
        public int ReadSpindleCommandSpeed()
        {

            return -1;
        }
        /// <summary>
        /// 主轴转速百分百(串行主轴的最大转速比)??
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public int ReadSpindleSpeedPercent(short a)
        {
            if (IsConnected)
            {
                short ret = cnc_rdspmaxrpm(_hander, a, _odbspn);

                if (ret == 0)
                {
                    return _odbspn.data[0];
                }
                OnErrorLoged(new Exception($"ReadSpindleSpeedPercent读取失败 :{ret}"));
            }
            return -1;
        }
        /// <summary>
        /// 实际进给率
        /// </summary>
        /// <returns></returns>
        public int ReadActualFeedRate()
        {
            if (IsConnected)
            {
                cnc_actf(_hander, _pindle);
                pmc_rdpmcrng(_hander, 0, 0, 12, 13, 10, _pmc);
                return _pmc.cdata[0];
            }
            return -1;
        }
        /// <summary>
        /// 命令进给率??
        /// </summary>
        /// <returns></returns>
        public int ReadCommandRate()
        {
            return -1;
        }
        /// <summary>
        /// 进给率百分百??
        /// </summary>
        /// <returns></returns>
        public int ReadRatePercent()
        {
            return -1;
        }
        #endregion

        #region OEE相关数据

        /// <summary>
        /// 通电时间
        /// </summary>
        /// <returns></returns>
        public long ReadPowerOnTime()
        {
            return -1;
        }
        /// <summary>
        /// 切削时间
        /// </summary>
        /// <returns></returns>
        public long ReadCuttingTime()
        {
            return -1;
        }
        /// <summary>
        /// 运转时间
        /// </summary>
        /// <returns></returns>
        public long ReadRunningTime()
        {
            return -1;
        }
        /// <summary>
        /// 循环时间
        /// </summary>
        /// <returns></returns>
        public long ReadCycleTime()
        {

            return -1;
        }
        /// <summary>
        /// 日期时间
        /// </summary>
        /// <returns></returns>
        public string ReadDateTime()
        {
            if (IsConnected)
            {
                int ret = cnc_gettimer(_hander, _iodbtimer);
                if (ret == EW_OK)
                {
                    TIMER_DATE timerdate = _iodbtimer.date;
                    TIMER_TIME timertime = _iodbtimer.time;

                    return $"{timerdate.year}-{timerdate.month}-{timerdate.date}:{timertime.hour} :{timertime.hour}:{timertime.second}";
                }
                OnErrorLoged(new Exception($"ReadDateTime读取失败 :{ret}"));
            }
            return "";
        }
        /// <summary>
        /// 加工零件总数
        /// </summary>
        /// <returns></returns>
        public int ReadProcessTotalNumber()
        {

            return -1;
        }
        /// <summary>
        /// 加工零件数
        /// </summary>
        /// <returns></returns>
        public int ReadProcessNumber()
        {

            return -1;
        }
        /// <summary>
        /// 所需零件数
        /// </summary>
        /// <returns></returns>
        public int ReadProcessNeedNumber()
        {
            return -1;
        }
        #endregion


        #region 状态，警告，排故
        /// <summary>
        /// 目前报警状态
        /// </summary>
        /// <returns></returns>
        public string ReadCurrentAlarmStatus()
        {
            if (IsConnected)
            {
                short ret = cnc_alarm(_hander, _odbAlm);
                if (ret == 0)
                {
                    return _odbAlm.data.ToString();
                }
                OnErrorLoged(new Exception($"ReadCurrentAlarmStatus读取失败 :{ret}"));
            }
            return "";
        }

        /// <summary>
        /// 目前警告(读取报警信息)
        /// </summary>
        /// <param name="a">1：表示有消息；0：无消息</param>
        /// <param name="b">b表示警报的类型，比如5：电机温度过热</param>
        /// <param name="c">short c = 8</param>
        /// <returns></returns>
        public string ReadCurrentAlarmInfo(short a, short b, short c)
        {
            //获取报警信息
            if (IsConnected)
            {
                short ret = cnc_rdalminfo(_hander, a, b, c, _almInfo1);
                if (ret == 0)
                {
                    return $"轴：{_almInfo1.msg1.axis}；警告：{_almInfo1.msg1.alm_no}；" +
                           $"轴：{_almInfo1.msg2.axis}；警告：{_almInfo1.msg2.alm_no}；" +
                           $"轴：{_almInfo1.msg3.axis}；警告：{_almInfo1.msg3.alm_no}；" +
                           $"轴：{_almInfo1.msg4.axis}；警告：{_almInfo1.msg4.alm_no}；" +
                           $"轴：{_almInfo1.msg5.axis}；警告：{_almInfo1.msg5.alm_no}";
                }
                OnErrorLoged(new Exception($"ReadCurrentAlarmInfo读取失败 :{ret}"));
            }
            return "";
        }
        /// <summary>
        /// 目前讯息(读取警报消息)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public string[] ReadCurrentAlarmMessage(short a, short b)
        {
            //if (IsConnected)
            //{
            //    StringBuilder m = new StringBuilder("");
            //    //cnc_rdalmmsg(_hander, a,ref b, odbAlmMeg);
            //    short ret = cnc_rdspdlalm(_hander, m); 
            //       if (ret == 0)
            //    {
            //        return m.ToString();
            //    }
            //    else
            //    {
            //        OnErrorLogged(new Exception(" : {ret}"));
            //    }
            //}
            return new String[0];
        }
        /// <summary>
        /// .目前状态 PMC的状态
        /// </summary>
        /// <returns></returns>
        public int ReadCurrentStatus()
        {
            if (IsConnected)
            {
                //运行状态: obst.run//0停止，1待机，开动
                //警报状态:  obst.alarm);//0没有警报，1表示有警报
                //自动模式下的选择:  obst.aut
                //手动模式下的选择: obst.mstb
                cnc_statinfo(_hander, _obst);
            }
            return _obst.run;
        }

        /// <summary>
        /// PLC报警
        /// </summary>
        /// <param name="number"></param>
        /// <param name="readnum">When 'read_num' is specified less than the actual number of PMC alarms occurred on PMC side, '1' is returned. 
        ///-1  :  There is no alarm. 
        ///0  :  All alarms were read. 
        ///1  :  Alarm still exists.</param>
        /// <returns></returns>
        public string[] ReadPlcAlarm(short number, short readnum)
        {

            if (IsConnected)
            {
                string[] ss = new string[readnum];
                int ret = pmc_rdalmmsg(_hander, number, ref readnum, out short c, _odnpmcalm);
                string buf;
                if (ret == EW_OK)
                {
                    if (c == -1)
                    {
                        OnErrorLogged(new Exception("读取数量超过警告数量，读取失败"));
                        return new string[0];
                    }
                    for (int i = 1; i < readnum + 1; i++)
                    {
                        buf = "msg" + i;
                        ss[i - 1] = FanucComm.GetField(FanucComm.GetField(_odnpmcalm, buf), "almmsg").ToString();
                    }
                    return ss;
                }
                OnErrorLoged(new Exception($"ReadPlcAlarm读取失败 :{ret}"));
            }
            return new string[0];
        }

        /// <summary>
        /// 警报履历
        /// </summary>
        /// <param name="a">起始</param>
        /// <param name="b">结束</param>
        /// <param name="length">请求长度</param>
        /// <returns></returns>
        public string[] ReadHistoryAlarm(ushort a, ushort b, ushort length)
        {
            //从最历史的时间开始
            if (IsConnected)
            {
                int ret = cnc_rdalmhistry(_hander, a, b, length, _odbahis);
                if (ret == EW_OK)
                {
                    ushort sno = _odbahis.s_no;
                    ushort eno = _odbahis.e_no;
                    _almhis = _odbahis.alm_his;
                    string[] alarms = new string[eno - sno + 1];
                    for (int i = 1; i < eno - sno + 1; i++)
                    {
                        string time = "";
                        object datao = FanucComm.GetField(_almhis, "data" + i);
                        time += FanucComm.GetField(datao, "year").ToString().PadLeft(4, '0');
                        time += "/";
                        time += FanucComm.GetField(datao, "month").ToString().PadLeft(2, '0');
                        time += "/";
                        time += FanucComm.GetField(datao, "day").ToString().PadLeft(2, '0');
                        time += " ";
                        time += FanucComm.GetField(datao, "hour").ToString().PadLeft(2, '0');
                        time += ":";
                        time += FanucComm.GetField(datao, "minute").ToString().PadLeft(2, '0');
                        time += ":";
                        time += FanucComm.GetField(datao, "second").ToString().PadLeft(2, '0');
                        string message = FanucComm.GetField(datao, "alm_msg").ToString();
                        string almgrp = FanucComm.GetAlmgrp(Convert.ToInt16(FanucComm.GetField(datao, "alm_grp")));
                        var almno = FanucComm.GetField(datao, "alm_no").ToString().PadLeft(4, '0');
                        string temp = almgrp + almno;
                        alarms[i - 1] = (time + "|" + temp.PadRight(15, ' ') + "|" + message);
                    }
                    return alarms;
                }
                OnErrorLogged(new Exception($"ReadHistoryAlarm读取失败 : {ret}"));
            }
            return new string[0];
        }

        /// <summary>
        /// 警报履历最大号码
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public int ReadHistoryAlarmMaxNumb(ushort a, ushort b, ushort length)
        {
            //从最历史的时间开始
            if (IsConnected)
            {
                //int ret = cnc_rdalmhistry(_hander, a, b, length, _odbahis);
                //if (ret == EW_OK)
                //{
                //    return _odbahis.e_no;
                //}
                //OnErrorLogged(new Exception($"ReadHistoryAlarmMaxNumb读取失败 : {ret}"));
            }
            return -1;
        }
        /// <summary>
        /// 警报履历最小号码
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public int ReadHistoryAlarmMinNumb(ushort a, ushort b, ushort length)
        {
            if (IsConnected)
            {
                //int ret = cnc_rdalmhistry(_hander, a, b, length, _odbahis);
                //if (ret == EW_OK)
                //{
                //    return _odbahis.s_no;
                //}
                //OnErrorLogged(new Exception($"ReadHistoryAlarmMinNumb读取失败 : {ret}"));
            }
            return -1;
        }
        /// <summary>
        /// 外部操作讯息履历
        /// </summary>
        /// <returns></returns>
        public string[] ReadHistoryExOperate()
        {
            short ret = cnc_rdopmsg(_hander, 0, 6 + 256, _oPmsg);
            Type type = _oPmsg.GetType();
            if (ret == 0)
            {
                for (int i = 1; i < 6; i++)
                {
                    var str2 = "msg" + i;
                    object obj = type.GetField(str2).GetValue(_oPmsg);
                    Type type2 = obj.GetType();
                    if (Convert.ToInt16(type2.GetField("datano").GetValue(obj)) != -1)
                    {
                        if (Convert.ToInt16(type2.GetField("datano").GetValue(obj)) == 0)
                        {
                            break;
                        }
                        List<string> ss = new List<string>();
                        ss.Add($"操作信息： {type2.GetField("datano").GetValue(obj)}{type2.GetField("data").GetValue(obj)}");
                        return ss.ToArray();
                    }
                    OnLoged("无操作信息");
                }
            }
            else
            {
                OnErrorLoged(new Exception($"ReadHistoryExOperate获取失败：{ret}"));
            }
            return new string[0];
        }
        /// <summary>
        /// 外部操作讯息履历最大号码
        /// </summary>
        /// <returns></returns>
        public int ReadHistoryExOperateMaxNumb()
        {
            return -1;
        }
        /// <summary>
        /// 外部操作讯息履历最小号码
        /// </summary>
        /// <returns></returns>
        public int ReadHistoryExOperateMinNumb()
        {
            return -1;
        }

        /// <summary>
        /// 操作履历
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public string[] ReadHistoryOperate(ushort a, ushort b, ushort c)
        {
            cnc_rdophistry(_hander, a, b, c, _oDbhis);
            int numb = b - a + 1;
            string[] ss = new string[numb];
            for (int i = 1; i < numb; i++)
            {
                string time = "";
                object odtao = FanucComm.GetField(_oDbhis.data, "data" + i);
                time += FanucComm.GetField(odtao, "year").ToString().PadLeft(4, '0');
                time += "/";
                time += FanucComm.GetField(odtao, "month").ToString().PadLeft(2, '0');
                time += "/";
                time += FanucComm.GetField(odtao, "day").ToString().PadLeft(2, '0');
                time += " ";
                time += FanucComm.GetField(odtao, "hour").ToString().PadLeft(2, '0');
                time += ":";
                time += FanucComm.GetField(odtao, "minute").ToString().PadLeft(2, '0');
                time += ":";
                time += FanucComm.GetField(odtao, "second").ToString().PadLeft(2, '0');
                string message = FanucComm.GetField(odtao, "alm_msg").ToString();
                string almgrp = FanucComm.GetAlmgrp(Convert.ToInt16(FanucComm.GetField(odtao, "alm_grp")));
                var almno = FanucComm.GetField(odtao, "alm_no").ToString().PadLeft(4, '0');
                string temp = almgrp + almno;
                ss[i - 1] = (time + "|" + temp.PadRight(15, ' ') + "|" + message);
            }
            return ss;
        }
        /// <summary>
        /// 操作履历最大号码
        /// </summary>
        /// <returns></returns>
        public int ReadHistoryOperateMaxNumb()
        {

            return -1;
        }
        /// <summary>
        /// 操作履历最小号码
        /// </summary>
        /// <returns></returns>
        public int ReadHistoryOperateMinNumb()
        {
            return -1;
        }


        #endregion

        #region 加工参数相关


        /// <summary>
        /// 读取螺距误差补偿数据
        /// </summary>
        /// <param name="start">开始的编号</param>
        /// <param name="end">结束的偏号</param>
        public string Readpitchr(short start, short end)
        {
            short length = (short)(6 + end - start + 1);
            if (length > 11)
                OnErrorLoged(new Exception("Readpitchr:数据不符合"));
            else
            {
                //short a ;
                //cnc_rdpitchinfo(_hander, out a);
                short ret = cnc_rdpitchr(_hander, start, end, length, _pitch);
                if (ret == 0)
                {
                    string result = "";
                    for (short idx = 0; idx < end - start + 1; idx++)
                    {
                        //pitch.datano_s.ToString();
                        //pitch.datano_e.ToString();
                        //pitch.dummy;
                        result = $"{(idx + start).ToString()} {_pitch.data[idx]}";
                    }
                    return result;
                }
            }
            return "";
        }
        /// <summary>
        /// 刀具补偿值
        /// </summary>
        /// <returns></returns>
        public String ReadKnifeCorrect()
        {
            if (IsConnected)
            {
                cnc_rdtofsinfo(_hander, _inf);
                short a = _inf.use_no;
                short b = _inf.ofs_type;
                short ret = cnc_rdtofs(_hander, a, b, 8, _tof);
                if (ret == 0)
                {
                    return _tof.data.ToString();
                }
                OnErrorLoged(new Exception($"ReadKnifeCorrect读取失败：{ret}"));
            }
            return "";
        }
        /// <summary>
        /// 刀具补偿最大号码
        /// </summary>
        /// <returns></returns>
        public int ReadKnifeCorrectMaxNumb()
        {
            return -1;
        }
        /// <summary>
        /// 刀具补偿最小号码
        /// </summary>
        /// <returns></returns>
        public int ReadKnifeCorrectMinNumb()
        {
            return -1;
        }
        /// <summary>
        /// 刀组编号等信息
        /// </summary>
        /// <returns></returns>
        public int ReadKnifeGroupNumber()
        {
            if (IsConnected)
            {
                int m = 2;
                cnc_rdgrpid2(_hander, m, _tool);
                int groupNumer = _tool.data;
                //int all = cnc_rdngrp(_hander, tool1);//刀片组的全部数量
                //short b = Convert.ToInt16(group_numer);
                //cnc_rdntool(_hander, b, tool2);//刀具的数量
                //cnc_rdlife(_hander, b, tool2);//刀具寿命
                //cnc_rdcount(_hander, b, tool2);//刀具计时器
                //cnc_rdtoolgrp(_hander, 2, 20 + 20 * 1, btg);//根据刀组号读出所有信息，很重要；
                //cnc_rdtlusegrp(_hander, grp);//读出正在使用的到组号；
                //OnLoged($"type:{tool.type} 刀片号：{tool2.data} 刀片组号 {group_numer} 寿命：{tool2.data}");
                return groupNumer;
            }
            return -1;
        }
        /// <summary>
        /// 刀长(刀长编号)
        /// </summary>
        /// <param name="a">？</param>
        /// <param name="b">？</param>
        /// <returns></returns>
        public int ReadKnifeLength(short a, short b)
        {
            if (IsConnected)
            {
                cnc_rd1length(_hander, a, b, _tool4);
                return _tool4.data;
            }
            return -1;
        }

        /// <summary>
        /// 工件坐标补偿
        /// </summary>
        /// <returns></returns>
        public int[] ReadWpLoctionCorrect(short a, short b)
        {
            IODBWCSF Wcsf = new IODBWCSF();
            short ret = cnc_rdwkcdshft(_hander, a, b, Wcsf);
            if (ret == 0)
            {
                return Wcsf.data;
            }
            return new int[0];
        }

        /// <summary>
        /// 工件坐标补偿最大号码
        /// </summary>
        /// <returns></returns>
        public int ReadWpLoctionCorrectMaxNumb()
        {
            return -1;
        }
        /// <summary>
        /// 工件坐标补偿最小号码
        /// </summary>
        /// <returns></returns>
        public int ReadWpLoctionCorrectMinNumb()
        {
            return -1;
        }
        #endregion

        #region 获取句柄

        /// <summary>
        /// 连接设备
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口号</param>
        /// <param name="timeout">超时时间</param>
        public void Connect(string ip, int port, int timeout)
        {
            _ip = ip;
            _port = port;
            _timeout = timeout;
            int ret = cnc_allclibhndl3(ip, Convert.ToUInt16(port), timeout, out _hander);
            if (_hander != 0)
            {
                OnLoged($"[Connect] {ip} {port} {timeout} ");
            }
            ErrMsg = $"[Connect] {ip} {port} {timeout}连接失败： {ret}";
            OnErrorLogged(new Exception(ErrMsg));
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void DisConnect()
        {
            if (Convert.ToInt16(_hander) == 0) return;
            int ret = cnc_freelibhndl(_hander);
            if (ret == EW_OK)
            {
                _hander = 0;
                OnLoged($"[DisConnect] {_ip} {_port} {_timeout}");
                return;
            }
            ErrMsg = $"[DisConnect] {_ip} {_port} {_timeout} Failure:{ret}";
            OnErrorLogged(new Exception(ErrMsg));
        }

        /// <summary>
        /// 设置连接超时时间
        /// </summary>
        /// <param name="timeout">超时时长</param>
        public void SetTimeOut(int timeout)
        {
            if (Convert.ToInt16(_hander) == 0) return;
            int ret = cnc_settimeout(_hander, timeout);
            if (ret == EW_OK)
            {
                OnLoged($"[SetTimeOut] {_ip} {_port} {_timeout}");
                return;
            }
            ErrMsg = $"[SetTimeOut] {_ip} {_port} {_timeout} Failure:{ret}";
            OnErrorLogged(new Exception(ErrMsg));
        }

        #endregion




        #region 程序上传下载



        //下载程序的入口点
        /// <summary>
        /// 向CNC下载指定类型的程序
        /// </summary>
        /// <param name="handle">句柄</param>
        /// <param name="type">程序类型</param>
        /// <param name="data">程序的内容</param>
        /// <param name="odberr">保存返回错误信息的详细内容,为null不返回</param>
        /// <returns>错误码</returns>
        public short DownLoad(ushort handle, short type, string data, ODBERR odberr)
        {
            var datalength = data.Length;
            var ret = DownStart(handle, type);
            if (ret == 0)
            {
                var olddata = datalength;
                while (true)
                {
                    ret = DownLoad(handle, ref datalength, data);
                    //说明缓存已满或为空，继续尝试
                    if (ret == (short)focas_ret.EW_BUFFER)
                    {
                        continue;
                    }
                    if (ret == EW_OK)
                    {
                        //说明当前下载完成,temp记录剩余下载量
                        int temp = olddata - datalength;
                        if (temp <= 0)
                        {
                            break;
                        }
                        else
                        {
                            data = data.Substring(datalength, temp);
                            datalength = temp; olddata = temp;
                        }
                    }
                    else
                    {
                        //下载出现错误，解析出具体的错误信息
                        if (odberr != null)
                        {
                            GetDtailerr(handle, odberr);
                        }
                        //下载出错
                        break;
                    }
                }
                //判断是哪里出的问题
                if (ret == 0)
                {
                    ret = DownEnd(handle);
                    //结束下载出错
                    return ret;
                }
                else
                {
                    DownEnd(handle);
                    //下载出错
                    return ret;
                }
            }
            else
            {
                DownEnd(handle);
                //开始下载出错
                return ret;
            }
        }
        //下载程序  5-27

        //下载程序  5-27
        //开始
        private static short DownStart(ushort handle, short type)
        {
            return cnc_dwnstart3(handle, type);
        }
        //结束
        private static short DownEnd(ushort handle) =>
            cnc_dwnend3(handle);
        //下载
        //开始下载程序  datalength将会被返回，实际的输出的字符数量
        private static short DownLoad(ushort handle, ref int datalength, string data) => cnc_download3(handle, ref datalength, data);

        //获取详细的错误信息
        private static short GetDtailerr(ushort handle, ODBERR odberr) => cnc_getdtailerr(handle, odberr);


        //上传程序 5-28
        //开始
        private static short UpStart(ushort handle, short type, int startno, int endno)
        {
            return cnc_upstart3(handle, type, startno, endno);
        }
        //上传
        private static short UpLoad(ushort handle, int length, char[] databuf)
        {
            return cnc_upload3(handle, ref length, databuf);
        }
        //结束
        private static short Upend(ushort handle)
        {
            return cnc_upend3(handle);
        }
        //上传程序的入口
        /// <summary>
        /// 根据程序号读取指定程序
        /// </summary>
        /// <param name="handle">句柄</param>
        /// <param name="type">类型</param>
        /// <param name="no">程序号</param>
        /// <param name="odberr">详细错误内容，null不返回</param>
        /// <param name="data">返回的程序内容</param>
        /// <returns>错误码</returns>
        public static short UpLoad(ushort handle, short type, int no, ref string data, ODBERR odberr)
        {
            var startno = no;
            var endno = no;
            var length = 256;
            var databuf = new char[256];
            var ret = UpStart(handle, type, startno, endno);
            if (ret == EW_OK)
            {
                while (true)
                {
                    //上传
                    ret = UpLoad(handle, length, databuf);
                    var temp = new string(databuf);
                    if (ret == (short)focas_ret.EW_BUFFER)
                    {
                        continue;
                    }
                    if (ret == EW_OK)
                    {
                        temp = temp.Replace("\0", "");
                        data += temp;
                        var lastchar = temp.Substring(temp.Length - 1, 1);
                        if (lastchar == "%")
                        {
                            break;
                        }
                    }
                    else
                    {
                        //下载出现错误，解析出具体的错误信息
                        if (odberr != null)
                        {
                            GetDtailerr(handle, odberr);
                        }
                        //下载出错
                        break;
                    }
                }
                //判断是哪里出的问题
                if (ret == 0)
                {
                    ret = Upend(handle);
                    //结束上传出错
                    return ret;
                }
                else
                {
                    Upend(handle);
                    //上传出错
                    return ret;
                }
            }
            else
            {
                //开始出错
                Upend(handle);
                return ret;
            }
        }
        //上传程序 5-28
        #endregion



        private static void OnLoged(string obj)
        {
            Loged?.Invoke(obj);
        }

        private static void OnErrorLoged(Exception obj)
        {
            ErrorLoged?.Invoke(obj);
        }
    }




}
