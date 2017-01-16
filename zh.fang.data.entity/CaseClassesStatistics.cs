namespace zh.fang.data.entity
{
    /// <summary>
    /// 区域分类案件统计
    /// </summary>
    public class CaseClassesStatistics:BaseEntity
    {
        /// <summary>
        /// 案件类型ID
        /// </summary>
        public string ClassesId { get; set; }

        /// <summary>
        /// 行政区划ID
        /// </summary>
        public string OrgId { get; set; }

        /// <summary>
        /// 案发数量
        /// </summary>
        public int CaseCount { get; set; }

        /// <summary>
        /// 统计时间
        /// </summary>
        public long TotalDate { get; set; }
    }
}
