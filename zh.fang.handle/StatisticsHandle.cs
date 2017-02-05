namespace zh.fang.handle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using zh.fang.common;

    /// <summary>
    /// 统计相关处理
    /// </summary>
    public class StatisticsHandle:Handle
    {
        /// <summary>
        /// 分案件类型统计指定时间段内的案件发生次数
        /// </summary>
        /// <param name="timeToStart"></param>
        /// <param name="timeToEnd"></param>
        /// <returns></returns>
        public IEnumerable<Model.ClassesTotal> ClassesTotal(long timeToStart, long timeToEnd)
        {
            var query =
                from tts in Repository.Query<data.entity.CaseClassesStatistics>(t => t.TotalDate >= timeToStart && t.TotalDate <= timeToEnd)
                join clss in Repository.Query<data.entity.CaseClasses>(t => NonDeletedStatus.Any(x => t.IsDel == x)) on tts.ClassesId equals clss.Id into clsitems
                from cls in clsitems.DefaultIfEmpty()
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
        
        /// <summary>
        /// 分组织机构和案件类型统计指定时间段内的案件发生次数
        /// </summary>
        /// <param name="timeToStart"></param>
        /// <param name="timeToEnd"></param>
        /// <returns></returns>
        public IEnumerable<Model.OrgClassesTotal> OrgClassesTotal(long timeToStart, long timeToEnd)
        {
            var query =
                from total in Repository.Query<data.entity.CaseClassesStatistics>(t => t.TotalDate >= timeToStart && t.TotalDate <= timeToEnd)
                join orgitem in Repository.Query<data.entity.Orgnization>(t => NonDeletedStatus.Any(x => t.IsDel == x)) on total.OrgId equals orgitem.Id into orgs
                from org in orgs.DefaultIfEmpty()
                join clsitem in Repository.Query<data.entity.CaseClasses>(t => NonDeletedStatus.Any(x => t.IsDel == x)) on total.ClassesId equals clsitem.Id into clsitems
                from cls in clsitems.DefaultIfEmpty()
                select new { total = total, org = org, cls=cls };
            var items =
                query.ToList().Where(t => t.org != null && t.cls != null).GroupBy(t => t.org)
                .Select(t => new Model.OrgClassesTotal
                {
                    OrgId = t.Key.Id,
                    OrgName = t.Key.Name,
                    ParentId = t.Key.ParentId,
                    Order = t.Key.Code.Length,
                    GVal = t.Key.Glv,
                    OVal = t.Key.Olv,
                    YVal = t.Key.Ylv,
                    ClassesTotals = t.GroupBy(x => x.cls).Select(x => new Model.ClassesTotal
                    {
                        ClassId = x.Key.Id,
                        ClassName = x.Key.Name,
                        ParentId = x.Key.ParentId,
                        TotalCount = x.Sum(y => y.total.CaseCount)
                    })
                });
            return items;
        }

        public data.entity.CaseClassesStatistics First(string orgId, string clsId, DateTime recordTime)
        {
            var today = recordTime.Date.ToUnixTime();
            return
                Repository.Query<data.entity.CaseClassesStatistics>(t => t.ClassesId == clsId && t.OrgId == orgId && t.TotalDate >= today).FirstOrDefault();
        }
    }
}
