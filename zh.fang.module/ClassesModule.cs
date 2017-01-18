namespace zh.fang.module
{
    using System.Collections.Generic;

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
    }
}
