using System;
using KHBC.Core;

namespace KHBC.DataAccess
{
    /// <summary>
    /// 数据访问服务
    /// </summary>
    public class DBConf
    {
        /// <summary>
        /// sql日志
        /// </summary>
        public static event Action<string> DbSqlLoged;
        /// <summary>
        /// sql错误日志
        /// </summary>
        public static event Action<string> DbSqlErrorLoged;

        /// <summary>
        /// 输出sql执行日志
        /// </summary>
        /// <param name="msg"></param>
        internal static void WriteLog(string msg)
        {
            DbSqlLoged?.Invoke(msg);
        }

        /// <summary>
        /// 输出错误日志
        /// </summary>
        /// <param name="ex"></param>
        internal static void WriteErrorLog( string ex)
        {
            DbSqlErrorLoged?.Invoke(ex);
        }
    }
}
