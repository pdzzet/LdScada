using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using System;
using System.Diagnostics;

namespace KHBC.Core.Log
{
    /// <summary>
    /// 日志配置
    /// </summary>
    internal class LoggerMaker
    {
        static public string AppName { get; set; } = "MAIN";

        static LoggerMaker()
        {
            AppName = Process.GetCurrentProcess().ProcessName;

            foreach (var s in Enum.GetNames(typeof(LogType)))
            {
                var repo = LogManager.CreateRepository(s);
                //  var adorep = new AdoNetAppender();
                var layout = new PatternLayout()
                {
                    ConversionPattern =
                      "[%date] [%2thread] %-5level %logger [%property{NDC}] - %message%newline",
                };
                var rfAppender = new RollingFileAppender()
                {
                    Name = s,
                    File = $"../../Logs/{AppName}/{DateTime.Now.ToString("yyyy-MM-dd")}/{s}.log",
                    PreserveLogFileNameExtension = true,
                    StaticLogFileName = false,

                    RollingStyle = RollingFileAppender.RollingMode.Size,
                    MaxFileSize = 10 * 1024 * 1024,
                    MaxSizeRollBackups = 10,

                    AppendToFile = true,
                    Layout = layout
                };
                layout.ActivateOptions();
                //    adorep.ActivateOptions();
                rfAppender.ActivateOptions();
                //  BasicConfigurator.Configure(repo, adorep, rfAppender);
                BasicConfigurator.Configure(repo, rfAppender);
            }
        }

        /// <summary>
        /// 动态添加log对象
        /// </summary>
        /// <param name="logs"></param>
        internal static void AddLogName(params string[] logNames)
        {
            if (logNames == null)
                return;
            foreach (var s in logNames)
            {
                var repo = LogManager.CreateRepository(s);
                //  var adorep = new AdoNetAppender();
                var layout = new PatternLayout()
                {
                    ConversionPattern =
                        "[%date] [%thread] %-5level %logger [%property{NDC}] - %message%newline",
                };
                var rfAppender = new RollingFileAppender()
                {
                    Name = s,
                    File = $"../../Logs/{AppName}/{DateTime.Now.ToString("yyyy-MM-dd")}/{s}.log",
                    PreserveLogFileNameExtension = true,
                    StaticLogFileName = false,

                    RollingStyle = RollingFileAppender.RollingMode.Size,
                    MaxFileSize = 10 * 1024 * 1024,
                    MaxSizeRollBackups = 10,

                    AppendToFile = true,
                    Layout = layout
                };
                layout.ActivateOptions();
                //    adorep.ActivateOptions();
                rfAppender.ActivateOptions();
                //  BasicConfigurator.Configure(repo, adorep, rfAppender);
                BasicConfigurator.Configure(repo, rfAppender);
            }
        }

        /// <summary>
        /// 基础写入方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <param name="message"></param>
        internal static void WriteLog(LogType type, LogLevel level, object message)
        {
            var logger = LogManager.GetLogger(type.ToString(), type.ToString());
            switch (level)
            {
                case LogLevel.Debug:
                    logger.Debug(GetMessageString(message));
                    break;

                case LogLevel.Info:
                    logger.Info(GetMessageString(message));
                    break;

                case LogLevel.Warn:
                    logger.Warn(GetMessageString(message));
                    break;

                case LogLevel.Error:
                    logger.Error(GetMessageString(message));
                    break;

                case LogLevel.Fatal:
                    logger.Fatal(GetMessageString(message));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }

        /// <summary>
        /// 基础写入方法
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="level"></param>
        /// <param name="message"></param>
        internal static void WriteLog(string typeName, LogLevel level, object message)
        {
            var logger = LogManager.GetLogger(typeName, typeName);
            switch (level)
            {
                case LogLevel.Debug:
                    logger.Debug(GetMessageString(message));
                    break;

                case LogLevel.Info:
                    logger.Info(GetMessageString(message));
                    break;

                case LogLevel.Warn:
                    logger.Warn(GetMessageString(message));
                    break;

                case LogLevel.Error:
                    logger.Error(GetMessageString(message));
                    break;

                case LogLevel.Fatal:
                    logger.Fatal(GetMessageString(message));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }
        private static string GetMessageString(object message)
        {
            if (message is string)
                return message.ToString();
            try
            {
                return message.ToJsonStr();
            }
            catch (Exception)
            {
                return message.ToString();
            }
        }


    }


}
