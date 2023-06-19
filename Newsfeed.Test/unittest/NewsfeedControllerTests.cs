using NewsFeedService.Dtos;
using Moq;
using Microsoft.Extensions.Configuration;
using NewsFeedService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

public class NewsfeedControllerTests
{
    [Fact]
    public async Task GetNewsfeedByUser_NotFoundNewsfeed_ReturnsNotFoundResult()
    {
        // Arrange
        var mockNewsfeedRepo = MockNewsfeedRepo.GetMock();

        var newsfeedController = new NewsFeedController(mockNewsfeedRepo.Object);

        // Act
        var result = newsfeedController.GetNewsfeedByUser("user1");

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

}