namespace zh.fang.data.entity
{
    /// <summary>
    /// 案件类型
    /// </summary>
    public class CaseClasses:EntityTree
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 预警阈值
        /// </summary>
        public int ThdVal { get; set; }
    }
}
