using System.Collections.Generic;
using KHBC.Core.FrameBase;

namespace KHBC.Core.BaseModels
{
    public abstract class BaseStartup : IStartup
    {
        public virtual int Order => 0;

        public virtual void AfterStartup()
        {
            
        }

        public virtual void BeforeStartup()
        {
            
        }

        public virtual List<Registrations> Register()
        {
            return new List<Registrations>();
        }
    }
}
