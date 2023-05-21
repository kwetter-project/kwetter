using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<User> Users { get; set; }

    }
}