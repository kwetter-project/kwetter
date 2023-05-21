using TweetService.Models;

namespace TweetService.Data
{
    public class TweetRepo : ITweetRepo
    {
        private readonly AppDbContext _context;

        public TweetRepo(AppDbContext context)
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

        public void DeleteTweet(int id)
        {
            _context.Remove(_context.Tweets.Single(a => a.Id == id));
        }

        public IEnumerable<Tweet> GetAllTweets()
        {
            return _context.Tweets.ToList();
        }

        public Tweet GetTweetById(int id)
        {
            return _context.Tweets.FirstOrDefault
            (p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public List<Tweet> GetTweetsByName(string username)
        {
            return _context.Tweets.Where
            (p => p.Username == username).ToList();
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
    }
}