using TweetService.Dtos;

namespace TweetService.SyncDataServices.Http
{
    public interface IUserTimelineDataClient
    {
        Task SendTweetToUserTimeline(TweetReadDto tweet);
    }
}