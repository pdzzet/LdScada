using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using KHBC.Modbus.Enums;

namespace KHBC.Modbus
{
    /// <summary>
    /// plc配置类
    /// </summary>
    [Serializable]
    [XmlRoot("PlcConfig")]
    public class PlcConfig
    {
        public List<PlcDevice> PlcDevices { get; set; }
    }

    /// <summary>
    /// 设备信息
    /// </summary>
    [Serializable]
    public class PlcDevice
    {
        /// <summary>
        /// 设备名
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }
        /// <summary>
        /// 地址列表
        /// </summary>
        [XmlElement("Address")]
        public List<PlcAddress> Addresses { get; set; }
    }

    /// <summary>
    /// plc地址信息
    /// </summary>
    [Serializable]
    public class PlcAddress
    {
        /// <summary>
        /// 地址名称
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }
        /// <summary>
        /// 数据地址
        /// </summary>
        [XmlAttribute]
        public string Address { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        [XmlAttribute]
        public PlcDataType DataType { get; set; }
        /// <summary>
        /// 地址长度
        /// </summary>
        [XmlAttribute]
        public int Len { get; set; } = 1;
    }




}
