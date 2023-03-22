using System.ComponentModel.DataAnnotations;

namespace UserTimelineService.Models
{
    public class UserTimelineCreateDto
    {
        [Required]
        public string TweetId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string AuthorId { get; set; }
    }
}