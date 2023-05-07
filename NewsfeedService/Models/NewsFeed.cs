using System.ComponentModel.DataAnnotations;

namespace NewsFeedService.Models
{
    public class NewsFeed
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string UpdatedAt { get; set; }
        [Required]
        public int TweetID { get; set; }
        public Tweet Tweet { get; set; }

    }
}