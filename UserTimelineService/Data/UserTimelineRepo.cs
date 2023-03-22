using UserTimelineService.Models;

namespace UserTimelineService.Data
{
    public class UserTimelineRepo : IUserTimelineRepo
    {
        private readonly AppDbContext _context;

        public UserTimelineRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreateUserTimeline(UserTimeline UserTimeline)
        {
            if (UserTimeline == null)
            {
                throw new ArgumentNullException(nameof(UserTimeline));
            }
            _context.UserTimeline.Add(UserTimeline);
        }

        public IEnumerable<UserTimeline> GetAllUserTimeline()
        {
            return _context.UserTimeline.ToList();
        }

        public UserTimeline GetUserTimelineById(int id)
        {
            return _context.UserTimeline.FirstOrDefault
            (p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}