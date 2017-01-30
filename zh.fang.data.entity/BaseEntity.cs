namespace zh.fang.data.entity
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 基础实体
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public string Id { get; set; } = common.Utility.GuidToString16();
    }

    /// <summary>
    /// 可逻辑删除实体
    /// </summary>
    public class DeleteableEntity:BaseEntity
    {

        /// <summary>
        /// 是否已删除。0：未删除；1：已删除
        /// </summary>
        public short IsDel { get; set; }

    }
}
