using NewsFeedService.Dtos;
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
        void updateTweet(int tweetId, Tweet tweet);
        Tweet GetTweetById(int id);
        void DeleteTweet(int id);
        List<Tweet> GetTweetsByName(string username);


        //newsfeed
        void CreateNewsFeed(string userName);
        NewsFeedWithTweetsDto GetNewsFeedWithTweets(int newsFeedId);
        void DeleteNewsFeed(int newsFeedId);
        NewsFeed GetNewsFeedByUser(string userName);
    }
}