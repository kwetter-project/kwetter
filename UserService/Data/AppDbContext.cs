using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.Username)
                .IsClustered(false); // Disable clustered index on the primary key

            modelBuilder.Entity<User>()
                .HasIndex(u => u.ShardKey)
                .IsClustered(true); // Enable clustered index on the shard key
        }
    }
}