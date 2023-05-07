using System.ComponentModel.DataAnnotations;

namespace NewsFeedService.Dtos
{
    public class NewsFeedCreateDto
    {
        [Required]
        public string UpdatedAt { get; set; }
    }
}