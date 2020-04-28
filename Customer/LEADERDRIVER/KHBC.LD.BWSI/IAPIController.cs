using KHBC.Core.Extend;
using KHBC.Core.FrameBase;
using Nancy;

namespace KHBC.LD.BWSI
{
    /// <summary>
    /// HTTPHost API接口
    /// </summary>
    public interface IApiController : IDependency
    {
        /// <summary>
        /// 服务名
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Result DoHttpWork(Request req);
    }
}
