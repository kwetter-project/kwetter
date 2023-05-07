using System.Text.Json;
using AutoMapper;
using NewsFeedService.Data;
using NewsFeedService.Dtos;
using NewsFeedService.Models;

namespace NewsFeedService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.TweetPublished:
                    addTweet(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(String notificationMessage)
        {
            Console.WriteLine("--> Determining Event");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            switch (eventType.Event)
            {
                case "Tweet_Published":
                    Console.WriteLine("--> Tweet Published EventDetected");
                    return EventType.TweetPublished;
                default:
                    Console.WriteLine("--> Could not determine event type");
                    return EventType.Undetermined;
            }
        }
        private void addTweet(string tweetPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<INewsFeedRepo>();
                var tweetPublishedDto = JsonSerializer.Deserialize<TweetPublishedDto>(tweetPublishedMessage);

                try
                {
                    var tweet = _mapper.Map<Tweet>(tweetPublishedDto);
                    if (!repo.ExternalTweetExist(tweet.ExternalID))
                    {
                        repo.CreateTweet(tweet);
                        repo.SaveChanges();
                        Console.WriteLine("--> Tweet added!");
                    }
                    else
                    {
                        Console.WriteLine("--> Tweet already existed...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add tweet to db {ex.Message}");
                }
            }
        }
    }
    enum EventType
    {
        TweetPublished,
        Undetermined
    }
}
