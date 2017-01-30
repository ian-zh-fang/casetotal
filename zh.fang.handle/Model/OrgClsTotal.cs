namespace zh.fang.handle.Model
{
    using System;

    public class OrgClsTotal
    {
        public string orgId { get; set; }

        public string clsId { get; set; }

        public int count { get; set; }

        public DateTime totalTime => DateTime.Now;
    }
}
