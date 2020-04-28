using KHBC.Core;
using KHBC.Core.Device;
using KHBC.Core.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using KHBC.Core.BaseModels;

namespace KHBC.LD.APCM
{
    public class ServiceCfg
    {
        public DeviceInfo DeviceInfo;
        public string ServiceName;
        public IDevice DeviceApi;
    }

    public class DeviceHandler
    {
        public string SubModuleName = "EQ";
        private readonly ServiceCfg _serviceCfg;
        private long lastTime;
        private readonly BaseService _service;
        private bool ConnectStatus { get; set; } = false;
        private Dictionary<string, Dictionary<string, object>> LastDeviceDataValue { set; get; } = new Dictionary<string, Dictionary<string, object>>();

        public DeviceHandler(BaseService service, DeviceInfo devInfo, IDevice deviceApi)
        {
            _service = service;
            _serviceCfg = new ServiceCfg
            {
                ServiceName = service.ServiceName,
                DeviceInfo = devInfo,
                DeviceApi = deviceApi
            };

            _serviceCfg.DeviceApi.Connect();
        }


        ~DeviceHandler()
        {
            _serviceCfg.DeviceApi.Disconnect();
        }

        public void Process()
        {
            long curTime = DateTime.Now.Ticks;

            if (!_serviceCfg.DeviceApi.ConnectStatus)
            {
                _serviceCfg.DeviceApi.Connect();
            }
            
            /// AttrType=Event  按时读取
            if (_serviceCfg.DeviceApi.ConnectStatus &&
                ((curTime - lastTime) / 10000) >= _serviceCfg.DeviceInfo.PollingTime)
            {
                if (_serviceCfg.DeviceInfo.Addresses != null)
                {
                    var devicesDataValue = new Dictionary<string, Dictionary<string, object>>();
                    var currDataValue = new Dictionary<string, object>();

                    foreach (var val in _serviceCfg.DeviceInfo.Addresses.Where(x => x.Value.AttrType?.ToUpper() == "EVENT"))
                    {
                        var addr = val.Value;
                        bool ret = false;
                        var data = new object();

                        switch (addr.DataType.ToUpper())
                        {
                            case "API":
                                ret = _serviceCfg.DeviceApi.Read<Dictionary<string, object>>(null, 0, ref data);
                                break;
                            case "BYTE":
                                ret = _serviceCfg.DeviceApi.Read<byte[]>(addr.Address, addr.Length, ref data);
                                break;
                            case "BOOL":
                                ret = _serviceCfg.DeviceApi.Read<bool[]>(addr.Address, addr.Length, ref data);
                                break;
                            case "FLOAT":
                                ret = _serviceCfg.DeviceApi.Read<float[]>(addr.Address, addr.Length, ref data);
                                break;
                            case "UINT16":
                                ret = _serviceCfg.DeviceApi.Read<UInt16[]>(addr.Address, addr.Length, ref data);
                                break;
                        }

                        if (!ret)
                        {
                            Logger.Main.Warn($"[{_serviceCfg.ServiceName}] Read(\"{addr.Address}\", {addr.DataType}, {addr.Length})失败: {_serviceCfg.DeviceApi.ErrorCode}, {_serviceCfg.DeviceApi.Message}");
                            UpdateStatus(false);
                            return;
                        }
                        Logger.Main.Warn($"[{_serviceCfg.ServiceName}] Read(\"{addr.Address}\", {addr.DataType}, {addr.Length})成功: {_serviceCfg.DeviceApi.ErrorCode}, {_serviceCfg.DeviceApi.Message}");
                        UpdateStatus(true);
                        currDataValue[addr.Id] = data;
                        //ParseDataInfo(_serviceCfg.DeviceInfo, currDataValue, ref devicesDataValue);
                    }

                   ParseDataInfo(_serviceCfg.DeviceInfo, currDataValue, ref devicesDataValue);

                    lastTime = DateTime.Now.Ticks;
                }
            }

            if (ApcmEventConfig.CQ.TryDequeue(out ApcmEventArgs action))
            {
                switch (action.Action.ToUpper())
                {
                    case "READ":
                        if (_serviceCfg.DeviceInfo.Properties.ContainsKey(action.Address))
                        {
                            var p = _serviceCfg.DeviceInfo.Properties[action.Address];
                            var addr = Regex.Replace(p.Address, @"^[%a-zA-Z]+", string.Empty);
                            var data = new object();
                            var ret = _serviceCfg.DeviceApi.Read<byte[]>(addr, (int)p.Length, ref data);
                            if (!ret)
                            {
                                Logger.Main.Warn($"[{_serviceCfg.ServiceName}] Read(\"{p.Address}\", {p.DataType}, {addr.Length})失败: {_serviceCfg.DeviceApi.ErrorCode}, {_serviceCfg.DeviceApi.Message}");
                                UpdateStatus(false);
                            }
                            else
                            {
                                UpdateStatus(true);
                                DispatchAvc(action.EventSrc, p.Id, data);
                            }
                        }
                        break;
                    case "WRITE":
                        if (_serviceCfg.DeviceInfo.Properties.ContainsKey(action.Address))
                        {
                            var p = _serviceCfg.DeviceInfo.Properties[action.Address];
                            if (p.AttrType?.ToUpper() == "ACTION")
                            {
                                var addr = Regex.Replace(p.Address, @"^[%a-zA-Z]+", string.Empty);
                                var valAddr = addr;
                                var offAddr = 0;
                                var arr = addr.Split('.');
                                if (arr.Length == 2)
                                {
                                    valAddr = arr[0];
                                    offAddr = Convert.ToInt32(arr[1]);
                                }

                                bool ret = false;
                                var data = new object();
                                if (p.DataType == "BIT")
                                {
                                    ret = _serviceCfg.DeviceApi.Read<UInt16[]>(valAddr, 1, ref data);
                                    if (!ret)
                                    {
                                        Logger.Main.Warn($"[{_serviceCfg.ServiceName}] Read(\"{valAddr}\", {p.DataType}, {addr.Length})失败: {_serviceCfg.DeviceApi.ErrorCode}, {_serviceCfg.DeviceApi.Message}");
                                        UpdateStatus(false);
                                    }
                                    else
                                    {
                                        UpdateStatus(true);
                                        UInt16 bitValues = (UInt16)data;
                                        var value = (int)action.Data;
                                        if (value == 0)
                                        {
                                            bitValues &= (UInt16)(~(1 << (8 + offAddr)));
                                        }
                                        else
                                        {
                                            bitValues |= (UInt16)(1 << (8 + offAddr));
                                        }

                                        var byteData = new byte[2];
                                        byteData[0] = (byte)(bitValues >> 8);
                                        byteData[1] = (byte)(bitValues >> 0);
                                        ret = _serviceCfg.DeviceApi.Write<byte[]>(valAddr, byteData);
                                        if (!ret)
                                        {
                                            Logger.Main.Warn($"[{_serviceCfg.ServiceName}] Write(\"{p.Address}, {valAddr}\", {p.DataType}, {valAddr.Length})失败: {_serviceCfg.DeviceApi.ErrorCode}, {_serviceCfg.DeviceApi.Message}");
                                            UpdateStatus(false);
                                        }
                                        else
                                        {
                                            UpdateStatus(true);
                                        }
                                    }
                                }
                                else if (p.DataType == "BYTE")
                                {
                                    ret = _serviceCfg.DeviceApi.Write<byte[]>(addr, action.Data);
                                    if (!ret)
                                    {
                                        Logger.Main.Warn($"[{_serviceCfg.ServiceName}] Write(\"{p.Address}\", {p.DataType}, {addr.Length})失败: {_serviceCfg.DeviceApi.ErrorCode}, {_serviceCfg.DeviceApi.Message}");
                                        UpdateStatus(false);
                                    }
                                    else
                                    {
                                        UpdateStatus(true);
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }

        private void HandlePropertyRule(DeviceProperty p, string value)
        {
            if (p.Trigger == null || p.Trigger == "")
            {
                return;
            }

            var match = Regex.Match(p.Trigger, @"^(.*?):([RW]):(.*?)$", RegexOptions.Singleline);
            if (match.Success)
            {
                var vv = match.Groups[1].Value;
                var op = match.Groups[2].Value;
                var pr = match.Groups[3].Value;
                if (vv == value)
                {
                    ApcmEventConfig.CQ.Enqueue(new ApcmEventArgs
                    {
                        EventSrc = "WF",
                        Address = pr,
                        Action = op == "W" ? "WRITE" : "READ"
                    });
                }
            }
        }

        private void ParseDataInfo(DeviceInfo deviceInfo, Dictionary<string, object> currDataValue, ref Dictionary<string, Dictionary<string, object>> devicesDataValue)
        {

            var devPropertyValue = new Dictionary<string, object>();
            if (!LastDeviceDataValue.ContainsKey(deviceInfo.Id))
            {
                LastDeviceDataValue[deviceInfo.Id] = new Dictionary<string, object>();
            }

            foreach (var p in deviceInfo.Properties.Where(x => x.Value.AttrType.ToUpper() == "EVENT"))
            {
                if (currDataValue.ContainsKey(p.Value.AddressId))
                {
                    bool chg = !LastDeviceDataValue[deviceInfo.Id].ContainsKey(p.Value.Id);

                    switch (p.Value.DataType?.ToUpper())
                    {
                        case "BIT":
                            var bitValues = (byte[])currDataValue[p.Value.AddressId];
                            var bitVal = Convert.ToUInt32(bitValues[p.Value.AddressValueOffset]);
                            devPropertyValue[p.Value.Id] = (bitVal >> p.Value.AddressBitOffset) & (~(0xFFFFFFFF << (int)p.Value.Length));
                            chg = chg ? chg : (UInt32)LastDeviceDataValue[deviceInfo.Id][p.Value.Id] != (UInt32)devPropertyValue[p.Value.Id];
                            break;
                        case "BYTE":
                            var byteValues = (byte[])currDataValue[p.Value.AddressId];
                            var byteVal = byteValues[p.Value.AddressValueOffset];
                            devPropertyValue[p.Value.Id] = byteVal;
                            chg = chg ? chg : (byte)LastDeviceDataValue[deviceInfo.Id][p.Value.Id] != (byte)devPropertyValue[p.Value.Id];
                            break;
                        case "BOOL":
                            var boolValues = (bool[])currDataValue[p.Value.AddressId];
                            var boolVal = boolValues[p.Value.AddressValueOffset];
                            devPropertyValue[p.Value.Id] = boolVal;
                            chg = chg ? chg : (bool)LastDeviceDataValue[deviceInfo.Id][p.Value.Id] != (bool)devPropertyValue[p.Value.Id];
                            break;
                        case "FLOAT":
                            var floatValues = (float[])currDataValue[p.Value.AddressId];
                            var floatVal = floatValues[p.Value.AddressValueOffset];
                            devPropertyValue[p.Value.Id] = floatVal;
                            chg = chg ? chg : (float)LastDeviceDataValue[deviceInfo.Id][p.Value.Id] != (float)devPropertyValue[p.Value.Id];
                            break;
                        case "UINT16":
                            var uint16Values = (UInt16[])currDataValue[p.Value.AddressId];
                            var uint16Val = uint16Values[p.Value.AddressValueOffset];
                            devPropertyValue[p.Value.Id] = uint16Val;
                            chg = chg ? chg : (UInt16)LastDeviceDataValue[deviceInfo.Id][p.Value.Id] != (UInt16)devPropertyValue[p.Value.Id];
                            break;
                        case "DICT":
                            var values = (Dictionary<string, object>)currDataValue[p.Value.AddressId];
                            devPropertyValue[p.Value.Id] = values[p.Value.Id];
                            chg = chg ? chg : LastDeviceDataValue[deviceInfo.Id][p.Value.Id].ToJsonStr() != devPropertyValue[p.Value.Id].ToJsonStr();
                            break;
                        default:
                            break;
                    }

                    if (chg)
                    {
                        if (!devicesDataValue.ContainsKey(deviceInfo.Id))
                        {
                            devicesDataValue[deviceInfo.Id] = new Dictionary<string, object>();
                        }

                        devicesDataValue[deviceInfo.Id][p.Value.Id] = devPropertyValue[p.Value.Id];
                        LastDeviceDataValue[deviceInfo.Id][p.Value.Id] = devPropertyValue[p.Value.Id];

                        HandlePropertyRule(p.Value, devPropertyValue[p.Value.Id].ToString());
                        DispatchAvc("WF", p.Value.Id, devPropertyValue[p.Value.Id]);
                    }
                }
            }

            if (deviceInfo.Devices != null)
            {
                foreach (var d in deviceInfo.Devices)
                {
                    ParseDataInfo(d.Value, currDataValue, ref devicesDataValue);
                }
            }
        }

        private void DispatchAvc(string evtSrc, string key, object value)
        {
            _service.SetAsync($"{ApcmKeyConf.DataCollection}:{key}", value);
            ApcmEventConfig.TriggerEvent(evtSrc, key, value);
        }

        private void SendAlaram(string alarmId, object alarmVlaue)
        {
            var obj = InitMessageObject("ALARM", "Add");
            obj.Data[_serviceCfg.DeviceInfo.Id] = new Dictionary<string, object>();
            obj.Data[_serviceCfg.DeviceInfo.Id]["AlarmId"] = alarmId;
            obj.Data[_serviceCfg.DeviceInfo.Id]["AlarmValue"] = alarmVlaue;

            _service.SetAsync(ApcmKeyConf.BdciQueue, obj);
        }

        private void UpdateStatus(bool status)
        {
            if (ConnectStatus == status)
            {
                return;
            }

            ConnectStatus = status;
            SendAlaram("ConnectStatus", ConnectStatus);
        }

        private MessageDataObject InitMessageObject(string destModule, string actionName)
        {
            var obj = new MessageDataObject
            {
                ModuleName = SysConf.ModuleName,
                DestModule = destModule,
                ActionName = actionName,
                ServiceName = _serviceCfg.ServiceName,
                PlantId = SysConf.Main.Plant.Id,
                AssemblyLineId = SysConf.Main.AssemblyLine.Id
            };

            return obj;
        }
    }
}
