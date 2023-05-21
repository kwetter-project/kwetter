namespace TweetService.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}