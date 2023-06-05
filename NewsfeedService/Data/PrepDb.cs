using Microsoft.EntityFrameworkCore;
using NewsFeedService.Models;

namespace NewsFeedService.Data
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
            //isProd = true;
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
                Console.WriteLine("--> Seeding Data tweet...");
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
                Console.WriteLine("--> We already have data tweet");
            }
            if (!context.Followers.Any())
            {
                Console.WriteLine("--> Seeding Data follower...");
                context.Followers.AddRange(
                    new Follower() { FolloweeName = "Mary000", FollowerName = "ToM_B" },
                    new Follower() { FolloweeName = "Business_A", FollowerName = "ToM_B" },
                    new Follower() { FolloweeName = "Mary000", FollowerName = "Bob1023" },
                    new Follower() { FolloweeName = "Mary000", FollowerName = "Business_A" }
                );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data follower");
            }
        }
    }
}