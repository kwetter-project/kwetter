using NewsFeedService.Models;

namespace NewsFeedService.Data
{
    public interface INewsFeedRepo
    {
        bool SaveChanges();

        //Tweet related stuff
        IEnumerable<Tweet> GetAllTweets();
        void CreateTweet(Tweet tweet);
        bool TweetExist(int tweetId);
        bool ExternalTweetExist(int externalTweetId);

        //newsfeed
        IEnumerable<NewsFeed> GetNewsFeedsForTweet(int tweetId);
        NewsFeed GetNewsFeed(int tweetId, int newsFeedId);
        void CreateNewsFeed(int tweetId, NewsFeed newsFeed);
    }
}