using System;
using KHBC.Modbus.Enums;

namespace KHBC.Modbus
{   
    /// <summary>
    ///   配置类
    /// </summary>
    public class ModbusConf
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        public static ModbusConfModel[] CnfModel;
        /// <summary>
        /// sql日志
        /// </summary>
        public static event Action<string> DbSqlLoged;
        /// <summary>
        /// sql错误日志
        /// </summary>
        public static event Action<string> DbSqlErrorLoged;

        /// <summary>
        /// 输出sql执行日志
        /// </summary>
        /// <param name="msg"></param>
        internal static void WriteLog(string msg)
        {
            DbSqlLoged?.Invoke(msg);
        }

        /// <summary>
        /// 输出错误日志
        /// </summary>
        /// <param name="ex"></param>
        internal static void WriteErrorLog(string ex)
        {
            DbSqlErrorLoged?.Invoke(ex);
        }
    }

    /// <summary>
    /// modbus配置实体
    /// </summary>
    public class ModbusConfModel
    {
        /// <summary>
        /// 设备名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 连接模式
        /// </summary>
        public ConnectMode ConnectMode { get; set; } = ConnectMode.MODBUSTCP;
        /// <summary>
        /// 站点号
        /// </summary>
        public byte Station { get; set; } = 1;
        /// <summary>
        /// 地址是否从零开始
        /// </summary>
        public bool IsAddressStartWithZero { get; set; } = true;

        #region TCP
        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; } = "127.0.0.1";
        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; set; } = 502;
        #endregion

        #region RTU
        /// <summary>
        /// 端口名称
        /// </summary>
        public string PortName { get; set; } = "COM1";
        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate { get; set; } = 9600;
        /// <summary>
        /// 数据位
        /// </summary>
        public int DataBits { get; set; } = 8;
        /// <summary>
        /// 停止位  不定义则为0
        /// </summary>
        public int StopBits { get; set; }
        /// <summary>
        /// 奇偶校验 为0不校验
        /// </summary>
        public int Parity { get; set; }
        #endregion
    }
}
