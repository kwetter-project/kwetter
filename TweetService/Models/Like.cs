using System.ComponentModel.DataAnnotations;
namespace TweetService.Models
{
    public class Like
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public int TweetId { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

    }
}