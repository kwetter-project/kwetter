using System.ComponentModel.DataAnnotations;

namespace TweetService.Dtos
{
    public class TweetCreateDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int Like { get; set; } = 0;

        public int Retweet { get; set; } = 0;

        public int Reply { get; set; } = 0;
    }
}