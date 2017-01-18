namespace zh.fang.handle
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 组织机构相关处理
    /// </summary>
    public class OrgHandle:Handle
    {
        public IEnumerable<data.entity.Orgnization> FetchAll()
        {
            return Repository.Query<data.entity.Orgnization>(t => NonDeletedStatus.Any(x => t.IsDel == x)).ToList();
        }
    }
}
