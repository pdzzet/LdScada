using KHBC.BPMS.Log;
using System;
using System.ServiceProcess;

namespace KHBC.BPMS
{
    partial class BpmsService : ServiceBase
    {
        private readonly ProcessInfoService pInfoSevice;
        private readonly CmdInfoService cmdInfoSevice;


        public BpmsService()
        {
            InitializeComponent();

            pInfoSevice = new ProcessInfoService();
            cmdInfoSevice = new CmdInfoService();

            cmdInfoSevice.ExecCmded += CmdInfoSevice_ExecCmded;
        }

        public void Run()
        {
            pInfoSevice.BeginWork();
            cmdInfoSevice.BeginWork();
        }

        protected override void OnStart(string[] args)
        {
            pInfoSevice.BeginWork();
            cmdInfoSevice.BeginWork();
        }

        protected override void OnStop()
        {
            pInfoSevice.EndWork();
            cmdInfoSevice.EndWork();
        }

        private void CmdInfoSevice_ExecCmded(string str)
        {
            try
            {
                var arr = str.Split(':');
                var name = arr[0];
                var action = arr[1].ToUpper();
                switch (action)
                {
                    case "STOP":
                        pInfoSevice.Stop(name);
                        break;
                    case "START":
                        pInfoSevice.Start(name);
                        break;
                    case "RESTART":
                        pInfoSevice.ReStart(name);
                        break;
                }

            }
            catch (Exception ex)
            {
                Logger.Main.Info($"执行命令错误: {ex.Message}");
            }
        }
    }
}
