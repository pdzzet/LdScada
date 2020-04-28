using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using KHBC.Core.FrameBase;

/*Autofac生命周期
 * InstancePerDependency对每一个依赖或每一次调用创建一个新的唯一的实例。这也是默认的创建实例的方式
 * InstancePerLifetimeScope在一个生命周期域中，每一个依赖或调用创建一个单一的共享的实例，且每一个不同的生命周期域，实例是唯一的，不共享的。
 * InstancePerMatchingLifetimeScope在一个做标识的生命周期域中，每一个依赖或调用创建一个单一的共享的实例。打了标识了的生命周期域中的子标识域中可以共享父级域中的实例。若在整个继承层次中没有找到打标识的生命周期域，则会抛出异常：
 * InstancePerOwned在一个生命周期域中所拥有的实例创建的生命周期中，每一个依赖组件或调用Resolve()方法创建一个单一的共享的实例，并且子生命周期域共享父生命周期域中的实例。若在继承层级中没有发现合适的拥有子实例的生命周期域，则抛出异常
 * SingleInstance每一次依赖组件或调用Resolve()方法都会得到一个相同的共享的实例。其实就是单例模式。
 * InstancePerHttpRequest  （新版autofac建议使用InstancePerRequest）在一次Http请求上下文中,共享一个组件实例。仅适用于asp.net mvc开发。
 * */

/*
OnRegistered(e => Console.WriteLine("在注册的时候调用!"))
.OnPreparing(e => Console.WriteLine("在准备创建的时候调用!"))
.OnActivating(e => Console.WriteLine("在创建之前调用!"))
.OnActivated(e => Console.WriteLine("创建之后调用!"))
.OnRelease(e => Console.WriteLine("在释放占用的资源之前调用!"));
*/
namespace KHBC.Core
{

    /// <summary>
    /// Ioc容器
    /// </summary>
    public class IocAutofacContainer : ISysContainer
    {
        public readonly AssemblyInfo[] Assemblies;
        private readonly ContainerBuilder _builder;
        private IStartup[] _startups;
        public List<Type> Depends { get; private set; }
        /// <summary>
        /// 公用容器
        /// </summary>
        internal IContainer Container { get; private set; }

        /// <summary>
        /// 容器初始化
        /// </summary>
        /// <param name="builder">容器</param>
        /// <param name="assemblies">程序集</param>
        /// <param name="startups">启动项</param>
        internal IocAutofacContainer(AssemblyInfo[] assemblies, IStartup[] startups)
        {
            this._builder = new ContainerBuilder();
            this.Assemblies = assemblies;
            this._startups = startups;
        }

        /// <summary>
        /// IOC完整注册
        /// </summary>
        internal void Do()
        {

            ////按接口和程序集过滤
            //foreach (var assemblyInfo in assemblies)
            //    assemblyInfo.Types = assemblyInfo.Types
            //        .Where(p => typeof(IDependency).IsAssignableFrom(p))
            //        .ToArray();

            //按接口过滤，包含接口和接口实现
            Depends = Assemblies.SelectMany(x => x.Types.Where(p => typeof(IDependency).IsAssignableFrom(p))).ToList();
            
            //注册模块
            RegisterModules();

            //注册自定义的类型
            RegisterCustomers();

            //生成容器（真实注册）
            Container = _builder.Build();
          //  ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));

        }

        /// <summary>
        /// 所有非抽象类的接口实现类型
        /// </summary>
        /// <returns></returns>
        private List<Type> GetTypes<T>()
        {
            return Depends.Where(p => !p.IsAbstract && !p.IsInterface && typeof(T).IsAssignableFrom(p)).ToList();
        }
        /// <summary>
        /// 根据类型获取所有实例
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private List<Type> GetTypes(Type t)
        {
            return Depends.Where(p => !p.IsAbstract && !p.IsInterface && t.IsAssignableFrom(p)).ToList();
        }

        /// <summary>
        /// 控制反转所有
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> ResolveAll<T>()
        {
            var types = GetTypes<T>();
            var list = types.Select(x => Container.ResolveNamed<T>(x.Name)).ToList();
            return list;
        }

        /// <summary>
        /// 控制反转按特殊的名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private List<T> ResolveAllSpecailName<T>()
        {
            var types = GetTypes<T>();
            var list = types.Select(x => Container.ResolveNamed<T>(x.FullName.Replace(SysBootStrapper.Prefix, "").Replace(".", ""))).ToList();
            return list;
        }



        private void RegisterModules()
        {
            /* 注册IModule规则：
             * 1.获取所有继承IModule的接口
             * 2.每个业务模块单独注册其下类型对应的所有实现
             * 3.开启属性注入
             */
            var modelInterfaces =
             Depends.Where(p => typeof(IModule).IsAssignableFrom(p) && p.IsInterface && p != typeof(IModule))
                    .ToArray();
            foreach (var modelInterface in modelInterfaces)
            {
                //找到接口的实现类
                var sub = Depends.Where(p => modelInterface.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract).ToList();
                sub.ForEach(t => _builder.RegisterType(t).Named(t.Name, modelInterface).InstancePerLifetimeScope().OnActivated(e => RegisterProperties(e.Instance, e.Context)));
            }
        }

        /// <summary>
        /// 启动页面配置的自定义注册
        /// </summary>
        private void RegisterCustomers()
        {
            //获取要注册的类型信息
            var list = _startups.SelectMany(s => s.Register()).Distinct();
            foreach (var item in list)
            {
                // inType为空时，接口类型不能为空,要求接口类型实现IDependency，引用接口的类型都被注册
                if (item.InType == null)
                {
                    if (item.DType != null)
                    {
                        var types = GetTypes(item.DType);
                        if (item.IsShare)
                            types.ForEach(t => _builder.RegisterType(t).Named(t.Name, item.DType).InstancePerLifetimeScope());
                        else
                            types.ForEach(t => _builder.RegisterType(t).Named(t.Name, item.DType).InstancePerDependency());
                    }
                }
                //inType为泛型，接口类型必须为泛型
                else if (item.InType.IsGenericType)
                {
                    if (item.DType.IsGenericType)
                    {
                        if (item.IsShare)
                            _builder.RegisterGeneric(item.InType).As(item.DType).InstancePerLifetimeScope();
                        else
                            _builder.RegisterGeneric(item.InType).As(item.DType).InstancePerDependency();
                    }
                }
                // dType为空时，注入类型不能为空，根据注入类型的接口单一注册
                else if (item.DType == null)
                {
                    var inters = item.InType.GetInterfaces();
                    var first = inters.FirstOrDefault(x => typeof(IDependency).IsAssignableFrom(x));
                    if (first != null)
                        if (item.IsShare)
                            _builder.RegisterType(item.InType).As(first).InstancePerLifetimeScope();
                        else
                            _builder.RegisterType(item.InType).As(first).InstancePerDependency();
                    else if (inters.Any())
                        if (item.IsShare)
                            _builder.RegisterType(item.InType).As(inters.FirstOrDefault()).InstancePerLifetimeScope();
                        else
                            _builder.RegisterType(item.InType).As(inters.FirstOrDefault()).InstancePerDependency();

                }
                else//inType不为空，dType页不为空，按名字注册
                {
                    if (item.IsShare)
                        _builder.RegisterType(item.InType).Named(item.InType.Name, item.DType).InstancePerLifetimeScope();
                    else
                        _builder.RegisterType(item.InType).Named(item.InType.Name, item.DType).InstancePerDependency();
                }


            }

        }
        /// <summary>
        /// 属性注入
        /// note:PropertiesAutowired可以自动注入，但不好控制
        /// </summary>
        private void RegisterProperties(object obj, IComponentContext context)
        {
            var properties = obj.GetType().GetProperties().Where(p => p.PropertyType.IsInterface && p.CanWrite && p.IsDefined(typeof(DependencyPropertyAttribute), false)).ToList();
            //判断name的值
            properties.ForEach(p =>
            {
                var objAttrs = p.GetCustomAttributes(typeof(DependencyPropertyAttribute), false);
                if (objAttrs.Length > 0)
                {
                    var name = (objAttrs[0] as DependencyPropertyAttribute).Named;

                    if (string.IsNullOrEmpty(name))
                    {
                        var value = context.Resolve(p.PropertyType);
                        p.SetValue(obj, value, null);
                    }
                    else
                    {
                        var value = context.ResolveNamed(name, p.PropertyType);
                        p.SetValue(obj, value, null);
                    }
                }
            });
        }


        #region 注册方法



        #endregion







    }

}
