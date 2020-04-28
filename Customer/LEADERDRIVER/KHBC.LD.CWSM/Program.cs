using KHBC.Core;

namespace KHBC.LD.CWSM
{
    static class Program
    {
        public const string Name = "KHBC.LD.CWSM";
        public const string ModuleName = "CWSM";

        static void Main(string[] args)
        {
            SysRun.MainEntry(args, Name, ModuleName);
        }
    }
}
