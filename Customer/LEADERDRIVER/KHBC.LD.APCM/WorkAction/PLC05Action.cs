using System;
using System.Linq;
using KHBC.Core.Extend;
using KHBC.Core.Log;
using KHBC.Modbus;
using KHBC.Modbus.WorkFlows;

namespace KHBC.LD.APCM
{
    public class PLC05Action : IActionHandle
    {
        //料箱转移到5号位
        public Result PLC05WorkbinInEvent(ActionArgs actionArgs)
        {
            Logger.Device.Info($" invoke PLC05WorkbinInEvent 料箱转移到5号位成功");
            return Result.Success();
        }
        //读取单个箱体信息
        public Result PLC05ReadSingleInfoEvent(ActionArgs actionArgs)
        {
            Logger.Device.Info($" invoke PLC05ReadSingleInfoEvent 读取单个箱体信息成功");
            return Result.Success();
        }
        //写入批次号成功
        public Result PLC05WriteBatchEvent(ActionArgs actionArgs)
        {
            //生成批次号
            string batch = CreateBatch(actionArgs.StepResult);
            var eng = ModBusDataFactory.workFlowEngines.FirstOrDefault(x => x.ModuleName == "WFE");
            FlowBlock blocku = eng._workFlow.Blocks.FirstOrDefault(v => v.Name == "PLC05WriteBatchB");
            blocku.Steps[0].Value = batch;
            eng.EnqueueBlock(blocku);
            Logger.Device.Info($" invoke PLC05WriteBatchEvent 写入批次号成功");
            return Result.Success();
        }

        private string CreateBatch(Result res)
        {
            //批次生成规则生成批次号
            return "";
        }

        //料箱转移出到5号位
        public Result PLC05WorkbinOutEvent(ActionArgs actionArgs)
        {
            Logger.Device.Info($" invoke PLC05WorkbinOutEvent 料箱转移出5号位成功");
            return Result.Success();
        }
    }
}
