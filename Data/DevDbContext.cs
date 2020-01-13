using Microsoft.EntityFrameworkCore;


namespace Data
{
    public class DevDbContext : DbContext
    {
        public DevDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public virtual DbSet<UserDetails> UserDetails { get; set; }
    }
}