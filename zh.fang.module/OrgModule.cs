namespace zh.fang.module
{
    using System.Collections.Generic;

    public class OrgModule
    {
        public IEnumerable<data.entity.Orgnization> FetchAll()
        {
            using (var handler = new handle.OrgHandle())
            {
                return handler.FetchAll();
            }
        }
    }
}
