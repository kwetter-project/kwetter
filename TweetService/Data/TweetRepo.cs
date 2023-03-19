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
    }
}