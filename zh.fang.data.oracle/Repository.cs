namespace zh.fang.data.oracle
{
    /// <summary>
    /// 基于 ORACLE 的数据仓库
    /// </summary>
    public class Repository : repository.EntityFrameworkRepository, repository.IRepository, repository.IUnit, System.IDisposable
    {
        public Repository(string connectionStringName, string userName) :
            base(new zh.fang.data.oracle.DbContext(connectionStringName, userName))
        {
        }

        public Repository(string userName)
            : this("OracleDbContext", userName)
        {
        }
    }
}
