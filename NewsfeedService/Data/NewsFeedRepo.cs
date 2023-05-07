using NewsFeedService.Models;

namespace NewsFeedService.Data
{
    public class NewsFeedRepo : INewsFeedRepo
    {
        private readonly AppDbContext _context;

        public NewsFeedRepo(AppDbContext context)
        {
            _context = context;
        }
        public void CreateTweet(Tweet tweet)
        {
            if (tweet == null)
            {
                throw new ArgumentNullException(nameof(tweet));
            }
            _context.Tweets.Add(tweet);
        }

        public void CreateNewsFeed(int tweetId, NewsFeed newsFeed)
        {
            if (newsFeed == null)
            {
                throw new ArgumentNullException(nameof(newsFeed));

            }
            newsFeed.TweetID = tweetId;
            _context.NewsFeeds.Add(newsFeed);
        }

        public bool ExternalTweetExist(int externalTweetId)
        {
            return _context.Tweets.Any(p => p.ExternalID == externalTweetId);
        }

        public IEnumerable<Tweet> GetAllTweets()
        {
            return _context.Tweets.ToList();
        }

        public NewsFeed GetNewsFeed(int tweetId, int newsFeedId)
        {
            return _context.NewsFeeds
                .Where(c => c.TweetID == tweetId && c.Id == newsFeedId).FirstOrDefault();
        }

        public IEnumerable<NewsFeed> GetNewsFeedsForTweet(int tweetId)
        {
            return _context.NewsFeeds
                .Where(c => c.TweetID == tweetId)
                .OrderBy(c => c.Tweet.Message);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public bool TweetExist(int tweetId)
        {
            return _context.Tweets.Any(p => p.Id == tweetId);
        }
    }
}