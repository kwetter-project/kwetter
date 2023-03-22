namespace UserTimelineService.Dtos
{
    public class UserTimelineReadDto
    {

        public int Id { get; set; }

        public string TweetId { get; set; }

        public string UserId { get; set; }

        public string AuthorId { get; set; }
    }
}