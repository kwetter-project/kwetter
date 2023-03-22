using Microsoft.EntityFrameworkCore;
using UserTimelineService.Models;

namespace UserTimelineService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        public DbSet<UserTimeline> UserTimeline { get; set; }
    }
}