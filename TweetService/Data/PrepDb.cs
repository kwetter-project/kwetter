using Microsoft.EntityFrameworkCore;
using TweetService.Models;

namespace TweetService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }
        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("--> Attempting to apply migration...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }
            if (!context.Tweets.Any())
            {
                Console.WriteLine("--> Seeding Data...");
                context.Tweets.AddRange(
                    new Tweet() { Username = "Bob1023", Type = "Tweet", Content = "Today is Monday.", CreatedAt = DateTime.Now.AddHours(-12) },
                    new Tweet() { Username = "ToM_B", Type = "Retweet", Content = "1", CreatedAt = DateTime.Now.AddHours(-2) },
                    new Tweet() { Username = "Business_A", Type = "Ads", Content = "New Product" },
                    new Tweet() { Username = "Mary000", Type = "Tweet", Content = "Today is Thursday.", CreatedAt = DateTime.Now.AddHours(-22) }
                );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have tweet data");
            }
            if (!context.Likes.Any())
            {
                Console.WriteLine("--> Seeding Data...");
                context.Likes.AddRange(
                    new Like() { Username = "Bob1023", TweetId = 1, CreatedAt = DateTime.Now.AddHours(-1) },
                    new Like() { Username = "ToM_B", TweetId = 1, CreatedAt = DateTime.Now.AddHours(-2) }
                );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have Like data");
            }
        }
    }
}