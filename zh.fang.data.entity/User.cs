namespace zh.fang.data.entity
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User:DeleteableEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别。
        /// 0：未知；1：女性；2：男性；3：其它；
        /// </summary>
        public short Sex { get; set; }

        /// <summary>
        /// 账户
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Passwd { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public long SignupDate { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 用户状态。
        /// 0：正常；-1：异常；-2：冻结；
        /// </summary>
        public short Status { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        public string Avatar { get; set; }
    }
}
