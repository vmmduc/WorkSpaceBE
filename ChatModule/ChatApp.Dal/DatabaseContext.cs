using Microsoft.EntityFrameworkCore;

namespace Models.Models
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext() { }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public virtual DbSet<USER> LS_USERS { get; set; } = null!;
        public virtual DbSet<USER_TOKEN> LS_USER_TOKEN { get; set; } = null!;
        public virtual DbSet<MESSAGE> LS_MESSAGES { get; set; } = null!;
        public virtual DbSet<FRIENDSHIP> LS_FRIENDSHIPS { get; set; } = null!;
        public virtual DbSet<USER_ROLE> LS_USER_ROLES { get; set; } = null!;
        public virtual DbSet<ROLE> LS_ROLES { get; set; } = null!;
        public virtual DbSet<STATE> LS_STATES { get; set; } = null!;
        public virtual DbSet<CONVERSATION> LS_CONVERSATIONS { get; set; } = null!;
        public virtual DbSet<CONVERSATION_MEMBER> LS_CONVERSATION_MEMBERS { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=125.212.231.206;Initial Catalog=WorkSpace;Persist Security Info=True;User ID=sa;Password=sql@123456;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<USER>();

            modelBuilder.Entity<USER_TOKEN>();

            modelBuilder.Entity<MESSAGE>();

            modelBuilder.Entity<FRIENDSHIP>();

            modelBuilder.Entity<USER_ROLE>();

            modelBuilder.Entity<ROLE>();

            modelBuilder.Entity<STATE>();

            modelBuilder.Entity<CONVERSATION>();

            modelBuilder.Entity<CONVERSATION_MEMBER>();

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
