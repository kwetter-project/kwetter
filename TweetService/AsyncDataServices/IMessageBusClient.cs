using TweetService.Dtos;

namespace TweetService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewTweet(TweetPublishedDto tweetPublishedDto);
        void PublishNewLike(LikePublishedDto likePublishedDto);
        void PublishDeleteTweet(TweetDeletedDto tweetDeletedDto);
        void PublishDeleteLike(LikeDeletedDto likeDeletedDto);
    }
}