using KHBC.Core;

namespace KHBC.LD.MDCI
{
    static class Program
    {
        public const string Name = "KHBC.LD.MDCI";
        public const string ModuleName = "MDCI";

        static void Main(string[] args)
        {
            SysRun.MainEntry(args, Name, ModuleName);
        }
    }
}
