using Microsoft.EntityFrameworkCore;
using NewsFeedService.Models;

namespace NewsFeedService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<NewsFeed> NewsFeeds { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Tweet>()
                .HasMany(p => p.NewsFeeds)
                .WithOne(p => p.Tweet!)
                .HasForeignKey(p => p.TweetID);

            modelBuilder
                .Entity<NewsFeed>()
                .HasOne(p => p.Tweet)
                .WithMany(p => p.NewsFeeds)
                .HasForeignKey(p => p.TweetID);
        }
    }
}