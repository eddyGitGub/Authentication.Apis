using Microsoft.EntityFrameworkCore;

namespace Authentication.Apis.Models
{
    public class AuthDbDbContext: DbContext
    {
        public AuthDbDbContext(DbContextOptions<AuthDbDbContext> options)
          : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
    }
}
