using System.Threading;
using KHBC.Core;
using KHBC.Core.Log;

namespace KHBC.LD.BDCI
{
    static class Program
    {
        private const string Name = "KHBC.LD.BDCI";
        private const string ModuleName = "BDCI";
        static void Main(string[] args)
        {
            SysRun.MainEntry(args, Name, ModuleName);
        }
    }
}
