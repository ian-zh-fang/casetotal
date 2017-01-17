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

        /// <summary>
        /// 分案件类型统计指定时间段内的案件发生次数
        /// </summary>
        /// <param name="timeToStart"></param>
        /// <param name="timeToEnd"></param>
        /// <returns></returns>
        public IEnumerable<Model.ClassesTotal> ClassesTotal(long timeToStart, long timeToEnd)
        {
            var validCodes = new short[] { 0 };
            var query =
                from tts in Repository.Query<data.entity.CaseClassesStatistics>(t => t.TotalDate >= timeToStart && t.TotalDate <= timeToEnd)
                join clss in Repository.Query<data.entity.CaseClasses>(null) on tts.ClassesId equals clss.Id into clsitems
                from cls in clsitems.DefaultIfEmpty()
                where validCodes.Any(t => t == cls.IsDel)
                select new { total = tts, cls = cls };
            var items = query.ToList().Where(t => t.cls != null).GroupBy(t => t.cls)
                .Select(
                    t => new Model.ClassesTotal
                    {
                        ClassId = t.Key.Id,
                        ClassName = t.Key.Name,
                        ParentId = t.Key.ParentId,
                        TotalCount = t.Select(x => x.total.CaseCount).Sum()
                    }
                );

            foreach (var item in items)
            {
                item.TotalCount += items.Where(t => t.ParentId == item.ClassId).Select(t => t.TotalCount).Sum();
            }
            return items;
        }
    }
}
