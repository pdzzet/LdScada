using KHBC.Core.FrameBase;

namespace KHBC.Core
{
    public interface IActionHandle : IDependency
    {
        string Name { get; }
    }
}
