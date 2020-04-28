using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using KHBC.Core.BaseModels;
using KHBC.Core;
using KHBC.LD.APCM.Workflow;
//using KHBC.Modbus;
//using KHBC.Modbus.WorkFlows;

namespace KHBC.LD.APCM
{
    public class Startup : BaseStartup
    {
        public override void AfterStartup()
        {
            // 加入设备线程
            if (SysConf.Device != null && SysConf.Device.Devices != null)
            {
                foreach (var val in SysConf.Device.Devices)
                {
                    var d = val.Value;
                    ServiceManager.Services.Add(new ApcmDeviceService(d));
                    ServiceManager.Services.Add(new ApcmWorkflowP00(d));
                    ServiceManager.Services.Add(new ApcmWorkflowP01(d));
                    ServiceManager.Services.Add(new ApcmWorkflowP02(d));
                    ServiceManager.Services.Add(new ApcmWorkflowP03(d));
                    ServiceManager.Services.Add(new ApcmWorkflowP04(d));
                    ServiceManager.Services.Add(new ApcmWorkflowP05(d));
                    ServiceManager.Services.Add(new ApcmWorkflowCNC(d));
                }
            }
        }

        //private void StartReadP4PlcInfoTimmer()
        //{
        //    System.Timers.Timer timer = new System.Timers.Timer();
        //    timer.Enabled = true;
        //    timer.Interval = 6000; //执行间隔时间,单位为毫秒; 这里实际间隔为1分钟  
        //    timer.Start();
        //    timer.Elapsed += new System.Timers.ElapsedEventHandler(ReadP4PlcInfo);
        //}

        //private void ReadP4PlcInfo(object sender, ElapsedEventArgs e)
        //{
        //    //每个工控机只对应一个PLC,才可以下边的操作?
        //    var eng = ModBusDataFactory.workFlowEngines.FirstOrDefault(x => x.ModuleName == "WFE");
        //    FlowBlock block = eng._workFlow.Blocks.FirstOrDefault(v => v.Name == "PLC04ReadInfoB");
        //    eng.EnqueueBlock(block);
        //}

        //private void StartReadP4NumTimmer()
        //{
        //    System.Timers.Timer timer = new System.Timers.Timer();
        //    timer.Enabled = true;
        //    timer.Interval = 6000; //执行间隔时间,单位为毫秒; 这里实际间隔为1分钟  
        //    timer.Start();
        //    timer.Elapsed += new System.Timers.ElapsedEventHandler(ReadP4Num);
        //}

        ////触发读取4号位置数量
        //private void ReadP4Num(object source, ElapsedEventArgs e)
        //{
        //    //每个工控机只对应一个PLC,才可以下边的操作
        //    var eng = ModBusDataFactory.workFlowEngines.FirstOrDefault(x => x.ModuleName == "WFE");
        //    FlowBlock block = eng._workFlow.Blocks.FirstOrDefault(v => v.Name == "PLC04ReadProNumB");
        //    eng.EnqueueBlock(block);
        //}
        public override void BeforeStartup()
        {
            ApcmKeyConf.Init();
        }
    }
}
