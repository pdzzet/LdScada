using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace KHBC.Core.Extend
{
    /// <summary>
    /// 不同类型按属性名直接clone
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    internal static class ObjectClone<TIn, TOut> where TIn : class where TOut : class
    {
        private static Func<TIn, TOut> MapFunc { get; set; }

        private static Action<TIn, TOut> MapAction { get; set; }

        /// <summary>
        /// 深克隆,将对象TIn转换为TOut
        /// </summary>
        /// <param name="tin"></param>
        /// <returns></returns>
        public static TOut Trans(TIn tin)
        {
            if (MapFunc == null)
                MapFunc = GetMapFunc();
            return MapFunc(tin);
        }

        public static List<TOut> MapList(IEnumerable<TIn> ins)
        {
            if (MapFunc == null)
                MapFunc = GetMapFunc();
            var result = new List<TOut>();
            foreach (var item in ins)
            {
                result.Add(MapFunc(item));
            }
            return result;
        }

        /// <summary>
        /// 浅克隆,将对象TIn的值赋给给TOut
        /// </summary>
        /// <param name="tin"></param>
        /// <param name="tout"></param>
        public static void Trans(TIn tin, TOut tout)
        {
            if (MapAction == null)
                MapAction = GetMapAction();
            MapAction(tin, tout);
        }


        private static Func<TIn, TOut> GetMapFunc()
        {
            var inType = typeof(TIn);
            var outType = typeof(TOut);


            //Func委托传入变量
            var parameter = Expression.Parameter(inType, "pIn");
            var memberBindings = new List<MemberBinding>();

            //传入值类型可能报错


            var outTypes = outType.GetProperties().Where(x => x.PropertyType.IsPublic && x.CanWrite);
            foreach (var outItem in outTypes)
            {
                //忽略大小写
                var inItem = inType.GetProperty(outItem.Name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);

                //判断实体的读写权限
                if (inItem == null || !inItem.CanRead || inItem.PropertyType.IsNotPublic)
                    continue;

                //标注NotMapped特性的属性忽略转换


                var inProperty = Expression.Property(parameter, inItem);

                //当非值类型且类型不相同时
                // if (!inItem.PropertyType.IsValueType && inItem.PropertyType != outItem.PropertyType)

                if (!inItem.PropertyType.IsValueType && !outItem.PropertyType.IsValueType && inItem.PropertyType != typeof(string))
                {
                    //判断都是(非泛型)class
                    if (inItem.PropertyType.IsClass && outItem.PropertyType.IsClass &&
                        !inItem.PropertyType.IsGenericType && !outItem.PropertyType.IsGenericType)
                    {
                        var expression = GetClassExpression(inProperty, inItem.PropertyType, outItem.PropertyType);
                        memberBindings.Add(Expression.Bind(outItem, expression));
                    }

                    //集合数组类型的转换
                    else if (typeof(IEnumerable).IsAssignableFrom(inItem.PropertyType) && typeof(IEnumerable).IsAssignableFrom(outItem.PropertyType))
                    {
                        var gInType = inItem.PropertyType.GetGenericArguments()[0];
                        var gOutType = outItem.PropertyType.GetGenericArguments()[0];
                        if ((gInType.IsValueType && gOutType.IsValueType) || (gInType == typeof(string) && gOutType == typeof(string)))
                        {
                            if (inItem.PropertyType == outItem.PropertyType)
                            {
                                memberBindings.Add(Expression.Bind(outItem, inProperty));
                            }
                        }
                        else
                        {
                            var expression = GetListExpression(inProperty, inItem.PropertyType, outItem.PropertyType);
                            memberBindings.Add(Expression.Bind(outItem, expression));
                        }
                    }

                    continue;
                }

                if (outItem.PropertyType != inItem.PropertyType)
                    continue;

                memberBindings.Add(Expression.Bind(outItem, inProperty));
            }

            //创建一个if条件表达式
            var test = Expression.NotEqual(parameter, Expression.Constant(null, inType));// p==null;
            var ifTrue = Expression.MemberInit(Expression.New(outType), memberBindings);
            var condition = Expression.Condition(test, ifTrue, Expression.Constant(null, outType));

            var lambda = Expression.Lambda<Func<TIn, TOut>>(condition, parameter);
            return lambda.Compile();
        }

        /// <summary>
        /// 类型是clas时赋值
        /// </summary>
        /// <param name="inProperty"></param>
        /// <param name="inType"></param>
        /// <param name="outType"></param>
        /// <returns></returns>
        private static Expression GetClassExpression(Expression inProperty, Type inType, Type outType)
        {
            //条件p.Item!=null    
            var testItem = Expression.NotEqual(inProperty, Expression.Constant(null, inType));

            //构造回调 ObjectClone<TIn, TOut>.Trans()
            var mapperType = typeof(ObjectClone<,>).MakeGenericType(inType, outType);
            var iftrue = Expression.Call(mapperType.GetMethod("Trans", new[] { inType }), inProperty);

            var conditionItem = Expression.Condition(testItem, iftrue, Expression.Constant(null, outType));

            return conditionItem;
        }

        /// <summary>
        /// 类型为集合时赋值
        /// </summary>
        /// <param name="inProperty"></param>
        /// <param name="inType"></param>
        /// <param name="outType"></param>
        /// <returns></returns>
        private static Expression GetListExpression(Expression inProperty, Type inType, Type outType)
        {
            //条件p.Item!=null    
            var testItem = Expression.NotEqual(inProperty, Expression.Constant(null, inType));

            //构造回调 Mapper<TIn, TOut>.MapList()
            var inArg = inType.IsArray ? inType.GetElementType() : inType.GetGenericArguments()[0];
            var outArg = outType.IsArray ? outType.GetElementType() : outType.GetGenericArguments()[0];
            var mapperType = typeof(ObjectClone<,>).MakeGenericType(inArg, outArg);

            var mapperExecMap = Expression.Call(mapperType.GetMethod(nameof(MapList), new[] { inType }), inProperty);

            Expression iftrue;
            if (outType == mapperExecMap.Type)
            {
                iftrue = mapperExecMap;
            }
            else if (outType.IsArray)//数组类型调用ToArray()方法
            {
                iftrue = Expression.Call(mapperExecMap, mapperExecMap.Type.GetMethod("ToArray"));
            }
            else if (typeof(IDictionary).IsAssignableFrom(outType))
            {
                iftrue = Expression.Constant(null, outType);//字典类型不转换
            }
            else
            {
                iftrue = Expression.Convert(mapperExecMap, outType);
            }

            var conditionItem = Expression.Condition(testItem, iftrue, Expression.Constant(null, outType));

            return conditionItem;
        }

        private static Action<TIn, TOut> GetMapAction()
        {
            var inType = typeof(TIn);
            var outType = typeof(TOut);
            //Func委托传入变量
            var inParameter = Expression.Parameter(inType, "p");

            var outParameter = Expression.Parameter(outType, "t");

            //创建一个表达式集合
            var expressions = new List<Expression>();

            var outTypes = outType.GetProperties().Where(x => x.PropertyType.IsPublic && x.CanWrite);
            foreach (var outItem in outTypes)
            {
                //忽略大小写
                var inItem = inType.GetProperty(outItem.Name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);

                //判断实体的读写权限
                if (inItem == null || !inItem.CanRead || inItem.PropertyType.IsNotPublic)
                    continue;

                //标注NotMapped特性的属性忽略转换


                var inProperty = Expression.Property(inParameter, inItem);
                var outProperty = Expression.Property(outParameter, outItem);

                //当非值类型
                //if (!inItem.PropertyType.IsValueType && inItem.PropertyType != outItem.PropertyType)
                if (!inItem.PropertyType.IsValueType && !outItem.PropertyType.IsValueType && inItem.PropertyType != typeof(string))
                {
                    //判断都是(非泛型)class
                    if (inItem.PropertyType.IsClass && outItem.PropertyType.IsClass &&
                        !inItem.PropertyType.IsGenericType && !outItem.PropertyType.IsGenericType)
                    {
                        var expression = GetClassExpression(inProperty, inItem.PropertyType, outItem.PropertyType);
                        expressions.Add(Expression.Assign(outProperty, expression));
                    }

                    //集合数组类型的转换
                    if (typeof(IEnumerable).IsAssignableFrom(inItem.PropertyType) && typeof(IEnumerable).IsAssignableFrom(outItem.PropertyType))
                    {
                        var expression = GetListExpression(inProperty, inItem.PropertyType, outItem.PropertyType);
                        expressions.Add(Expression.Assign(outProperty, expression));
                    }

                    continue;
                }

                if (outItem.PropertyType != inItem.PropertyType)
                    continue;


                expressions.Add(Expression.Assign(outProperty, inProperty));
            }

            //当out!=null判断in是否为空
            var testin = Expression.NotEqual(inParameter, Expression.Constant(null, inType));
            var ifTruein = Expression.Block(expressions);
            var conditionin = Expression.IfThen(testin, ifTruein);

            //判断out是否为空
            var testout = Expression.NotEqual(outParameter, Expression.Constant(null, outType));
            var conditionout = Expression.IfThen(testout, conditionin);

            var lambda = Expression.Lambda<Action<TIn, TOut>>(conditionout, inParameter, outParameter);
            return lambda.Compile();
        }
    }

    /// <summary>
    /// 对象克隆类
    /// </summary>
    public static class ObjectClone
    {
        private static Action<T, T> CreateCloneMethod<T>()
        {
            var type = typeof(T);
            ParameterExpression TOut = Expression.Parameter(type, "TOut"),
            TIn = Expression.Parameter(type, "TIn");
            var lsExp = new List<BinaryExpression>();
            var props = type.GetProperties();
            foreach (PropertyInfo item in props)
            {
                if (!item.CanWrite)
                    continue;
                MemberExpression originalMember = Expression.Property(TIn, item);
                MemberExpression newMember = Expression.Property(TOut, item);
                BinaryExpression setValue = Expression.Assign(originalMember, newMember);
                lsExp.Add(setValue);
            }
            var body = Expression.Block(typeof(void), lsExp);
            return Expression.Lambda<Action<T, T>>(body, TOut, TIn).Compile();
        }

        /// <summary>
        /// 深克隆（不支持List和引用类型的属性）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">当前对象</param>
        /// <returns></returns>
        public static T Clone<T>(this T instance)
        {
            var newIns = Activator.CreateInstance<T>();
            var cloneMethod = CreateCloneMethod<T>();
            cloneMethod(instance, newIns);
            return newIns;
        }

        /// <summary>
        /// 加强版克隆
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static T CloneEx<T>(this T instance) where T : class, new()
        {
            return ObjectClone<T, T>.Trans(instance);
        }

        public static T2 CloneEx<T1, T2>(this T1 instance) where T1 : class, new() where T2 : class, new()
        {
            return ObjectClone<T1, T2>.Trans(instance);
        }
        /// <summary>
        /// 浅克隆
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">当前对象</param>
        /// <param name="target">目标对象</param>
        /// <returns></returns>
        public static T Clone<T>(this T instance, T target)
        {
            var cloneMethod = CreateCloneMethod<T>();
            cloneMethod(instance, target);
            return target;
        }
    }

}
