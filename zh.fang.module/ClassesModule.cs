namespace zh.fang.module
{
    using System.Collections.Generic;

    public class ClassesModule
    {
        public IEnumerable<data.entity.CaseClasses> FetchAll()
        {
            using (var handler = new handle.CaseClassesHandle())
            {
                return handler.FetchAll();
            }
        }
    }
}
