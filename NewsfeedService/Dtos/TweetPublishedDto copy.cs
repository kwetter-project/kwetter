namespace NewsFeedService.Dtos
{
    public class TweetCreateDto
    {
        public string Username { get; set; }

        public string Type { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }
        public int Like { get; set; }

        public int Retweet { get; set; }

        public int Reply { get; set; }
    }
}