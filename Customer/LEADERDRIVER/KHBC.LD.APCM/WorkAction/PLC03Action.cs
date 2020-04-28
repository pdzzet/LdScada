using System.Linq;
using KHBC.Core.Extend;
using KHBC.Core;
using KHBC.Core.Log;
using KHBC.LD.DTO;
using KHBC.Modbus;
using KHBC.Modbus.WorkFlows;

namespace KHBC.LD.APCM
{
    public class PLC03Action : IActionHandle
    {
        //料箱转移到3号位
        public Result PLC03WorkbinInEvent(ActionArgs actionArgs)
        {
            Logger.Device.Info($" invoke PLC03WorkbinInEvent 料箱转移到3号位成功");
            return Result.Success();
        }
        //读取NgNum来判断是否需要NG空箱呼叫
        public Result PLC03NgNumReadEvent(ActionArgs actionArgs)
        {
            //查找当前工作流引擎
            var eng = ModBusDataFactory.workFlowEngines.FirstOrDefault(x => x.ModuleName == "WFE");
            int NgNum =(int)actionArgs.StepResult.Data;
            if (NgNum >= Constant.Package)
            {
                //触发设置空箱任务呼叫信号
                FlowBlock block = eng?._workFlow.Blocks.FirstOrDefault(v => v.Name == "PLC03Setp3NgNullAgvCallB");
                eng?.EnqueueBlock(block);
            }
            Logger.Device.Info($" invoke PLC03WorkbinInEvent 料箱转移到3号位成功");
            return Result.Success();
        }

        //给MES添加空箱下料请求 3
        public Result PLC03NgNullAgvCallEvent(ActionArgs actionArgs)
        {
            RChargingState chargingState = new RChargingState()
            {
                LINE = Constant.OrderLine,
                IS_READ = 0,
                DISPATCH_STATE = 1,
                FLAG = 1,
                NO_FROM = "3"
            };
            actionArgs.RedisClientRemote.LPush("LD:A00:MDCI:CHARGING_STATE:Q", chargingState);
            Logger.Device.Info($" invoke PLC03NgNullAgvCallEvent 给MES添加空箱下料请求成功");
            return Result.Success();
        }

        //SCADA记录Agv到达3号口
        public Result PLC03AgvArriveEvent(ActionArgs actionArgs)
        {
            Logger.Device.Info($" invoke PLC03AgvArriveEvent SCADA记录Agv到达3号口");
            return Result.Success();
        }
        //料箱转移出到3号位
        public Result PLC03WorkbinOutEvent(ActionArgs actionArgs)
        {
            Logger.Device.Info($" invoke PLC03WorkbinOutEvent 料箱转移出3号位成功");
            return Result.Success();
        }
    }
}
