using System.Collections.Generic;

namespace KHBC.Core.FrameBase
{
    public interface IDbController : IDependency
    {
        /// <summary>
        /// 服务名
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="entity">字段字典</param>
        /// <returns>是否操作成功</returns>
        bool Add(Dictionary<string, object> entity);
    }
}
