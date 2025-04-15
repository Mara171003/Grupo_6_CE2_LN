using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventCorpModels.Data
{
    public class CE2DbContext : IdentityDbContext<User>
    {
        public CE2DbContext(DbContextOptions<CE2DbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "ADMIN", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "ORGANIZADOR", NormalizedName = "ORGANIZADOR" },
                new IdentityRole { Id = "3", Name = "USER", NormalizedName = "USER" }
            );

            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}

