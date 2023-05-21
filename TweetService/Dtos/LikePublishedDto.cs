using System.ComponentModel.DataAnnotations;

namespace TweetService.Dtos
{
    public class LikePublishedDto
    {
        public int TweetId { get; set; }
        public string Event { get; set; }
    }
}