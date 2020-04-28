using System;

namespace KHBC.Core.Extend
{
    /// <summary>
    /// 日期扩展方法类
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// 日期转字符串 yyyy/MM/dd HH:mm:ss
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string ToFormatString(this DateTime obj)
        {
            return obj.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <param name="nowTime">当前时间</param>
        /// <returns>返回时间(秒)</returns>
        public static int DateNowtoStamp(DateTime nowTime)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(nowTime - startTime).TotalSeconds;
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="time">当前时间</param>
        /// <returns>返回时间(秒)</returns>
        public static long ToStamp(this DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }

        /// <summary>
        /// 当月有多少天
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int DaysInMonth(int year, int month)
        {
            int days;
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12: days = 31; break;
                case 4:
                case 6:
                case 9:
                case 11: days = 30; break;
                case 2:
                    if ((year % 100 != 0 && year % 4 == 0) || (year % 400 == 0)) days = 29;
                    else days = 28;
                    break;
                default: days = 0; break;
            }
            return days;
        }
    }
}
