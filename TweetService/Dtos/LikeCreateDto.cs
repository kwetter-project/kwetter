using System.ComponentModel.DataAnnotations;

namespace TweetService.Dtos
{
    public class LikeCreateDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public int TweetId { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}