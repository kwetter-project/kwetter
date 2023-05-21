using NewsFeedService.Models;

namespace NewsFeedService.Dtos
{
    public class NewsFeedWithTweetsDto
    {
        public int Id { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Username { get; set; }
        public ICollection<TweetReadDto> Tweets { get; set; }
    }
}
