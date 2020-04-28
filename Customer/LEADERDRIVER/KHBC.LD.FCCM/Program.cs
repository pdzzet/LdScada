using KHBC.Core;

namespace KHBC.LD.FCCM
{
    static class Program
    {
        public const string Name = "KHBC.LD.FCCM";
        public const string ModuleName = "FCCM";

        static void Main(string[] args)
        {
            SysRun.MainEntry(args, Name, ModuleName);
        }
    }
}
