using UserTimelineService.Models;

namespace UserTimelineService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }
        private static void SeedData(AppDbContext context)
        {
            if (!context.UserTimeline.Any())
            {
                Console.WriteLine("--> Seeding Data...");
                context.UserTimeline.AddRange(
                    new UserTimeline() { TweetId = "1", UserId = "1", AuthorId = "1" },
                    new UserTimeline() { TweetId = "2", UserId = "2", AuthorId = "2" },
                    new UserTimeline() { TweetId = "3", UserId = "3", AuthorId = "3" },
                    new UserTimeline() { TweetId = "4", UserId = "4", AuthorId = "4" }
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