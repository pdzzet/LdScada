using KHBC.Core;
using KHBC.Core.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace KHBC.Core.Device
{
    #region 设备配置XML文件定义
    /// <summary>
    /// 设备配置类
    /// </summary>
    [Serializable]
    [XmlRoot("DeviceConfig")]
    public class DeviceConfig
    {
        [XmlElement("Device")]
        public List<DeviceInfo> XmlDevices { get; set; }

        [XmlIgnore]
        public Dictionary<string, DeviceInfo> Devices { get; set; } = null;


        [XmlElement("Template")]
        public List<DeviceInfo> XmlTemplates { get; set; }

        [XmlIgnore]
        public Dictionary<string, DeviceInfo> Templates { get; set; } = null;

        private Dictionary<string, DeviceAddressInfo> RecombineAddress(List<DeviceAddressInfo> xmlAddresses)
        {
            if (xmlAddresses == null || xmlAddresses.Count == 0)
            {
                return null;
            }

            var ads = new Dictionary<string, DeviceAddressInfo>();
            foreach (var addr in xmlAddresses)
            {
                if (addr.Id != null && addr.Id != "")
                {
                    ads[addr.Id] = addr;
                }
            }

            return ads;
        }

        private Dictionary<string, DeviceProperty> RecombineProperties(Dictionary<string, DeviceAddressInfo> tplAddress, List<DeviceProperty> xmlProperties)
        {
            try
            {
                if (xmlProperties != null && xmlProperties.Count > 0)
                {
                    var propertyDict = new Dictionary<string, DeviceProperty>();

                    foreach (var p in xmlProperties)
                    {
                        if (p.DataType.ToUpper() == "DICT")
                        {
                            //Tony -2020-4-1
                            propertyDict[p.Id] = p;

                            continue;
                        }

                        try
                        {
                            var addr = Regex.Replace(p.Address, @"^[%A-Za-z]+", string.Empty);
                            var div = 1;
                            var bitAddr = addr.Split('.');
                            var match = Regex.Match(bitAddr[0], @"(\d+)\[(\d+)\]", RegexOptions.Singleline);
                            if (match.Success)
                            {
                                p.AddressValueOffset = Convert.ToInt32(match.Groups[1].Value) + Convert.ToInt32(match.Groups[2].Value);
                            }
                            else
                            {
                                p.AddressValueOffset = Convert.ToInt32(bitAddr[0]);
                            }
                            p.AddressBitOffset = 0;
                            if (bitAddr.Length == 2)
                            {
                                p.AddressBitOffset = Convert.ToInt32(bitAddr[1]);
                            }

                            if (tplAddress != null && tplAddress.ContainsKey(p.AddressId))
                            {
                                var tplAddr = Regex.Replace(tplAddress[p.AddressId].Address, @"^[%A-Za-z]+", string.Empty);
                                p.AddressValueOffset -= Convert.ToInt32(tplAddr);
                            }
                            p.AddressValueOffset /= div;

                            propertyDict[p.Id] = p;
                        }
                        catch (Exception ex)
                        {
                            Logger.Main.Error(ex.Message);
                        }
                    }

                    return propertyDict;
                }


            }
            catch (Exception ex)
            {
                Logger.Main.Error(ex.Message);
            }
            return null;
        }

        private Dictionary<string, DeviceInfo> RecombineDevice(List<DeviceInfo> xmlDevices)
        {
            if (xmlDevices == null || xmlDevices.Count == 0)
            {
                return null;
            }

            var devices = new Dictionary<string, DeviceInfo>();
            // 处理设备列表
            foreach (var xd in xmlDevices)
            {
                var d = xd;
                if (d.Id != null && d.Id != "")
                {
                    d.Addresses = RecombineAddress(d.XmlAddresses);
                    d.Devices = RecombineDevice(d.XmlDevices);
                    d.Properties = RecombineProperties(null, d.XmlProperties);

                    // 主设备
                    if (d.Template != null &&
                        d.Template != "" &&
                        Templates != null &&
                        Templates.ContainsKey(d.Template))
                    {
                        d.Properties = RecombineProperties(Templates[d.Template].Addresses, d.XmlProperties);

                        var t = new DeviceInfo();
                        DataObject.Mapper<DeviceInfo, DeviceInfo>(ref t, Templates[d.Template], false);
                        DataObject.Mapper<DeviceInfo, DeviceInfo>(ref t, d, true);
                        devices[d.Id] = t;
                    }
                    else
                    {
                        devices[d.Id] = d;
                    }
                }
            }

            return devices;
        }

        public void Recombine()
        {
            try
            {


                if (XmlTemplates.Count > 0)
                {
                    Templates = new Dictionary<string, DeviceInfo>();
                    // 处理模板
                    foreach (var t in XmlTemplates)
                    {
                        if (t.Id != null && t.Id != "")
                        {
                            t.Addresses = RecombineAddress(t.XmlAddresses);
                            t.Properties = RecombineProperties(t.Addresses, t.XmlProperties);
                            Templates[t.Id] = t;
                        }
                    }
                }

                Devices = RecombineDevice(XmlDevices);
            }
            catch (Exception ex)
            {
                Logger.Main.Error($"{ex.Message}");
            }
        }
    }

    /// <summary>
    /// 设备信息
    /// </summary>
    [Serializable]
    public class DeviceInfo
    {
        [XmlAttribute]
        public string Id { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// 车间ID
        /// </summary>
        [XmlAttribute]
        public string PlantId { get; set; }

        /// <summary>
        /// 产线ID
        /// </summary>
        [XmlAttribute]
        public string AssemblyLineId { get; set; }

        /// <summary>
        /// 设备型号
        /// </summary>
        [XmlAttribute]
        public string DeviceModel { get; set; }

        /// <summary>
        /// 是否可以控制
        /// </summary>
        [XmlAttribute]
        public bool Controllable { get; set; } = true;

        /// <summary>
        /// 设备IP地址
        /// </summary>
        [XmlAttribute]
        public string IpAddress { get; set; } = null;

        /// <summary>
        /// 设备网络端口
        /// </summary>
        [XmlAttribute]
        public int Port { get; set; }

        /// <summary>
        /// 等待间隔
        /// </summary>
        [XmlAttribute]
        public int PollingTime { get; set; }

        /// <summary>
        /// 关联模板
        /// </summary>
        [XmlAttribute]
        public string Template { get; set; }

        /// <summary>
        /// 地址列表
        /// </summary>
        [XmlElement("Address")]
        public List<DeviceAddressInfo> XmlAddresses;

        [XmlIgnore]
        public Dictionary<string, DeviceAddressInfo> Addresses { get; set; } = null;

        /// <summary>
        /// 设备属性列表
        /// </summary>
        [XmlElement("Property")]
        public List<DeviceProperty> XmlProperties { get; set; }
        [XmlIgnore]
        public Dictionary<string, DeviceProperty> Properties { get; set; } = null;

        /// <summary>
        /// 子设备列表
        /// </summary>
        [XmlElement("Device")]
        public List<DeviceInfo> XmlDevices { get; set; }

        [XmlIgnore]
        public Dictionary<string, DeviceInfo> Devices { get; set; } = null;

    }

    /// <summary>
    /// 设备属性信息
    /// </summary>
    [Serializable]
    public class DeviceProperty
    {
        /// <summary>
        /// Id
        /// </summary>
        [XmlAttribute]
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// 属性类型
        /// </summary>
        [XmlAttribute]
        public string AttrType { get; set; } = "PROPERTY";

        /// <summary>
        /// 地址名称
        /// </summary>
        [XmlAttribute]
        public string AddressName { get; set; }

        /// <summary>
        /// 地址ID
        /// </summary>
        [XmlAttribute]
        public string AddressId { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [XmlAttribute]
        public string Address { get; set; }

        /// <summary>
        /// 地址值位置
        /// </summary>
        [XmlIgnore]
        public int AddressValueOffset { get; set; } = 0;

        /// <summary>
        /// 地址值位位置
        /// </summary>
        [XmlIgnore]
        public int AddressBitOffset { get; set; } = 0;

        /// <summary>
        /// Alarm
        /// </summary>
        [XmlAttribute]
        public bool Alarm { get; set; } = false;

        /// <summary>
        /// 类型
        /// </summary>
        [XmlAttribute]
        public string DataType { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        [XmlAttribute]
        public uint Length { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        [XmlAttribute]
        public string DefValue { get; set; }

        /// <summary>
        /// 触发器
        /// </summary>
        [XmlAttribute]
        public string Trigger { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute]
        public string Descr { get; set; }
    }

    /// <summary>
    /// 设备地址信息
    /// </summary>
    [Serializable]
    public class DeviceAddressInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute]
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [XmlAttribute]
        public string Address { get; set; }

        /// <summary>
        /// 属性
        /// </summary>
        [XmlAttribute]
        public string AttrType { get; set; } = "PROPERTY";

        /// <summary>
        /// 地址值
        /// </summary>
        [XmlAttribute]
        public uint AddressValue { get; set; } = 0;

        /// <summary>
        /// 内容位置
        /// </summary>
        [XmlAttribute]
        public uint ValueOffset { get; set; } = 0;

        /// <summary>
        /// 数据类型
        /// </summary>
        [XmlAttribute]
        public string DataType { get; set; } = "BYTE";

        /// <summary>
        /// 长度
        /// </summary>
        [XmlAttribute]
        public int Length { get; set; } = 1;
    }
    #endregion
}
