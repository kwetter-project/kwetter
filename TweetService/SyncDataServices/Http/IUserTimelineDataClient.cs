using TweetService.Dtos;

namespace TweetService.SyncDataServices.Http
{
    public interface INewsFeedDataClient
    {
        Task SendTweetToNewsFeed(TweetReadDto tweet);
    }
}