namespace zh.fang.handle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 统计相关处理
    /// </summary>
    public class StatisticsHandle:Handle
    {
        // 1，分案件类型统计当日发生的次数
        // 2，分案件类型统计本周发生的次数
        // 3，分案件类型统计上一周发生的次数
        // 4，分案件类型统计本月发生的次数
        // 5，分案件类型统计上一月发生的次数

        public IEnumerable<Model.ClassesTotal> ClassesTotal(long timeToStart, long timeToEnd)
        {
            var now = TimeZone.CurrentTimeZone.ToLocalTime(DateTime.Now).Ticks;
            var query =
                from cls in Repository.Query<data.entity.CaseClasses>(t => t.IsDel == 0)
                join clsts in Repository.Query<data.entity.CaseClassesStatistics>(t => t.TotalDate >= timeToStart && t.TotalDate <= timeToEnd)
                on cls.Id equals clsts.ClassesId into cts
                from clstt in cts.DefaultIfEmpty()
                select new { ClassId = cls.Id, ClassName = cls.Name, ParentId = cls.ParentId, total = clstt };

            return
                query
                .ToList()
                .GroupBy(t => t.ClassId)
                .Select(t =>
                {
                    var cls = t.First();
                    return new Model.ClassesTotal
                    {
                        ClassId = cls.ClassId,
                        ClassName = cls.ClassName,
                        ParentId = cls.ParentId,
                        TotalCount = t.Where(x => x.total != null).Sum(x => x.total.CaseCount)
                    };
                })
                .ToList();
        }
    }
}
