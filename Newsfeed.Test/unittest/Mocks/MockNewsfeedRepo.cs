using System.Security.Cryptography;
using Moq;
using NewsFeedService.Data;

internal class MockNewsfeedRepo
{
    public static Mock<INewsFeedRepo> GetMock()
    {
        var mock = new Mock<INewsFeedRepo>();
        mock.Setup(m => m.GetNewsFeedByUser(It.IsAny<string>())).Callback(() => { return; });
        mock.Setup(m => m.SaveChanges()).Callback(() => { return; });

        return mock;
    }
}