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
                case EventType.TweetDeleted:
                    deleteTweet(message);
                    break;
                case EventType.UserDeleted:
                    deleteUser(message);
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
                case "Tweet_Deleted":
                    Console.WriteLine("--> Tweet Deleted EventDetected");
                    return EventType.TweetDeleted;
                case "User_Deleted":
                    Console.WriteLine("--> User Deleted EventDetected");
                    return EventType.UserDeleted;
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
                    repo.CreateTweet(tweet);
                    repo.SaveChanges();
                    Console.WriteLine("--> Tweet added!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add tweet to db {ex.Message}");
                }
            }
        }
        private void deleteTweet(string tweetPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<INewsFeedRepo>();
                var tweetPublishedDto = JsonSerializer.Deserialize<TweetPublishedDto>(tweetPublishedMessage);

                try
                {
                    repo.DeleteTweet(tweetPublishedDto.Id);
                    repo.SaveChanges();
                    Console.WriteLine("--> Tweet deleted!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not delete tweet in db {ex.Message}");
                }
            }
        }
        private void deleteUser(string userDeletePublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<INewsFeedRepo>();
                var userDeletePublishedDto = JsonSerializer.Deserialize<UserDeletePublishedDto>(userDeletePublishedMessage);

                try
                {
                    var nf = repo.GetNewsFeedByUser(userDeletePublishedDto.Id);
                    repo.DeleteNewsFeed(nf.Id);
                    repo.SaveChanges();
                    Console.WriteLine("--> newsfeed associated with user deleted!");

                    var tweetToDelete = repo.GetTweetsByName(userDeletePublishedDto.Id);
                    foreach (var twt in tweetToDelete)
                    {
                        repo.DeleteTweet(twt.Id);
                        repo.SaveChanges();
                    }
                    Console.WriteLine("--> tweets associated with user deleted!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not delete newsfeed associated with user in db {ex.Message}");
                }
            }
        }
    }
    enum EventType
    {
        TweetPublished,
        TweetDeleted,
        LikePublished,
        LikeDeleted,

        UserDeleted,
        Undetermined
    }
}
