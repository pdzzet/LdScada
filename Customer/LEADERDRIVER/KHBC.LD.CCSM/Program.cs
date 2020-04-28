using KHBC.Core;

namespace KHBC.LD.CCSM
{
    static class Program
    {
        public const string Name = "KHBC.LD.CCSM";
        public const string ModuleName = "CCSM";

        static void Main(string[] args)
        {
            SysRun.MainEntry(args, Name, ModuleName);
        }
    }
}
