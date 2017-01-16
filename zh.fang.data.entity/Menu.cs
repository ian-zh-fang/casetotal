namespace zh.fang.data.entity
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class Menu:EntityTree
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }
    }
}
