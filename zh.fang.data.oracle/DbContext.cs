using System.Data.Entity;

namespace zh.fang.data.oracle
{
    public class DbContext:System.Data.Entity.DbContext
    {
        private readonly string UserName;

        public DbContext(string connectionStringName, string userName)
            :base($"name={connectionStringName}")
        {
            UserName = userName;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema($"{UserName.ToUpper()}");
            modelBuilder.Entity<entity.Auth>().ToTable("tb_Auth");
            modelBuilder.Entity<entity.CaseClasses>().ToTable("tb_CaseClasses");
            modelBuilder.Entity<entity.CaseClassesStatistics>().ToTable("tb_CaseClassesStatistics");
            modelBuilder.Entity<entity.Menu>().ToTable("tb_Menu");
            modelBuilder.Entity<entity.Orgnization>().ToTable("tb_Orgnization");
            modelBuilder.Entity<entity.Role>().ToTable("tb_Role");
            modelBuilder.Entity<entity.User>().ToTable("tb_User");
            modelBuilder.Entity<entity.Config>().ToTable("tb_Config");
        }

        public virtual DbSet<entity.Auth> AuthentizationItems { get; set; }

        public virtual DbSet<entity.CaseClasses> CaseClassesItems { get; set; }

        public virtual DbSet<entity.CaseClassesStatistics> CaseClassesStaticsItems { get; set; }

        public virtual DbSet<entity.Menu> MenuItems { get; set; }

        public virtual DbSet<entity.Orgnization> OrgnizationItems { get; set; }

        public virtual DbSet<entity.Role> RoleItems { get; set; }

        public virtual DbSet<entity.User> UserItems { get; set; }

        public virtual DbSet<entity.Config> Configs { get; set; }
    }
}
