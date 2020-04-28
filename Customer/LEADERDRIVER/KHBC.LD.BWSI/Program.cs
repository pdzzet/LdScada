using System.Threading;
using KHBC.Core;
using KHBC.Core.Log;

namespace KHBC.LD.BWSI
{
    static class Program
    {
        public const string Name = "KHBC.LD.BWSI";
        public const string ModuleName = "BWSI";

        static void Main(string[] args)
        {
            SysRun.MainEntry(args, Name, ModuleName);
        }
    }
}
