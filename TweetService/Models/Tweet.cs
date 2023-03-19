using System.ComponentModel.DataAnnotations;
namespace TweetService.Models
{
    public class Tweet
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string User { get; set; }
        [Required]
        public string DateTime { get; set; }
        public int Like { get; set; }

    }
}