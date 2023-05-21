using System.ComponentModel.DataAnnotations;

namespace NewsFeedService.Models
{
    public class Follower
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string FollowerName { get; set; }
        [Required]
        public string FolloweeName { get; set; }

    }
}