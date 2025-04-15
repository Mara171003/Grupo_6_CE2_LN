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
    }
}
