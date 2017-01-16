namespace zh.fang.data.entity
{
    /// <summary>
    /// 实体树
    /// </summary>
    public abstract class EntityTree:DeleteableEntity
    {
        /// <summary>
        /// 父级主键
        /// </summary>
        public string ParentId { get; set; }
    }
}
