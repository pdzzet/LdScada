using KHBC.Core;

namespace KHBC.LD.ARCM
{
    static class Program
    {
        public const string Name = "KHBC.LD.ARCM";
        public const string ModuleName = "ARCM";

        static void Main(string[] args)
        {
            SysRun.MainEntry(args, Name, ModuleName);
        }
    }
}
