using KHBC.Core;

namespace KHBC.LD.CLSM
{
    static class Program
    {
        public const string Name = "KHBC.LD.CLSM";
        public const string ModuleName = "CLSM";

        static void Main(string[] args)
        {
            SysRun.MainEntry(args, Name, ModuleName);
        }
    }
}
