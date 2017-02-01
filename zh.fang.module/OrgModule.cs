namespace zh.fang.module
{
    using System.Collections.Generic;
    using common;

    public class OrgModule
    {
        public IEnumerable<data.entity.Orgnization> FetchAll()
        {
            using (var handler = new handle.OrgHandle())
            {
                return handler.FetchAll();
            }
        }

        public bool DelOrg(string id)
        {
            using (var handler = new handle.OrgHandle())
            {
                return handler.Remove<data.entity.Orgnization>(t => t.Id == id || t.ParentId == id);
            }
        }

        public bool AddOrg(string name, string parentId, short gVal, short yVal, short oVal)
        {
            using (var handler = new handle.OrgHandle())
            {
                if (string.IsNullOrWhiteSpace(parentId))
                {
                    parentId = null;
                }

                data.entity.Orgnization parent = null;
                if (parent == null)
                {
                    parent = handler.FetchOne(parentId);
                }

                var d = new data.entity.Orgnization { Code = $"{parent?.Code}{Utility.GuidToString16()}", Glv = gVal, IsDel = 0, Olv = oVal, Name = name, ParentId = parentId, Ylv = yVal };
                return handler.Add(d);                
            }
        }

        public bool UpgOrg(string id, string name, short gVal, short yVal, short oVal)
        {
            using (var handler = new handle.OrgHandle())
            {
                var d = handler.FetchOne(id);
                d.Glv = gVal;
                d.Name = name;
                d.Olv = oVal;
                d.Ylv = yVal;
                return handler.Update(d);
            }
        }
    }
}
