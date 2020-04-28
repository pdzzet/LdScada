using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProcessTool
{
    public class ProcessViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<ProcessModel> ProcessData
        {
            get { return processData; }
            set
            {
                processData = value;
                OnPropertyChanged("ProcessData");
            }
        }
        private ObservableCollection<ProcessModel> processData = new ObservableCollection<ProcessModel>();

        public ICommand StartCommand { get; set; }
        public ICommand StopCommand { get; set; }
        public ICommand ReStartCommand { get; set; }


        private List<string> _pathList = new List<string>();
        private List<string> _errorList = new List<string>();
        private const string ModuleName = "KHBC";
        private const string FileName = "processList.txt";

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ProcessViewModel()
        {
            StartCommand = new DelegateCommand<ProcessModel>(Start);
            StopCommand = new DelegateCommand<ProcessModel>(Stop);
            ReStartCommand = new DelegateCommand<ProcessModel>(ReStart);
            InitData();
            BeginWork();
        }

        private void InitData()
        {
            var filePath = AppDomain.CurrentDomain.BaseDirectory + FileName;
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                        _pathList.Add(line);
                }
                sr.Close();
                sr.Dispose();
            }

        }

        public void BeginWork()
        {
            Task.Factory.StartNew(ThrWork);
        }


        private void ThrWork()
        {
            while (true)
            {
                System.Threading.Thread.Sleep((int)TaskSpeed.Fast * 1000);
                try
                {
                    var procsList = new List<Process>();
                    var procs = Process.GetProcesses().Where(p => !_errorList.Contains(p.ProcessName));
                    foreach (var p in procs)
                    {
                        try
                        {
                            if (p.MainModule.FileVersionInfo.FileDescription == ModuleName|| _pathList.Contains(p.StartInfo.FileName))
                            {
                                procsList.Add(p);
                            }
                        }
                        catch (Exception)
                        {
                            _errorList.Add(p.ProcessName);
                        }
                    }
                    var list = new List<ProcessModel>();
                    foreach (var item in ProcessData)
                    {
                        Process info = procsList.FirstOrDefault(x => x.ProcessName == item.ProcessName);
                        if (info == null)
                        {
                            item.Status = 0;
                            item.ProcessId = 0;
                            list.Add(item);
                        }
                    }
                   
                    procsList.ForEach(x =>
                    {
                        var thrList = new List<string>();
                        foreach (ProcessThread thr in x.Threads)
                            thrList.Add($"[{thr.Id},{thr.ThreadState.ToString()}]");
                        PerformanceCounter pf1 =
                            new PerformanceCounter("Process", "Working Set - Private", x.ProcessName);
                        list.Add(new ProcessModel
                        {
                            ProcessId = x.Id,
                            ProcessName = x.ProcessName,
                            StartTime = x.StartTime.ToShortTimeString(),
                            MachineName = x.MachineName,
                            Memory = $"{Math.Round(pf1.NextValue() / 1024 / 1024, 2)}M",
                            CPU = $"{Math.Round(x.TotalProcessorTime.TotalMilliseconds/100 / x.UserProcessorTime.TotalMilliseconds, 2)}%",
                            ProcessPath = x.MainModule?.FileName,
                            ThreadNum=x.Threads.Count,
                            ThreadInfos=string.Join("|",thrList),
                            Status = 1
                        });
                    });
                    ProcessData = new ObservableCollection<ProcessModel>(list);
            }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="filePaths"></param>
        private void Start(ProcessModel model)
        {
            try
            {
                if (File.Exists(model.ProcessPath))
                    Process.Start(model.ProcessPath);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 强行终止
        /// </summary>
        /// <param name="pid"></param>
        private void Stop(ProcessModel model)
        {
            var p = Process.GetProcessById(model.ProcessId);
            try
            {
                p?.Kill();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 重启
        /// </summary>
        /// <param name="models"></param>
        private void ReStart(ProcessModel model)
        {
            model.Status = 2;
            Task.Factory.StartNew(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Stop(model);
                Thread.Sleep(3000);
                Start(model);
            });
        }
    }

    public enum TaskSpeed : int
    {
        Fast = 1,
        Normal = 5,
        Slow = 30
    }
}
