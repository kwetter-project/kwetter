using TweetService.Dtos;
using Moq;
using Microsoft.Extensions.Configuration;
using TweetService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

public class TweetControllerTests
{
    [Fact]
    public async Task DeleteTweet_TweetExist_ReturnsNoContentResult()
    {
        // Arrange
        var mockTweetRepo = MockTweetRepo.GetMock();
        var mockMessageBusClient = MockMessageBusClient.GetMock();

        var controller = new TweetController(mockTweetRepo.Object, mockMessageBusClient.Object);

        // Act
        var result = await controller.DeleteTweet(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

}