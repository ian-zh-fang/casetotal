namespace zh.fang.handle.Model
{
    public class ClassesTotal
    {
        public string ClassId { get; set; }

        public string ClassName { get; set; }

        public int TotalCount { get; set; }

        public string ParentId { get; set; }
    }

    public class OrgClassesTotal
    {
        public string OrgId { get; set; }

        public string OrgName { get; set; }

        public short GVal { get; set; }

        public short YVal { get; set; }

        public short OVal { get; set; }

        public string ParentId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        public System.Collections.Generic.IEnumerable<ClassesTotal> ClassesTotals { get; set; }
    }
}
