namespace zh.fang.data.entity
{
    /// <summary>
    /// 组织机构
    /// </summary>
    public class Orgnization:EntityTree
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
        /// 绿色等级最大值
        /// </summary>
        public short Glv { get; set; }

        /// <summary>
        /// 黄色等级最大值
        /// </summary>
        public short Ylv { get; set; }

        /// <summary>
        /// 橙色等级最大值
        /// </summary>
        public short Olv { get; set; }
    }
}
