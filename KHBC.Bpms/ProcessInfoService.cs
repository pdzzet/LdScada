using KHBC.BPMS.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KHBC.BPMS
{
    public class ProcessInfoService : SvrBase
    {
        public Dictionary<string, ProcessModel> _processList = new Dictionary<string, ProcessModel>();

        public ProcessInfoService()
        {
            InitData();
        }
        private void InitData()
        {
            foreach (string procName in BpmsConf.Conf.ProcessList)
            {
                _processList.Add(procName, new ProcessModel
                {
                    Status = "STOP",
                    ProcessId = 0,
                    ProcessName = procName
                });
            }
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="filePath"></param>
        public void Start(string procName)
        {
            var filePath = procName;

            if (_processList.ContainsKey(procName))
            {
                filePath = Path.GetFullPath($"{BpmsConf.AppPath}/{procName}/{procName}.exe");
            }
            try
            {
                if (File.Exists(filePath))
                {
                    Process.Start(filePath);
                    Logger.Main.Info($"正在启动进程: ${filePath}");
                    if (_processList.ContainsKey(procName))
                    {
                        _processList[procName].Status = "STARTING";
                        SetRedis(_processList[procName]);
                    }
                }
                else
                {
                    Logger.Main.Error($"启动进程失败: ${filePath} 不存在");
                }
            }
            catch (Exception ex)
            {
                Logger.Main.Error($"启动进程失败: ${filePath}, {ex.Message}");
            }
        }

        /// <summary>
        /// 强行终止
        /// </summary>
        /// <param name="processName"></param>
        public void Stop(string processName)
        {
            var p = Process.GetProcessesByName(processName);
            try
            {
                if (p?.Length > 0)
                {
                    p[0].Kill();
                    Logger.Main.Info($"停止进程成功: ${processName}");
                }
            }
            catch (Exception ex)
            {
                Logger.Main.Error($"停止进程失败: ${processName}, {ex.Message}");
            }
        }

        /// <summary>
        /// 重启
        /// </summary>
        /// <param name="models"></param>
        public void ReStart(string processName)
        {
            if (!_processList.ContainsKey(processName))
            {
                Logger.Main.Error($"进程{processName}不在管理列表中");
                return;
            }

            var model = _processList[processName];
            model.Status = "RESTARTING";
            Task.Factory.StartNew(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Stop(model.ProcessName);
                Thread.Sleep(3000);
                Start(model.ProcessPath);
            });
        }

        protected override void DoWork()
        {
            while (Running)
            {
                Thread.Sleep((int)Speed * 1000);

                try
                {
                    var runList = new List<string>();

                    // 找出正常运行的进程
                    var procs = Process.GetProcesses().Where(p => _processList.ContainsKey(p.ProcessName));
                    foreach (var x in procs)
                    {
                        var p = x.ProcessName;

                        runList.Add(p);

                        _processList[p].Threads = new List<ThreadModel> { };

                        foreach (ProcessThread thr in x.Threads)
                        {
                            ThreadModel tm = new ThreadModel
                            {
                                Id = thr.Id,
                                StartTime = thr.StartTime.ToString(),
                                State = thr.ThreadState
                            };
                            _processList[p].Threads.Add(tm);
                        }

                        _processList[p].ProcessId = x.Id;
                        _processList[p].ProcessName = x.ProcessName;
                        _processList[p].StartTime = x.StartTime.ToString();
                        _processList[p].MachineName = x.MachineName;
                        PerformanceCounter pf1 = new PerformanceCounter("Process", "Working Set - Private", x.ProcessName);
                        _processList[p].Memory = $"{Math.Round(pf1.NextValue() / 1024 / 1024, 2)}M";
                        _processList[p].CPU = $"{Math.Round(x.TotalProcessorTime.TotalMilliseconds / 100 / x.UserProcessorTime.TotalMilliseconds, 2)}%";
                        _processList[p].ProcessPath = x.MainModule?.FileName;
                        _processList[p].Status = "RUNNING";
                        _processList[p].UpdateTime = DateTime.Now.ToString();
                        SetRedis(_processList[p]);
                    }

                    foreach (var p in _processList.Keys)
                    {
                        if (!runList.Contains(p))
                        {
                            _processList[p].Status = "STOP";
                            _processList[p].ProcessId = 0;
                            _processList[p].UpdateTime = DateTime.Now.ToString();
                        }

                        SetRedis(_processList[p]);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Main.Error($"获取进程信息失败: {ex.Message}");
                }
            }
        }

        private void SetRedis(ProcessModel model)
        {
            var key = $"LD:{BpmsConf.Conf.ProductionLineId}:BPMS:{model.ProcessName}";

            try
            {
                BpmsConf.RedisClientLocal.Set(key, model);
                if (BpmsConf.RunInEvoc)
                {
                    BpmsConf.RedisClientRemote.Set(key, model);
                }
            }
            catch (Exception ex)
            {
                Logger.Main.Error($"保存进程信息到REDIS失败: {ex.Message}");
            }
        }
    }
}
