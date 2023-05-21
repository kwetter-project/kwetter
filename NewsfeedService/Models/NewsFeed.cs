using System.ComponentModel.DataAnnotations;

namespace NewsFeedService.Models
{
    public class NewsFeed
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
        [Required]
        public string Username { get; set; }

        public ICollection<Tweet> Tweets { get; set; } = new List<Tweet>();
    }
}