using System.ComponentModel.DataAnnotations;
namespace TweetService.Models
{
    public class Tweet
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public int Like { get; set; }

        public int Retweet { get; set; }

        public int Reply { get; set; }

    }
}