using Microsoft.EntityFrameworkCore;

namespace WebApiFacebookOAuth.Entities
{
    public class MyDbContext : DbContext
    {
        public DbSet<FacebookAccountEntity> FacebookAccounts { get; set; }

        public DbSet<UserProfilEntity> UserProfiles { get; set; }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}