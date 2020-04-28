using KHBC.Core.BaseModels;
using KHBC.Core.Log;
using System;
using System.Collections.Generic;

namespace KHBC.Core.Device
{
    public class ServiceCfg
    {
        public DeviceInfo DeviceInfo;
        public string ServiceName;
        public string KeyDevData;
        public string KeyDevState;
        public string KeyRemoteQueue;
        public string KeyCmdQueue;
        public IDevice DeviceApi;
    }

    public class DeviceHandler
    {
        private readonly ServiceCfg _serviceCfg;
        private bool ConnectStatus { get; set; } = false;
        private Dictionary<string, Dictionary<string, object>> LastDeviceDataValue { set; get; } = new Dictionary<string, Dictionary<string, object>>();
        private readonly BaseService _service;

        public DeviceHandler(BaseService service, DeviceInfo devInfo, IDevice deviceApi)
        {
            _service = service;
            var deviceFullName = SysConf.RunInEvoc ? $"{SysConf.KeyAssemblyLine}:{service.ServiceName}"
                                                   : $"{SysConf.KeyPlant}:{service.ServiceName}";
            _serviceCfg = new ServiceCfg
            {
                ServiceName = service.ServiceName,
                DeviceInfo = devInfo,
                KeyDevData = $"{deviceFullName}:DC",
                KeyDevState = $"{deviceFullName}:ST",
                KeyRemoteQueue = $"{SysConf.KeyPlant}:BDCI:Q",
                KeyCmdQueue = $"{deviceFullName}:CMD:Q",
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

            if (_serviceCfg.DeviceApi.ConnectStatus)
            {
                if (_serviceCfg.DeviceInfo.Addresses != null)
                {
                    var devicesDataValue = new Dictionary<string, Dictionary<string, object>>();
                    var currDataValue = new Dictionary<string, object>();

                    foreach (var val in _serviceCfg.DeviceInfo.Addresses)
                       
                        {
                        var addr = val.Value;
                            
                        bool ret = false;
                        var data = new object();

                        switch (addr.DataType.ToUpper())
                        {
                            case "API":
                                //2020-4-8 Tony modified
                                if (_serviceCfg.DeviceInfo.DeviceModel == "OKUMA")
                                {
                                    foreach (var pro in _serviceCfg.DeviceInfo.Properties)
                                    {
                                        var property = pro.Value;
                                        ret = _serviceCfg.DeviceApi.Read<Dictionary<string, object>>(property.Address, property.Id, ref data);
                                    }
                                    break;
                                }
                                else
                                {
                                    ret = _serviceCfg.DeviceApi.Read<Dictionary<string, object>>(addr.Address, 0, ref data);
                                    break;
                                }
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

                        UpdateStatus(true);
                        currDataValue[addr.Id] = data;
                    }

                    ParseDataInfo(_serviceCfg.DeviceInfo, currDataValue, ref devicesDataValue);
                    if (devicesDataValue.Count > 0)
                    {
                        UpdateDeviceInfo(devicesDataValue);
                    }
                }
            }

            if (_serviceCfg.DeviceInfo.Controllable)
            {
                try
                {
                    var cmd = _service.Pop<string>(_serviceCfg.KeyCmdQueue);
                    if (!string.IsNullOrWhiteSpace(cmd))
                    {
                        Logger.Main.Info($"[{_serviceCfg.ServiceName}]收到控制命令: {cmd}");
                        _serviceCfg.DeviceApi.ExeCmd(cmd);
                    }
                }
                catch (Exception)
                {
                    //Logger.Main.Warn($"[{ServiceName}]RedisClientLocal.BRPop异常: {ex.Message}");
                }
            }
        }

        private void ParseDataInfo(DeviceInfo deviceInfo, Dictionary<string, object> currDataValue, ref Dictionary<string, Dictionary<string, object>> devicesDataValue)
        {
            if (deviceInfo.Properties != null && deviceInfo.Properties.Count > 0)
            {
                var devPropertyValue = new Dictionary<string, object>();

                if (!LastDeviceDataValue.ContainsKey(deviceInfo.Id))
                {
                    LastDeviceDataValue[deviceInfo.Id] = new Dictionary<string, object>();
                }
                foreach (var p in deviceInfo.Properties)
                {
                    if (currDataValue.ContainsKey(p.Value.AddressId))
                    {
                        bool chg = !LastDeviceDataValue[deviceInfo.Id].ContainsKey(p.Value.Id);

                        switch (p.Value.DataType?.ToUpper())
                        {
                            case "BIT":
                                var bitValues = (byte[])currDataValue[p.Value.AddressId];
                                var bitVal = Convert.ToUInt32(bitValues[p.Value.AddressValueOffset]);
                                devPropertyValue[p.Value.Id] = (bitVal >> p.Value.AddressBitOffset) & 1;
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
                            _service.SetAsync($"{_serviceCfg.KeyDevData}:{p.Value.Id}", devPropertyValue[p.Value.Id]);
                            if (p.Value.AttrType?.ToUpper() == "EVENT")
                            {
                                SendAlaram(deviceInfo.Id, p.Value.Id, devPropertyValue[p.Value.Id]);
                            }
                            else
                            {
                                if (!devicesDataValue.ContainsKey(deviceInfo.Id))
                                {
                                    devicesDataValue[deviceInfo.Id] = new Dictionary<string, object>();
                                }
                                devicesDataValue[deviceInfo.Id][p.Value.Id] = devPropertyValue[p.Value.Id];
                                LastDeviceDataValue[deviceInfo.Id][p.Value.Id] = devPropertyValue[p.Value.Id];
                            }
                        }
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

        private void SendAlaram(string deviceId, string alarmId, object alarmVlaue)
        {
            var obj = InitMessageObject("ALARM", "Add");
            obj.Data[deviceId] = new Dictionary<string, object>
            {
                [alarmId] = alarmVlaue
            };
            _service.Push(_serviceCfg.KeyRemoteQueue, obj);
        }

        private void UpdateStatus(bool status)
        {
            if (ConnectStatus == status)
            {
                return;
            }

            ConnectStatus = status;
            SendAlaram(_serviceCfg.DeviceInfo.Id, "ConnectStatus", ConnectStatus);
        }

        private void UpdateDeviceInfo(Dictionary<string, Dictionary<string, object>> devicesDataValue)
        {
            //Tony modified 2020-4-10 
            //var obj = InitMessageObject(SysConf.ModuleName, "Add");
            var obj = InitMessageObject(SysConf.ModuleName, "Update");
            obj.Data = devicesDataValue;
            _service.Push(_serviceCfg.KeyRemoteQueue, obj);
           
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
