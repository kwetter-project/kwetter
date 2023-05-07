using TweetService.Dtos;

namespace TweetService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewTweet(TweetPublishedDto tweetPublishedDto);
    }
}