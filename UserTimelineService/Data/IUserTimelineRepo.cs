using UserTimelineService.Models;

namespace UserTimelineService.Data
{
    public interface IUserTimelineRepo
    {
        bool SaveChanges();
        IEnumerable<UserTimeline> GetAllUserTimeline();
        UserTimeline GetUserTimelineById(int id);
        void CreateUserTimeline(UserTimeline userTl);
    }
}