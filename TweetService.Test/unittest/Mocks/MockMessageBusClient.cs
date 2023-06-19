using Moq;
using TweetService.AsyncDataServices;
using TweetService.Data;
using TweetService.Dtos;
using TweetService.Models;

internal class MockMessageBusClient
{
    public static Mock<IMessageBusClient> GetMock()
    {
        var mock = new Mock<IMessageBusClient>();

        mock.Setup(m => m.PublishDeleteTweet(It.IsAny<TweetDeletedDto>())).Callback(() => { return; });

        return mock;
    }
}