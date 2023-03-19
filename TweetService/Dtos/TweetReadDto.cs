namespace TweetService.Dtos
{
    public class TweetReadDto
    {

        public int Id { get; set; }

        public string Message { get; set; }
        public string User { get; set; }
        public string DateTime { get; set; }
        public int Like { get; set; }
    }
}