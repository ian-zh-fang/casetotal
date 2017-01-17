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
            return Repository.Query<data.entity.CaseClasses>(t => t.IsDel == 0).ToList();
        }
    }
}
