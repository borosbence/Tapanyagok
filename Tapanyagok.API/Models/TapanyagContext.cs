using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Tapanyagok.API.Models
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

        public virtual DbSet<Tapanyag> tapanyagok { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            modelBuilder.Entity<Tapanyag>(entity =>
            {
                entity.Property(e => e.energia).HasPrecision(10, 1);

                entity.Property(e => e.feherje).HasPrecision(10, 1);

                entity.Property(e => e.szenhidrat).HasPrecision(10, 1);

                entity.Property(e => e.zsir).HasPrecision(10, 1);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
