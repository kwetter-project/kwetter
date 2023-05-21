using Moq;
using UserService.AsyncDataServices;
using UserService.Data;
using UserService.Dtos;
using UserService.Models;

internal class MockMessageBusClient
{
    public static Mock<IMessageBusClient> GetMock()
    {
        var mock = new Mock<IMessageBusClient>();

        mock.Setup(m => m.PublishUserDeleted(It.IsAny<UserDeletePublishedDto>())).Callback(() => { return; });

        return mock;
    }
}