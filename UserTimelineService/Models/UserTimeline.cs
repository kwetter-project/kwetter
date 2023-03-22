using System.ComponentModel.DataAnnotations;
namespace UserTimelineService.Models
{
    public class UserTimeline
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string TweetId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string AuthorId { get; set; }

    }
}