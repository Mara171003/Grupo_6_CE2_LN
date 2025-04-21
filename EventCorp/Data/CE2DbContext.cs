using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EventCorp.Models;
using System.Reflection.Emit;

namespace EventCorpModels.Data
{
    public class CE2DbContext : IdentityDbContext<User>
    {
        public CE2DbContext(DbContextOptions<CE2DbContext> options) : base(options){ 
        
        }
   
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Inscripcion> Inscripciones {  get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Category> Category { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) { 
       
            base.OnModelCreating(builder);

           
            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<Inscripcion>()
                .HasOne(i => i.Evento)
                .WithMany(e => e.Inscripciones)  
                .HasForeignKey(i => i.EventoId);

            builder.Entity<Inscripcion>()
                .HasOne(i => i.Usuario)
                .WithMany(u => u.Inscripciones)  
                .HasForeignKey(i => i.UserId);
}
    }

}

