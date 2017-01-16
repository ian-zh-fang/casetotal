namespace zh.fang.data.entity
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Role:DeleteableEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
    }
}
