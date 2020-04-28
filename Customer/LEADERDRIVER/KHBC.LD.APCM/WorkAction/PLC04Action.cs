using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using KHBC.Core;
using KHBC.Core.Extend;
using KHBC.Core.Log;
using KHBC.Modbus;
using KHBC.Modbus.WorkFlows;

namespace KHBC.LD.APCM
{
    public class PLC04Action : IActionHandle
    {
        //料箱转移到4号位
        public Result PLC04WorkbinInEvent(ActionArgs actionArgs)
        {
            Logger.Device.Info($" invoke PLC04WorkbinInEvent 料箱转移到4号位成功");
            return Result.Success();
        }
        //读取4号位数量
        public Result PLC04ReadProNumEvent(ActionArgs actionArgs)
        {
            int P4Num = (int)actionArgs.StepResult.Data;
            if (P4Num != Constant.p4Num)
            {
                //更新本地缓存
                Constant.p4Num = P4Num;
                actionArgs.RedisClientLocal.Set($"LD:{Constant.OrderLine}:APCM:P4NUM", P4Num);
            }
            Logger.Device.Info($" invoke PLC04ReadProNumB 读取4号位数量");
            return Result.Success();
        }

        //读取4号位所有数据
        public Result PLC04ReadInfoEvent(ActionArgs actionArgs)
        {
            //获取4号位所有信息;更新料箱明细表
            byte[]  P4PlcInfo = (byte[])actionArgs.StepResult.Data;
            //对比物料号数据，得出是否需要更新
            bool isUpdate = CompareP4PlcInfo(P4PlcInfo);
            var eng = ModBusDataFactory.workFlowEngines.FirstOrDefault(x => x.ModuleName == "WFE");
            if (!isUpdate)
            {
                //4号位需要更新
                FlowBlock blocku = eng._workFlow.Blocks.FirstOrDefault(v => v.Name == "PLC04UpdateB");
                eng.EnqueueBlock(blocku);
                //更新具体信息
                //TODO 获取更新的内容
                string content = "";
                FlowBlock blockci = eng._workFlow.Blocks.FirstOrDefault(v => v.Name == "PLC04UpdateCppInfoB");
                blockci.Steps[0].Value = content;
                eng.EnqueueBlock(blockci);
            }
            else
            {
                //4号位不需要更新
                FlowBlock block = eng._workFlow.Blocks.FirstOrDefault(v => v.Name == "PLC04NoUpdateB");
                eng.EnqueueBlock(block);
            }
            Logger.Device.Info($" invoke PLC04NoUpdateB 读取4号位所有数据");
            return Result.Success();
        }

        private bool CompareP4PlcInfo(byte[] p4PlcInfo)
        {
            //TODO 获取此订单的物料号信息，进行对比
            return true;
        }

        //正常需要更新的具体信息字段完毕
        public Result PLC04UpdateCppInfoEvent(ActionArgs actionArgs)
        {
            Logger.Device.Info($" invoke PLC04UpdateCppInfoEvent 正常 更新具体信息数据完毕");
            return Result.Success();
        }
        //SCADA记录PLC更新物料号完毕
        public Result PLC04PlcUpdateDoneEvent(ActionArgs actionArgs)
        {
            Logger.Device.Info($" invoke PLC04PlcUpdateDoneEvent SCADA记录PLC更新物料号完毕");
            return Result.Success();
        }
        //特殊放行 需要更新的具体信息字段完毕
        public Result PLC04SpeCppInfoEvent(ActionArgs actionArgs)
        {
            Logger.Device.Info($" invoke PLC04SpeCppInfoEvent 特殊放行 更新数据 完毕");
            return Result.Success();
        }
        //SCADA记录 特殊放行 PLC更新物料号完毕
        public Result PLC04PlcUpdateDoneSpeEvent(ActionArgs actionArgs)
        {
            Logger.Device.Info($" invoke PLC04PlcUpdateDoneSpeEvent SCADA记录 特殊放行 PLC更新物料号完毕");
            return Result.Success();
        }
        //特殊放行关闭
        public Result PLC04WorkbinSpeOffBEvent(ActionArgs actionArgs)
        {
            Logger.Device.Info($" invoke PLC04WorkbinSpeOffBEvent 特殊放行关闭完成");
            return Result.Success();
        }
        //4号位 箱出
        public Result PLC04WorkbinOutEvent(ActionArgs actionArgs)
        {
            Logger.Device.Info($" invoke PLC04WorkbinOutEvent 料箱转移出4号位成功");
            return Result.Success();
        }
    }
}
