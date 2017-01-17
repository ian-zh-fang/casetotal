namespace zh.fang.module
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using zh.fang.common;

    /// <summary>
    /// 统计模块数据
    /// </summary>
    public class StatisticsModule
    {
        /// <summary>
        /// 分类统计当天数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<handle.Model.ClassesTotal> ClassesTotalOnToday()
        {
            return ClassesTotal(DateTime.Now.Date.ToUnixTime(), DateTime.Now.ToUnixTime());
        }

        /// <summary>
        /// 分类统计本周数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<handle.Model.ClassesTotal> ClassesTotalOnCurrentWeek()
        {
            return ClassesTotal(DateTime.Now.FirstDayCurrentweeek().ToUnixTime(), DateTime.Now.ToUnixTime());
        }

        /// <summary>
        /// 分类统计上一周数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<handle.Model.ClassesTotal> ClassesTotalOnPreviousWeek()
        {
            var firstDayCurrentWeek = DateTime.Now.FirstDayCurrentweeek();
            return ClassesTotal(firstDayCurrentWeek.AddDays(-7).ToUnixTime(), firstDayCurrentWeek.ToUnixTime());
        }

        /// <summary>
        /// 分类统计本月数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<handle.Model.ClassesTotal> ClassesTotalOnCurrentMonth()
        {
            return ClassesTotal(DateTime.Now.FirstDayCurrentMonth().ToUnixTime(), DateTime.Now.ToUnixTime());
        }

        /// <summary>
        /// 分类统计上一月的数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<handle.Model.ClassesTotal> ClassesTotalOnPreMonth()
        {
            var firstDayCurrentMonth = DateTime.Now.FirstDayCurrentMonth();
            return ClassesTotal(DateTime.Now.FirstDayPreviousMonth().ToUnixTime(), DateTime.Now.FirstDayCurrentMonth().ToUnixTime());
        }

        /// <summary>
        /// 分类统计信息
        /// </summary>
        /// <param name="timeToStart"></param>
        /// <param name="timeToEnd"></param>
        /// <returns></returns>
        public IEnumerable<handle.Model.ClassesTotal> ClassesTotal(DateTime? timeToStart, DateTime? timeToEnd)
        {
            if (timeToStart == null && timeToEnd == null)
            {
                return ClassesTotal(DateTime.Now.AddDays(-1).ToUnixTime(), DateTime.Now.ToUnixTime());
            }

            if (timeToStart == null)
            {
                return ClassesTotal(DateTime.Now.ToUnixTime(), timeToEnd.Value.ToUnixTime());
            }

            if (timeToEnd == null)
            {
                return ClassesTotal(timeToStart.Value.ToUnixTime(), DateTime.Now.ToUnixTime());
            }

            // @此处 timeToStart != null && timeToEnd != null
            return ClassesTotal(timeToStart.Value.ToUnixTime(), timeToEnd.Value.ToUnixTime());
        }

        /// <summary>
        /// 分类统计指定时间段内的案件发生次数
        /// </summary>
        /// <param name="timepoint1"></param>
        /// <param name="timepoint2"></param>
        /// <returns></returns>
        public IEnumerable<handle.Model.ClassesTotal> ClassesTotal(long timepoint1, long timepoint2)
        {
            if (timepoint1 > timepoint2)
            {
                var exchange = new common.Exchange.Int64Exchange();
                var tuple = exchange.ExchangeByShift(timepoint1, timepoint2);
                timepoint1 = tuple.Item1;
                timepoint2 = tuple.Item2;
            }
            using (handle.Handle
                totalHandler = new handle.StatisticsHandle(),
                clsHandler = new handle.CaseClassesHandle())
            {
                var clsItems = ((handle.CaseClassesHandle)clsHandler).FetchAll();
                var clsTotalItems = ((handle.StatisticsHandle)totalHandler).ClassesTotal(timepoint1, timepoint2);
                var items =
                    from cls in clsItems
                    join tt in clsTotalItems on cls.Id equals tt.ClassId into totals
                    from total in totals.DefaultIfEmpty(new handle.Model.ClassesTotal { ClassId = cls.Id, ClassName = cls.Name, ParentId = cls.ParentId, TotalCount = clsTotalItems.Where(t => t.ParentId == cls.Id).Sum(t => t.TotalCount) })
                    select total;

                return items;
            }
        }
    }
}
