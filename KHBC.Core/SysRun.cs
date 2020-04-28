using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using KHBC.Core;
using KHBC.Core.Log;

namespace KHBC.Core
{
    public static class SysRun
    {
        private static readonly CancellationTokenSource _cts = new CancellationTokenSource();
        public static void MainEntry(string[] args, string processName, string moduleName = "")
        {
            Logger.Main.Info("");
            Logger.Main.Info($"[{processName}] 开始运行");

            try
            {
                var pInfo = Process.GetCurrentProcess();
                if (pInfo.ProcessName == processName)
                {
                    if (args.Length > 0)
                    {
                        var pName = args[0];
                        if (pInfo.ProcessName != pName)
                        {
                            var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{pName}.exe");
                            if (!File.Exists(filename))
                            {
                                if (pInfo.MainModule != null) File.Copy(pInfo.MainModule.FileName, filename);
                            }
                            Process.Start(filename);
                            return;
                        }
                    }
                }
                var title = pInfo.ProcessName;
                Console.Title = title;
                //检测进程
                var unused = new Mutex(true, title, out bool isRunning);
                if (!isRunning)
                {
                    Environment.Exit(-1);
                }

                //IntPtr intptr = FindWindow("ConsoleWindowClass", Console.Title);
                //if (intptr != IntPtr.Zero)
                //{
                //    ShowWindow(intptr, 0);//隐藏这个窗口
                //}

                SysConf.Startup(processName, moduleName);
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Logger.Main.Error(ex);
            }
        }

        //private static void HideWindow()
        //{
        //    int time = 5;
        //    while (time > 0)
        //    {
        //        if (_cts.IsCancellationRequested)
        //        {
        //            return;
        //        }
        //        Console.WriteLine(time);
        //        Thread.Sleep(1000);
        //        time--;
        //    }
        //    IntPtr intptr = FindWindow("ConsoleWindowClass", Console.Title);
        //    if (intptr != IntPtr.Zero)
        //    {
        //        ShowWindow(intptr, 0);//隐藏这个窗口
        //    }
        //}

        #region win_api
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        #endregion
    }
}
