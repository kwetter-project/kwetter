using NewsFeedService.Dtos;
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

        public void CreateNewsFeed(string userName)
        {
            var followers = _context.Followers.Where(c => c.FollowerName == userName).ToList();
            var createFeed = new NewsFeed();
            createFeed.UpdatedAt = DateTime.UtcNow;

            foreach (var follower in followers)
            {
                var newsfeed = _context.Tweets.Where(c => c.Username == follower.FolloweeName).ToList();
                createFeed.Tweets = newsfeed;
            }

            var newsfeedExist = GetNewsFeedByUser(userName);
            if (newsfeedExist == null)
            {
                _context.NewsFeeds.Add(createFeed);
            }
            else
            {
                _context.NewsFeeds.Attach(createFeed);
                var entry = _context.NewsFeeds.Entry(createFeed);
                entry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
        }

        public IEnumerable<Tweet> GetAllTweets()
        {
            return _context.Tweets.ToList();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public bool TweetExist(int tweetId)
        {
            return _context.Tweets.Any(p => p.Id == tweetId);
        }

        public Tweet GetTweetById(int id)
        {
            return _context.Tweets.FirstOrDefault
            (p => p.Id == id);
        }

        public void DeleteTweet(int id)
        {
            _context.Remove(_context.Tweets.Single(a => a.Id == id));
        }

        public void updateTweet(int tweetId, Tweet tweet)
        {
            Tweet tweetToUpdate = GetTweetById(tweetId);
            if (tweetToUpdate != tweet)
            {
                _context.Tweets.Attach(tweet);
                var entry = _context.Tweets.Entry(tweet);
                entry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }

        }

        public NewsFeedWithTweetsDto GetNewsFeedWithTweets(int newsFeedId)
        {
            var result = _context.NewsFeeds
                .Where(c => c.Id == newsFeedId)
                .Select(n => new NewsFeedWithTweetsDto
                {
                    Id = n.Id,
                    UpdatedAt = n.UpdatedAt,
                    Username = n.Username,
                    Tweets = n.Tweets.Select(t => new TweetReadDto
                    {
                        Id = t.Id,
                        Message = t.Content
                    }).ToList()
                })
                .FirstOrDefault();

            return result;
        }

        public void DeleteNewsFeed(int newsFeedId)
        {
            _context.Remove(_context.NewsFeeds.Single(a => a.Id == newsFeedId));
        }

        public List<Tweet> GetTweetsByName(string username)
        {
            return _context.Tweets.Where
            (p => p.Username == username).ToList();
        }

        public NewsFeed GetNewsFeedByUser(string userName)
        {
            return _context.NewsFeeds.FirstOrDefault
            (p => p.Username == userName);
        }
    }
}