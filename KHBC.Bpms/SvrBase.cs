using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KHBC.BPMS
{
    public abstract class SvrBase
    {
        protected bool Running = false;
        protected Thread thr;
        protected TaskSpeed Speed = TaskSpeed.Fast;
        public SvrBase()
        {
        }
        public virtual void BeginWork()
        {
            if (Running)
            {
                Running = false;
                Thread.Sleep(3 * 1000);
            }
            Running = true;
            Task.Factory.StartNew(ThrWork);
        }
        public virtual void EndWork()
        {
            Running = false;
        }


        private void ThrWork()
        {
            thr = System.Threading.Thread.CurrentThread;
            thr.IsBackground = true;
            DoWork();
            thr = null;
        }

        protected abstract void DoWork();

    }
}
