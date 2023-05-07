using System.ComponentModel.DataAnnotations;
namespace NewsFeedService.Models
{
    public class Tweet
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public int ExternalID { get; set; }
        [Required]
        public string Message { get; set; }
        public ICollection<NewsFeed> NewsFeeds { get; set; } = new List<NewsFeed>();
    }
}