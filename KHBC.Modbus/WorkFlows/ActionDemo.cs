using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KHBC.Core.Extend;
using KHBC.Core.Log;
using KHBC.Core;

namespace KHBC.Modbus.WorkFlows
{
    public class ActionDemo : IActionHandle
    {
        public string Name => "Demo";

        public Result DemoHello(ActionArgs actionArgs)
        {
            Logger.Device.Info($"{actionArgs.WorkFlowName}, {actionArgs.BlockName} invoke DemoHello");
            return Result.Success();
        }

        public Result DemoWaitHello(ActionArgs actionArgs)
        {
            Logger.Device.Info($"{actionArgs.WorkFlowName}, {actionArgs.BlockName} invoke begin DemoHello");
            System.Threading.Thread.Sleep(30 * 1000);
            Logger.Device.Info($"{actionArgs.WorkFlowName}, {actionArgs.BlockName} invoke DemoHello");
            return Result.Success();
        }
    }
}
