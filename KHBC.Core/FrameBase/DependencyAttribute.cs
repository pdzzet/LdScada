using System;

namespace KHBC.Core.FrameBase
{
    //属性依赖注入
    public class DependencyPropertyAttribute : Attribute
    {
        /// <summary>
        /// 按照注册名取实例
        /// </summary>
        public string Named { get; }
        //属性依赖注入
        public DependencyPropertyAttribute(string name = "")
        {
            Named = name;
        }
    }


}
