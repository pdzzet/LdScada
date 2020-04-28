using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHBC.BPMS
{
    public class ThreadModel
    {
        public int Id { get; set; }
        public string StartTime { get; set; }
        public ThreadState State { get; set; }
    }

    public class ProcessModel
    {
        public int ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string CPU { get; set; }
        public string Memory { get; set; }
        public string Status { get; set; }
        public string StartTime { get; set; }
        public string UpdateTime { get; set; }
        public string MachineName { get; set; }
        public string ProcessPath { get; set; }
        public List<ThreadModel> Threads { get; set; }
    }

    public enum TaskSpeed : int
    {
        Fast = 1,
        Normal = 5,
        Slow = 30
    }
}
