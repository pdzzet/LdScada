using System;

namespace KHBC.Core.FrameBase
{
    // <summary>
    /// 注册信息
    /// </summary>
    public struct Registrations
    {
        /// <summary>
        /// 注入类型
        /// </summary>
        public Type InType { get; set; }
        /// <summary>
        /// 依赖类型(指接口类型)
        /// </summary>
        public Type DType { get; set; }
        /// <summary>
        ///是共享实例
        /// </summary>
        public bool IsShare { get; set; }
        /// <summary>
        /// 注册依赖
        /// 1.inType为空时，dType不能为空,要求dType实现IDependency，引用接口的类型都被注册<para/>
        /// 2.inType为泛型，dType必须为泛型<para/>
        /// 3.dType为空时，注入类型不能为空，根据注入类型的实现依赖的接口单一注册<para/>
        /// 4.inType不为空，dType也不为空，按名字注册
        /// </summary>
        /// <param name="inType">注入类型</param>
        /// <param name="dType">接口类型</param>
        public Registrations(Type inType, bool isShare = true, Type dType = null)
        {
            InType = inType;
            DType = dType;
            IsShare = isShare;
        }

    }
}
