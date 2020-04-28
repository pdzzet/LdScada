using KHBC.Core;

namespace KHBC.LD.OMCM
{
    static class Program
    {
        public const string Name = "KHBC.LD.OMCM";
        public const string ModuleName = "OMCM";

        static void Main(string[] args)
        {
            SysRun.MainEntry(args, Name, ModuleName);
        }
    }
}
