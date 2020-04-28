using KHBC.Core;
using KHBC.Core.Device;
using System.Collections.Generic;

namespace KHBC.LD.APCM.Workflow
{
    public class ApcmWorkflowP03 : ApcmWorkflowService
    {
        public ApcmWorkflowP03(DeviceInfo devInfo)
        {
            DevInfo = devInfo;
            ServiceName = $"{SysConf.ModuleName}:WF_P03";

            #region 三号位事件
            RegEvent(ApcmDevcieConfig.EVENT_P3_NGNULLAGVCALL, OnEvent_P3_NgNullAgvCall);
            #endregion
        }

        #region 三号位事件
        public void OnEvent_P3_NgNullAgvCall(ApcmEventArgs evt)
        {
            PushToMes(ApcmDevcieConfig.EVENT_MES_CHARGING_STATE, new Dictionary<string, object>
            {
                ["LINE"] = SysConf.Main.AssemblyLine.Id,
                ["IS_READ"] = 0,
                ["DISPATCH_STATE"] = 1,
                ["FLAG"] = 1,
                ["NO_FROM"] = "3",
            });
        }
        #endregion
    }
}
