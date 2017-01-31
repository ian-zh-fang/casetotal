namespace zh.fang.module
{
    using System.Collections.Generic;
    using common;

    /// <summary>
    /// 案件类型模块
    /// </summary>
    public class ClassesModule
    {
        /// <summary>
        /// 获取所有可用的案件类型
        /// </summary>
        /// <returns></returns>
        public IEnumerable<data.entity.CaseClasses> FetchAll()
        {
            using (var handler = new handle.CaseClassesHandle())
            {
                return handler.FetchAll();
            }
        }

        public bool AddCls(string name, string parentId, int alertVal)
        {
            if (string.IsNullOrWhiteSpace(parentId))
            {
                parentId = null;
            }

            using (var handler = new handle.CaseClassesHandle())
            {
                data.entity.CaseClasses parent = null;
                if (parentId != null)
                {
                    parent = handler.FetchOne(parentId);
                }

                var d = new data.entity.CaseClasses { Code = $"{parent?.Code}{Utility.GuidToString16()}", IsDel = 0, Name = name, ParentId = parentId, ThdVal = alertVal };
                return handler.Add(d);
            }
        }

        public bool UpgCls(string id, string name, int alertVal)
        {
            using (var handler = new handle.CaseClassesHandle())
            {
                var d = handler.FetchOne(id);
                if (d == null)
                {
                    return true;
                }

                d.Name = name;
                d.ThdVal = alertVal;
                return handler.Update(d);
            }
        }

        public bool DelCls(string id)
        {
            using (var handler = new handle.CaseClassesHandle())
            {
                return handler.Remove<data.entity.CaseClasses>(t => t.Id == id || t.ParentId == id);
            }
        }
    }
}
