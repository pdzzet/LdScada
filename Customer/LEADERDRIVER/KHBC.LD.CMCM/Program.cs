using KHBC.Core;

namespace KHBC.LD.CMCM
{
    static class Program
    {
        public const string Name = "KHBC.LD.CMCM";
        public const string ModuleName = "CMCM";

        static void Main(string[] args)
        {
            SysRun.MainEntry(args, Name, ModuleName);
        }
    }
}
