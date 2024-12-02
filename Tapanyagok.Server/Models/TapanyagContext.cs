using Microsoft.EntityFrameworkCore;

namespace Tapanyagok.Server.Models
{
    public partial class TapanyagContext : DbContext
    {
        public TapanyagContext()
        {
        }

        public TapanyagContext(DbContextOptions<TapanyagContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Tapanyag> tapanyagok { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;user id=root;database=tapanyag", ServerVersion.Parse("10.4.24-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
