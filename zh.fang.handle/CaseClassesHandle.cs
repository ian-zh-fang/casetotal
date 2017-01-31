namespace zh.fang.handle
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 案件分类管理
    /// </summary>
    public class CaseClassesHandle:Handle
    {
        public IEnumerable<data.entity.CaseClasses> FetchAll()
        {
            return Repository.Query<data.entity.CaseClasses>(t => NonDeletedStatus.Any(x => t.IsDel == x)).ToList();
        }

        public data.entity.CaseClasses FetchOne(string id)
        {
            return
                Repository.Query<data.entity.CaseClasses>(t => t.Id == id).FirstOrDefault();
        }
    }
}
