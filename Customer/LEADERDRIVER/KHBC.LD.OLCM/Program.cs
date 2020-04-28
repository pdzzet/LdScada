using KHBC.Core;

namespace KHBC.LD.OLCM
{
    static class Program
    {
        public const string Name = "KHBC.LD.OLCM";
        public const string ModuleName = "OLCM";

        static void Main(string[] args)
        {
            SysRun.MainEntry(args, Name, ModuleName);
        }
    }
}
