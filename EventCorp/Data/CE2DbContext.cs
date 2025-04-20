using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EventCorp.Models;

namespace EventCorpModels.Data
{
    public class CE2DbContext : IdentityDbContext<User>
    {
        public CE2DbContext(DbContextOptions<CE2DbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Administrador", NormalizedName = "ADMINISTRADOR" },
                new IdentityRole { Id = "2", Name = "Organizador", NormalizedName = "ORGANIZADOR" },
                new IdentityRole { Id = "3", Name = "Usuario", NormalizedName = "USUARIO" }
            );

            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}

