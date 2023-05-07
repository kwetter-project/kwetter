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
            if (!context.NewsFeeds.Any())
            {
                Console.WriteLine("--> Seeding Data...");
                context.Tweets.AddRange(
                    new Tweet() { Message = "Dot Net" },
                    new Tweet() { Message = "My First Tweet" },
                    new Tweet() { Message = "How Why When" }
                );
                context.NewsFeeds.AddRange(
                    new NewsFeed()
                    {
                        UpdatedAt = "19/03/2021"
                    },
                    new NewsFeed()
                    {
                        UpdatedAt = "20/03/2021"
                    },
                    new NewsFeed()
                    {
                        UpdatedAt = "22/03/2021"
                    }
                );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}