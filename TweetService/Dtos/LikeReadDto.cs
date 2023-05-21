using System.ComponentModel.DataAnnotations;

namespace TweetService.Dtos
{
    public class LikeReadDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public int TweetId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}