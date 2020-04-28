using KHBC.Core;
using KHBC.Core.BaseModels;
using KHBC.Core.Extend;
using KHBC.Core.FrameBase;
using KHBC.Core.Log;
using KHBC.Modbus.Enums;
using KHBC.Modbus.WorkFlows;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace KHBC.Modbus
{
    public class Startup : BaseStartup
    {
        public override void BeforeStartup()
        {
            // 创建消息监听服务
            ServiceManager.Services.Add(new MsgRecvService());

            //注册日志
            WorkFlowEngine.Loged += Logger.Device.Info;
            WorkFlowEngine.ErrorLoged += Logger.Device.Error;

            //加载设备配置
            var device = Path.Combine(SysConf.ConfigPath, $"{SysConf.ProcessName}.device.json");
            ModbusConf.CnfModel = JsonExtension.GetDefKey(device, "Modbus", new ModbusConfModel[] { });
            //加载配置文件
            var plccfgPath = Path.Combine(SysConf.ConfigPath, "PLCConfig.xml");
            var workflowPath = Path.Combine(SysConf.ConfigPath, "WorkFlow.xml");
            if (File.Exists(plccfgPath) && File.Exists(workflowPath))
            {
                var plc = XmlExtension.LoadXMLFile<PlcConfig>(plccfgPath);
                var work = XmlExtension.LoadXMLFile<WorkFolwConfig>(workflowPath);
                if (!plc.IsSuccess)
                    Logger.Main.Info("加载modbusplc配置失败");
                if (!work.IsSuccess)
                    Logger.Main.Info("加载modbusworkflow配置失败");
                if (plc.IsSuccess && work.IsSuccess)
                {
                    ModBusDataFactory.Initialization(plc.Data, work.Data);
                }
            }
            else
            {
                Example();
            }


        }

        public override List<Registrations> Register()
        {
            return new List<Registrations>
            {
                new Registrations(null, true, typeof(IActionHandle))
            };
        }

        public override void AfterStartup()
        {
            //获取并缓存所有方法
            var handlers = Common.GetAllInstance<IActionHandle>();
            foreach (var handle in handlers)
            {
                var t = handle.GetType();
                var methods = t.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(x => !x.IsSpecialName);
                foreach (var m in methods)
                {
                    ModBusDataFactory.ActionDict[m.Name.ToUpper()] = t.Name;
                    ModBusDataFactory.MethdDict[m.Name.ToUpper()] = m;
                }
            }

        }

        private void Example()
        {
            var plccfg = Path.Combine(SysConf.ConfigPath, "ExamplePLCConfig.xml");
            var workflowcfg = Path.Combine(SysConf.ConfigPath, "ExampleWorkFlow.xml");
            var plcdevice = new PlcDevice()
            {
                Name = "测试设备",
                Addresses = new List<PlcAddress>
                {
                   new PlcAddress{ Name="监听位1",Address="1", DataType=PlcDataType.BIT},
                   new PlcAddress{ Name="监听位2",Address="2", DataType=PlcDataType.BIT},
                   new PlcAddress{ Name="锁定位1",Address="3", DataType=PlcDataType.BIT},
                   new PlcAddress{ Name="锁定位2",Address="4", DataType=PlcDataType.BIT},
                   new PlcAddress{ Name="数据位1",Address="5", DataType=PlcDataType.WORD},
                   new PlcAddress{ Name="数据位2",Address="6", DataType=PlcDataType.WORD},
                   new PlcAddress{ Name="监听位3",Address="20", DataType=PlcDataType.BIT}
                }
            };

            var flow = new WorkFlow
            {
                Name = plcdevice.Name + "Flow",
                Listens = new List<PlcListen>
                {
                    new PlcListen{ Name="监听1_2", StartAddressName="监听位1", Len=2, Items=new List<Item>
                    {
                        new Item{Index=0, Value="1", OnBlockName= "读取模块" },
                        new Item{Index=1, Value="1", OnBlockName= "写入模块" },
                    }},
                    new PlcListen{ Name="监听3", StartAddressName="监听位3",Value="1", OnBlockName="自定义模块"}
                },
                Blocks = new List<FlowBlock>
                  {
                      new FlowBlock
                      {
                          Name ="读取模块",
                          Steps=new List<Step>
                          {
                              new Step{ PlcAddressName="数据位1", Type= ActionTypes.Read },
                          }
                      },
                      new FlowBlock
                      {
                          Name="写入模块",
                          Steps=new  List<Step>
                          {
                              new Step{ PlcAddressName="锁定位2", Type= ActionTypes.Write , Value="0"},
                              new Step{ PlcAddressName="数据位2", Type= ActionTypes.Write },
                              new Step{ PlcAddressName="锁定位2", Type= ActionTypes.Write,Value="1" },
                          }
                      },
                      new  FlowBlock
                      {
                           Name="自定义模块",
                           Steps=new  List<Step>{new Step{ ActionName="redis"}}
                      }
                  }
            };

            var work = new WorkFolwConfig { WorkFlows = new WorkFlow[] { flow } };
            var plc = new PlcConfig { PlcDevices = new List<PlcDevice> { plcdevice } };
            work.SaveToXMLFile(workflowcfg);
            plc.SaveToXMLFile(plccfg);
        }
    }
}
