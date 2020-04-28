using System;
using System.Text;

namespace KHBC.BPMS.Log
{
    public class Logger
    {
        public static void Debug(LogType type, object obj)
        {
            LoggerMaker.WriteLog(type, LogLevel.Debug, obj);
        }

        public static void Info(LogType type, object obj)
        {
            LoggerMaker.WriteLog(type, LogLevel.Info, obj);
        }

        public static void Warn(LogType type, object obj)
        {
            LoggerMaker.WriteLog(type, LogLevel.Warn, obj);
        }

        public static void Error(LogType type, object obj)
        {
            LoggerMaker.WriteLog(type, LogLevel.Error, obj);
        }

        public static void Fatal(LogType type, object obj)
        {
            LoggerMaker.WriteLog(type, LogLevel.Fatal, obj);
        }

        public static void DebugFormat(LogType type, string format, params object[] args)
        {
            LoggerMaker.WriteLog(type, LogLevel.Debug, string.Format(format, args));
        }

        public static void InfoFormat(LogType type, string format, params object[] args)
        {
            LoggerMaker.WriteLog(type, LogLevel.Info, string.Format(format, args));
        }

        public static void WarnFormat(LogType type, string format, params object[] args)
        {
            LoggerMaker.WriteLog(type, LogLevel.Warn, string.Format(format, args));
        }

        public static void ErrorFormat(LogType type, string format, params object[] args)
        {
            LoggerMaker.WriteLog(type, LogLevel.Error, string.Format(format, args));
        }

        public static void FatalFormat(LogType type, string format, params object[] args)
        {
            LoggerMaker.WriteLog(type, LogLevel.Fatal, string.Format(format, args));
        }


        public static void Debug(string type, object obj)
        {
            LoggerMaker.WriteLog(type, LogLevel.Debug, obj);
        }

        public static void Info(string type, object obj)
        {
            LoggerMaker.WriteLog(type, LogLevel.Info, obj);
        }

        public static void Warn(string type, object obj)
        {
            LoggerMaker.WriteLog(type, LogLevel.Warn, obj);
        }

        public static void Error(string type, object obj)
        {
            LoggerMaker.WriteLog(type, LogLevel.Error, obj);
        }

        public static void Fatal(string type, object obj)
        {
            LoggerMaker.WriteLog(type, LogLevel.Fatal, obj);
        }

        public static void DebugFormat(string type, string format, params object[] args)
        {
            LoggerMaker.WriteLog(type, LogLevel.Debug, string.Format(format, args));
        }

        public static void InfoFormat(string type, string format, params object[] args)
        {
            LoggerMaker.WriteLog(type, LogLevel.Info, string.Format(format, args));
        }

        public static void WarnFormat(string type, string format, params object[] args)
        {
            LoggerMaker.WriteLog(type, LogLevel.Warn, string.Format(format, args));
        }

        public static void ErrorFormat(string type, string format, params object[] args)
        {
            LoggerMaker.WriteLog(type, LogLevel.Error, string.Format(format, args));
        }

        public static void FatalFormat(string type, string format, params object[] args)
        {
            LoggerMaker.WriteLog(type, LogLevel.Fatal, string.Format(format, args));
        }

        public static void RegisterLogs(params string[] args)
        {
            LoggerMaker.AddLogName(args);
        }
        #region 便捷访问

        public static LoggerWrapper Main => new LoggerWrapper(LogType.Main);
        public static LoggerWrapper GateWay => new LoggerWrapper(LogType.Gateway);
        public static LoggerWrapper Device => new LoggerWrapper(LogType.Device);
        public static LoggerWrapper Sql => new LoggerWrapper(LogType.Sql);
        public static LoggerWrapper Net => new LoggerWrapper(LogType.Net);

        #endregion
    }

    /// <summary>
    /// 扩展
    /// </summary>
    public class LoggerWrapper
    {
        private readonly LogType _logType;

        public LoggerWrapper(LogType ltype)
        {
            _logType = ltype;
        }

        public void Info(string message)
        {
            LoggerMaker.WriteLog(_logType, LogLevel.Info, message);
        }

        public void Debug(string message)
        {
            LoggerMaker.WriteLog(_logType, LogLevel.Info, message);
        }

        public void Warn(string message)
        {
            LoggerMaker.WriteLog(_logType, LogLevel.Warn, message);
        }

        public void Error(string message)
        {
            LoggerMaker.WriteLog(_logType, LogLevel.Error, message);
        }
        public void Error(Exception ex)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("*************************************** \n");
            msg.AppendFormat(" 异常发生时间： {0} \n", DateTime.Now);
            msg.AppendFormat(" 异常类型： {0} \n", ex.HResult);
            msg.AppendFormat(" 导致当前异常的 Exception 实例： {0} \n", ex.InnerException);
            msg.AppendFormat(" 导致异常的应用程序或对象的名称： {0} \n", ex.Source);
            msg.AppendFormat(" 引发异常的方法： {0} \n", ex.TargetSite);
            msg.AppendFormat(" 异常堆栈信息： {0} \n", ex.StackTrace);
            msg.AppendFormat(" 异常消息： {0} \n", ex.Message);
            msg.Append("***************************************");
            LoggerMaker.WriteLog(_logType, LogLevel.Error, msg.ToString());
        }

        public void Fatal(string message)
        {
            LoggerMaker.WriteLog(_logType, LogLevel.Fatal, message);
        }
    }

    public enum LogType
    {
        Main,
        Gateway,
        Device,
        Net,
        Sql
    }

    public enum LogLevel
    {
        Info,
        Debug,
        Warn,
        Error,
        Fatal
    }
}

