using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessTool
{
    public class ProcessModel : INotifyPropertyChanged
    {
        public int ProcessId
        {
            get { return processId; }
            set
            {
                processId = value;
                OnPropertyChanged("ProcessId");
            }
        }
        private int processId;

        public string ProcessName
        {
            get { return processName; }
            set
            {
                processName = value;
                OnPropertyChanged("ProcessName");
            }
        }
        private string processName;

        public string CPU
        {
            get { return cpu; }
            set
            {
                cpu = value;
                OnPropertyChanged("CPU");
            }
        }

        private string cpu;
        public string Memory
        {
            get { return memory; }
            set
            {
                memory = value;
                OnPropertyChanged("Memory");
            }
        }

        private string memory;
        public int Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        private int status;

        public string StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                OnPropertyChanged("StartTime");
            }
        }

        private string startTime;
        public string MachineName
        {
            get { return machineName; }
            set
            {
                machineName = value;
                OnPropertyChanged("MachineName");
            }
        }

        private string machineName;
        public string ProcessPath
        {
            get { return processPath; }
            set
            {
                processPath = value;
                OnPropertyChanged("ProcessPath");
            }
        }

        private string processPath;

        public int ThreadNum
        {
            get { return threadNum; }
            set
            {
                threadNum = value;
                OnPropertyChanged("ThreadNum");
            }
        }

        private int threadNum;

        public string ThreadInfos
        {
            get { return threadInfos; }
            set
            {
                threadInfos = value;
                OnPropertyChanged("ThreadInfos");
            }
        }

        private string threadInfos;
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
