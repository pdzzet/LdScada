using KHBC.Core;
using KHBC.Core.Log;
using KHBC.LD.DTO;
using KHBC.Modbus.WorkFlows;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KHBC.Modbus
{
    /// <summary>
    /// 公共数据
    /// </summary>
    public static class ModBusDataFactory
    {
        //当前的生产工位派工单
        public static readonly ConcurrentQueue<RMesDispatch> dispatrchs = new ConcurrentQueue<RMesDispatch>();
        //当前的质量单
        public static readonly ConcurrentQueue<RQualityResult> qualityResults = new ConcurrentQueue<RQualityResult>();
        //当前的线体状态 0 空闲 1 忙碌
        public static int LineReady = 0;
        //当前的线体Line
        public static string OrderLine;

        public static readonly Dictionary<string, string> ActionDict = new Dictionary<string, string>();
        public static readonly Dictionary<string, MethodInfo> MethdDict = new Dictionary<string, MethodInfo>();
        public static PlcConfig PlcConfig { get; private set; }
        public static WorkFolwConfig FlowConfig { get; private set; }
        public static readonly List<WorkFlowEngine> workFlowEngines = new List<WorkFlowEngine>();
        private static List<ModBusBase> modBuses = new List<ModBusBase>();

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialization(PlcConfig plc, WorkFolwConfig flow)
        {
            PlcConfig = plc;
            FlowConfig = flow;

            var modBuses = new List<Tuple<string, ModBusBase, Dictionary<string, PlcAddress>>>();
            if (flow?.WorkFlows != null && flow?.WorkFlows?.Length > 0 && plc?.PlcDevices != null && plc?.PlcDevices?.Count > 0)
            {
                //创建设备对象和地址
                foreach (var device in plc.PlcDevices)
                {
                    ModBusBase modBus;
                    var modbusmodel = ModbusConf.CnfModel.FirstOrDefault(x => x.Name == device.Name);
                    if (modbusmodel == null)
                        continue;
                    if (modbusmodel.ConnectMode == Enums.ConnectMode.MODBUSRTU)
                        modBus = new ModbusRtuClient(modbusmodel.PortName, modbusmodel.BaudRate, modbusmodel.Station, modbusmodel.IsAddressStartWithZero, modbusmodel.DataBits, modbusmodel.StopBits, modbusmodel.Parity);
                    else
                        modBus = new ModbusTcp(modbusmodel.IpAddress, modbusmodel.Port, modbusmodel.Station, modbusmodel.IsAddressStartWithZero);
                    modBus.Name = device.Name;
                    //地址
                    var addressdict = new Dictionary<string, PlcAddress>();
                    if (device.Addresses != null)
                        foreach (var address in device.Addresses)
                            addressdict[address.Name] = address;
                    else
                        addressdict = new Dictionary<string, PlcAddress>();
                    modBuses.Add(new Tuple<string, ModBusBase, Dictionary<string, PlcAddress>>(modBus.Name, modBus, addressdict));
                }
                //创建工作流
                foreach (var work in flow.WorkFlows)
                {
                    var modbus = modBuses.FirstOrDefault(x => x.Item1 == work.PlcDeviceName);
                    if (modbus == null)
                    {
                        Logger.Device.Info($"WorkFlow:{work.Name} Not Found PlcDeviceName:{work.PlcDeviceName}");
                        continue;
                    }
                    var engine = new WorkFlowEngine(work, modbus.Item2, modbus.Item3);
                    workFlowEngines.Add(engine);
                    ServiceManager.Services.Add(engine);
                }
            }
        }
    }
}
