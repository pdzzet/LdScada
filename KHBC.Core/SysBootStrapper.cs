using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Text;
using KHBC.Core.Log;
using KHBC.Core.FrameBase;

namespace KHBC.Core
{
    /// <summary>
    /// 系统启动器
    /// </summary>
    internal static class SysBootStrapper
    {
        /// <summary>
        /// 此处影响加载本地dll和IStartUp注册
        /// </summary>
        public static string Prefix = "KHBC.";

        /// <summary>
        /// 系统资源初始化
        /// </summary>
        /// <param name="assemblyName">按产品区分加载资源和配置 </param>
        internal static void Initialize(string assemblyName)
        {
            StringBuilder sbr = new StringBuilder();
            try
            {
                var assembly = Assembly.GetEntryAssembly()?.GetName();
                //CopyFile();
                var stopwatch = Stopwatch.StartNew();
                //Logger.Main.Debug("开始加载DLL....");
                var assemblies = GetAssemblyPrefix();
                stopwatch.Stop();
                var startups = GetInstances<IStartup>(assemblies).OrderByDescending(x => x.Order).ToArray();
                ModuleBefore(startups);
                Logger.Main.Debug($"系统预初始化,加载插件{startups.Length}个 耗时{stopwatch.ElapsedMilliseconds}ms");
                stopwatch.Restart();
                var iocinitializer = new IocAutofacContainer(assemblies, startups);
                iocinitializer.Do();
                stopwatch.Stop();

                SysConf.SysContainer = iocinitializer;
                Logger.Main.Debug($"系统注册{SysConf.SysContainer.Container.ComponentRegistry.Registrations.Count()}个 耗时{stopwatch.ElapsedMilliseconds}ms");
                ModuleAfter(startups);
            }
            catch (Exception ex)
            {
                Logger.Main.Error($"{ex.Message}\r\n {ex.StackTrace}");
                throw ex;
            }
        }


        /// <summary>
        /// 是否接口的实例类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="baseType"></param>
        /// <returns></returns>
        public static bool IsSolidTypeOf(this Type type, Type baseType)
        {
            return baseType.IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface;
        }
        /// <summary>
        /// 所有非抽象类的接口实现实例
        /// </summary>
        /// <returns></returns>
        public static T[] GetInstances<T>(AssemblyInfo[] assemblies)
        {
            return assemblies.SelectMany(a => a.Types
                .Where(p => p.IsSolidTypeOf(typeof(T)))
                .Select(p => (T)Activator.CreateInstance(p))
                ).Reverse()
                .ToArray();
        }

        #region[插件内部初始化]

        private static void ModuleBefore(IStartup[] startups)
        {
            foreach (var startup in startups)
                startup.BeforeStartup();
        }
        private static void ModuleAfter(IStartup[] startups)
        {
            foreach (var startup in startups)
                startup.AfterStartup();
        }
        #endregion


        #region[相关引用程序集加载]

        /// <summary>
        /// 按引用加载
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        private static AssemblyInfo[] GetSortedAssembly(string assemblyName)
        {
            //加载依赖Dll
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var loadedAssemblies = new List<Assembly>(assemblies);
            foreach (var assembly in assemblies)
                LoadReferences(loadedAssemblies, assembly);
            if (!string.IsNullOrWhiteSpace(assemblyName))
                LoadReferences(loadedAssemblies, Assembly.Load(assemblyName));

            //构造节点
            var nodes = AppDomain.CurrentDomain.GetAssemblies()
                .Where(FilterAssembly)
                .Select(a => new AssemblyNode
                {
                    Assembly = a
                })
                .ToList();

            //获取各节点依赖
            foreach (var node in nodes)
                node.Depends = node.Assembly.GetReferencedAssemblies()
                    .Where(FilterAssembly)
                    .Select(an => nodes.Find(a => a.Assembly.FullName == an.FullName))
                    .ToList();

            //根据依赖顺序排序 被依赖优先
            var sortedAssemblies = new List<Assembly>();
            while (true)
            {
                var node = nodes.FirstOrDefault(n => !n.Depends.Any());
                if (node == null)
                    break;
                sortedAssemblies.Add(node.Assembly);
                foreach (var n in nodes)
                    n.Depends.Remove(node);
                nodes.Remove(node);
            }
            return sortedAssemblies
                .Select(a => new AssemblyInfo
                {
                    Assembly = a,
                    Types = a.GetTypes()
                })
                .ToArray();
        }

        /// <summary>
        /// 按前缀名字加载
        /// </summary>
        /// <returns></returns>
        private static AssemblyInfo[] GetAssemblyPrefix()
        {
            //系统必备dll
            try
            {
                var mustdlls = new[] { "KHBC.Core" };
                var filelist = System.IO.Directory.GetFiles(SysConf.BasePath, "*.dll").Select(System.IO.Path.GetFileNameWithoutExtension).ToList();
                var loadlist = filelist.Where(x => !mustdlls.Contains(x) && x.StartsWith(Prefix)).ToList();
                //动态加载模块
                foreach (var item in loadlist)
                {
                    Assembly.Load(item);
                    Logger.Main.Info($"加载DLL: {item}");
                }
            }
            catch (Exception ex)
            {
                Logger.Main.Debug($"加载DLL失败！{ex.Message}=={ex.StackTrace}");
            }

            //构造节点信息
            var nodes = new List<AssemblyInfo>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(FilterAssembly);

            foreach (var item in assemblies)
            {
                var info = new AssemblyInfo();
                info.Assembly = item;
                try
                {
                    var types = item.GetTypes();
                    info.Types = types;
                    nodes.Add(info);
                }
                catch (Exception ex)
                {
                    Logger.Main.Error($"{item.FullName}内部错误！");
                    throw ex;
                }

            }
            return nodes.ToArray();
        }


        /// <summary>
        /// 加载所有引用
        /// </summary>
        /// <param name="loadedAssemblies"></param>
        /// <param name="assembly"></param>
        private static void LoadReferences(List<Assembly> loadedAssemblies, Assembly assembly)
        {
            var toLoad = assembly.GetReferencedAssemblies()
                .Where(FilterAssembly)
                .Where(an => loadedAssemblies.TrueForAll(a => a.FullName != an.FullName));
            var newlyLoaded = toLoad.Select(an => AppDomain.CurrentDomain.Load(an)).ToList();
            loadedAssemblies.AddRange(newlyLoaded);
            foreach (var a in newlyLoaded)
                LoadReferences(loadedAssemblies, a);
        }

        private static bool FilterAssembly(Assembly a)
        {
            return a.FullName.Contains(Prefix);
        }

        private static bool FilterAssembly(AssemblyName a)
        {
            return a.FullName.Contains(Prefix);
        }

        private class AssemblyNode
        {
            public Assembly Assembly { get; set; }

            public List<AssemblyNode> Depends { get; set; }
        }


        #endregion

        #region 复制文件
        private static void CopyFile()
        {
#if DEBUG
            var refProject = "";
            string projectName = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName);
            if (!projectName.StartsWith(refProject))
                return;
            string dir = SysConf.BasePath.Replace("KHBC", refProject);
            if (!Directory.Exists(dir))
                return;
            var filelist = Directory.GetFiles(dir).Select(x => Path.GetFileName(x)).Where(x => x.EndsWith(".dll"));
            foreach (var item in filelist)
            {
                try
                {
                    File.Copy(Path.Combine(dir, item), Path.Combine(SysConf.BasePath, item), true);
                }
                catch (Exception)
                {

                }
            }


#endif
        }
        #endregion
    }

    /// <summary>
    /// 程序集节点信息
    /// </summary>
    public class AssemblyInfo
    {
        /// <summary>
        /// 当前程序集信息
        /// </summary>
        public Assembly Assembly { get; set; }

        /// <summary>
        /// 当前程序集所有类型
        /// </summary>
        public Type[] Types { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Assembly.FullName} #{Types.Length}";
        }
    }
}