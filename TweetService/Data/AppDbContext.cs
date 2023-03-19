using Microsoft.EntityFrameworkCore;
using TweetService.Models;

namespace TweetService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        public DbSet<Tweet> Tweets { get; set; }
    }
}