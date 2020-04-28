using KHBC.BPMS.Log;
using System;

namespace KHBC.BPMS
{
    public class CmdInfoService : SvrBase
    {
        public event Action<string> ExecCmded;

        private readonly string cmdkey;
        public CmdInfoService()
        {
            cmdkey = $"LD:{BpmsConf.Conf.ProductionLineId}:BPMS:PROCCTL:Q";
        }

        protected override void DoWork()
        {
            while (Running)
            {
                try
                {
                    var cmd = BpmsConf.RedisClientLocal.BRPop(3, cmdkey);
                    if (!string.IsNullOrWhiteSpace(cmd))
                    {
                        Logger.Main.Info($"收到进程管理命令: {cmd}");
                        ExecCmded?.Invoke(cmd);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Main.Info($"LOCAL REDIS CLIENT 异常: {ex.Message}");
                }
            }
        }
    }
}
