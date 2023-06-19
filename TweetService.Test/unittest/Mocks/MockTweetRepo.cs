using System.Security.Cryptography;
using Moq;
using TweetService.Data;
using TweetService.Models;

internal class MockTweetRepo
{
    public static Mock<ITweetRepo> GetMock()
    {
        var mock = new Mock<ITweetRepo>();

        // Setup the mock
        var Tweets = new List<Tweet>
            {
                new Tweet { Username = "Bob1023", Type = "Tweet", Content = "Today is Monday.", CreatedAt = DateTime.Now.AddHours(-12) },
                new Tweet { Username = "ToM_B", Type = "Retweet", Content = "1", CreatedAt = DateTime.Now.AddHours(-2) },
                new Tweet { Username = "Business_A", Type = "Ads", Content = "New Product" },
                new Tweet { Username = "Mary000", Type = "Tweet", Content = "Today is Thursday.", CreatedAt = DateTime.Now.AddHours(-22) }
            };

        mock.Setup(m => m.DeleteTweet(It.IsAny<int>())).Callback(() => { return; });
        mock.Setup(m => m.SaveChanges()).Callback(() => { return; });

        return mock;
    }
}