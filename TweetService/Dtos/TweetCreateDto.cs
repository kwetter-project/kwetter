using System.ComponentModel.DataAnnotations;

namespace TweetService.Models
{
    public class TweetCreateDto
    {
        [Required]
        public string Message { get; set; }

        [Required]
        public string User { get; set; }

        [Required]
        public string DateTime { get; set; }
    }
}