namespace zh.fang.data.entity
{
    /// <summary>
    /// 权限
    /// </summary>
    public class Auth:BaseEntity
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        public string MenuId { get; set; }
    }
}
