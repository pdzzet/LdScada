using KHBC.Core;

namespace KHBC.LD.APCM
{
    static class Program
    {
        private const string Name = "KHBC.LD.APCM";
        private const string ModuleName = "APCM";

        static void Main(string[] args)
        {
            SysRun.MainEntry(args, Name, ModuleName);
        }
    }
}
