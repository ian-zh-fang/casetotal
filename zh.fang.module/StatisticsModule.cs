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
            var tuple = CompareSwitchTimestamp(timepoint1, timepoint2);
            using (handle.Handle
                totalHandler = new handle.StatisticsHandle(),
                clsHandler = new handle.CaseClassesHandle())
            {
                var clsItems = ((handle.CaseClassesHandle)clsHandler).FetchAll();
                var clsTotalItems = ((handle.StatisticsHandle)totalHandler).ClassesTotal(tuple.Item1, tuple.Item2);
                var items =
                    from cls in clsItems
                    join tt in clsTotalItems on cls.Id equals tt.ClassId into totals
                    from total in totals.DefaultIfEmpty(new handle.Model.ClassesTotal { ClassId = cls.Id, ClassName = cls.Name, ParentId = cls.ParentId, TotalCount = clsTotalItems.Where(t => t.ParentId == cls.Id).Sum(t => t.TotalCount) })
                    select total;

                return items;
            }
        }

        /// <summary>
        /// 分组织机构和类型统计指定当天的案件发生次数
        /// </summary>
        /// <returns></returns>
        public IEnumerable<handle.Model.OrgClassesTotal> OrgClassTotalOnToday()
        {
            return OrgClassTotal(DateTime.Now.Date.ToUnixTime(), DateTime.Now.ToUnixTime());
        }

        /// <summary>
        /// 分组织机构和类型统计指定时间段内的案件发生次数
        /// </summary>
        /// <param name="timeToStart"></param>
        /// <param name="timeToEnd"></param>
        /// <returns></returns>
        public IEnumerable<handle.Model.OrgClassesTotal> OrgClassTotal(DateTime? timeToStart, DateTime? timeToEnd)
        {
            if (timeToStart == null && timeToEnd == null)
            {
                return OrgClassTotal(DateTime.Now.AddDays(-1).ToUnixTime(), DateTime.Now.ToUnixTime());
            }

            if (timeToStart == null)
            {
                return OrgClassTotal(DateTime.Now.ToUnixTime(), timeToEnd.Value.ToUnixTime());
            }

            if (timeToEnd == null)
            {
                return OrgClassTotal(timeToStart.Value.ToUnixTime(), DateTime.Now.ToUnixTime());
            }

            // @此处 timeToStart != null && timeToEnd != null
            return OrgClassTotal(timeToStart.Value.ToUnixTime(), timeToEnd.Value.ToUnixTime());
        }

        /// <summary>
        /// 分组织机构和类型统计指定时间段内的案件发生次数
        /// </summary>
        /// <param name="timeToStart"></param>
        /// <param name="timeToEnd"></param>
        /// <returns></returns>
        public IEnumerable<handle.Model.OrgClassesTotal> OrgClassTotal(long timeToStart, long timeToEnd)
        {
            var tuple = CompareSwitchTimestamp(timeToStart, timeToEnd);
            using (handle.Handle
                orgHandler = new handle.OrgHandle(),
                clsHandler = new handle.CaseClassesHandle(),
                totalHandler = new handle.StatisticsHandle())
            {
                var clsItems = ((handle.CaseClassesHandle)clsHandler).FetchAll();
                var orgItmes = ((handle.OrgHandle)orgHandler).FetchAll();
                var totalItems = ((handle.StatisticsHandle)totalHandler).OrgClassesTotal(timeToStart, timeToEnd);
                var items =
                    from org in orgItmes
                    join totals in totalItems on org.Id equals totals.OrgId into totalArr
                    from total in totalArr.DefaultIfEmpty(new handle.Model.OrgClassesTotal
                    {
                        OrgId = org.Id,
                        OrgName = org.Name,
                        ParentId = org.ParentId,
                        Order = org.Code.Length,
                        ClassesTotals = new handle.Model.ClassesTotal[0]
                    })
                    orderby total.Order descending
                    select total;
                // 取类型统计和类型的并集，确保每一个组织机构都有所有有效的类型
                var result = new List<handle.Model.OrgClassesTotal>();
                foreach (var item in items)
                {
                    var arr = new List<handle.Model.ClassesTotal>();
                    arr.AddRange(item.ClassesTotals);
                    var totals = result.Where(t => t.ParentId == item.OrgId).Select(t => t.ClassesTotals);
                    foreach (var total in totals)
                    {
                        arr.AddRange(total);
                    }

                    item.ClassesTotals = UnionClsTotal(arr, clsItems);
                    result.Add(item);
                }

                return result;
            }
        }

        // 取指定类型统计和指定类型的并集，并返回新的类型统计信息
        private IEnumerable<handle.Model.ClassesTotal> UnionClsTotal(IEnumerable<handle.Model.ClassesTotal> totalItems, IEnumerable<data.entity.CaseClasses> clsItems)
        {
            var items = totalItems.GroupBy(t => new { ClassId = t.ClassId, ClassName = t.ClassName, ParentId = t.ParentId })
                .Select(t =>
                new handle.Model.ClassesTotal
                {
                    ClassId = t.Key.ClassId,
                    ClassName = t.Key.ClassName,
                    ParentId = t.Key.ParentId,
                    TotalCount = t.Sum(x => x.TotalCount)
                });

            return
                from cls in clsItems
                join totalItem in items on cls.Id equals totalItem.ClassId into totalArr
                from total in totalArr.DefaultIfEmpty(new handle.Model.ClassesTotal
                {
                    ClassId = cls.Id,
                    ClassName = cls.Name,
                    ParentId = cls.ParentId,
                    TotalCount = items.Where(t => t.ParentId == cls.Id).Sum(t => t.TotalCount)
                })
                select total;
        }
        
        // 取空类型统计和指定类型的并集
        private IEnumerable<handle.Model.ClassesTotal> GetEmptyTotal(IEnumerable<data.entity.CaseClasses> clsItems)
        {
            var totalItems = new handle.Model.ClassesTotal[0];
            return UnionClsTotal(totalItems, clsItems);
        }
                
        // 比较两个值，并返回两个值组成的组元实例，这个组元的第一个值总是比第二个值要小
        private Tuple<long, long> CompareSwitchTimestamp(long timestamp1, long timestamp2)
        {
            if(timestamp1 <= timestamp2)
            {
                return Tuple.Create(timestamp1, timestamp2);
            }

            return (new common.Exchange.Int64Exchange()).ExchangeByPlus(timestamp1, timestamp2);
        }
    }
}
