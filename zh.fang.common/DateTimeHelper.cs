namespace zh.fang.common
{
    using System;

    /// <summary>
    /// 时间对象相关扩展
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// 获取指定时区的指定时间节点
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeZone"></param>
        /// <returns></returns>
        public static DateTime ToLocalTime(this DateTime time, TimeZone timeZone = null)
        {
            timeZone = timeZone ?? TimeZone.CurrentTimeZone;
            return timeZone.ToLocalTime(time);
        }

        /// <summary>
        /// 获取指定时间的第一周第一天开始时间
        /// </summary>
        /// <param name="time">指定的时间</param>
        /// <param name="seek">每周开始时间节点</param>
        /// <returns></returns>
        public static DateTime FirstDayCurrentweeek(this DateTime time, DayOfWeek seek = DayOfWeek.Monday)
        {
            if (seek == time.DayOfWeek)
            {
                return time.Date;
            }

            var offset = (int)time.DayOfWeek;
            if (offset == 0)
            {
                offset = 7;
            }
            return time.AddDays((int)seek - offset).Date;
        }

        /// <summary>
        /// 获取指定时间所在月份的开始时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime FirstDayCurrentMonth(this DateTime time)
        {
            return new DateTime(time.Year, time.Month, 1);
        }

        /// <summary>
        /// 获取指定时间上一个月份的开始时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime FirstDayPreviousMonth(this DateTime time)
        {
            return time.FirstDayCurrentMonth().AddMonths(-1);
        }

        /// <summary>
        /// 获取指定时区的指定时间的自 1970 年 1 月 1 日 的时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeZone"></param>
        /// <returns></returns>
        public static long ToUnixTime(this DateTime time, TimeZone timeZone = null)
        {
            timeZone = timeZone ?? null;
            return (time.ToLocalTime(timeZone) - UnixTimeSeed().ToLocalTime(timeZone)).Ticks;
        }

        // 创建 1970 年 1 月 1 日 0 时 0 分 0 秒 时刻的时间
        private static DateTime UnixTimeSeed()
        {
            return new DateTime(1970, 1, 1, 0, 0, 0);
        }
    }
}
