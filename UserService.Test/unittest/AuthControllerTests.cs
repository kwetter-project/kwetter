using UserService.Dtos;
using Moq;
using Microsoft.Extensions.Configuration;
using UserService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

public class AuthControllerTests
{
    [Fact]
    public async Task Register_ValidUser_ReturnsOkResult()
    {
        // Arrange
        var userDto = new UserDto { Username = "test_user", Password = "test_password" };
        var mockUserRepo = MockUserRepo.GetMock();
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(c => c.GetSection("AppSettings:Token").Value).Returns("my top secret key");
        var mockMessageBusClient = MockMessageBusClient.GetMock();
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        var mockHttpContext = new DefaultHttpContext();

        mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext);

        var authController = new AuthController(mockConfiguration.Object, mockUserRepo.Object, mockMessageBusClient.Object, mockHttpContextAccessor.Object);

        // Act
        var result = await authController.Register(userDto);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task Register_ExistingUser_ReturnsBadRequestResult()
    {
        // Arrange
        var userDto = new UserDto { Username = "User1", Password = "test_password" };
        var mockUserRepo = MockUserRepo.GetMock();
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(c => c.GetSection("AppSettings:Token").Value).Returns("my top secret key");
        var mockMessageBusClient = MockMessageBusClient.GetMock();
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        var mockHttpContext = new DefaultHttpContext();

        mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext);

        var authController = new AuthController(mockConfiguration.Object, mockUserRepo.Object, mockMessageBusClient.Object, mockHttpContextAccessor.Object);

        // Act
        var result = await authController.Register(userDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.Equal("Username already exist", badRequestResult.Value);
    }

    [Fact]
    public async Task Login_InvalidUser_ReturnsBadRequestResult()
    {
        // Arrange
        var userDto = new UserDto { Username = "User4", Password = "test_password" };
        var mockUserRepo = MockUserRepo.GetMock();
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(c => c.GetSection("AppSettings:Token").Value).Returns("my top secret key");
        var mockMessageBusClient = MockMessageBusClient.GetMock();
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        var mockHttpContext = new DefaultHttpContext();

        mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext);

        var authController = new AuthController(mockConfiguration.Object, mockUserRepo.Object, mockMessageBusClient.Object, mockHttpContextAccessor.Object);

        // Act
        var result = await authController.Login(userDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.Equal("User not found", badRequestResult.Value);
    }

    [Fact]
    public async Task Login_InvalidPassword_ReturnsBadRequestResult()
    {
        // Arrange
        var userDto = new UserDto { Username = "User1", Password = "wrong_test_password" };
        var mockUserRepo = MockUserRepo.GetMock();
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(c => c.GetSection("AppSettings:Token").Value).Returns("my top secret key");
        var mockMessageBusClient = MockMessageBusClient.GetMock();
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        var mockHttpContext = new DefaultHttpContext();
        // mockHttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        // {
        //         new Claim(ClaimTypes.Name, "User2"),
        //         new Claim(ClaimTypes.Role, "Member"),
        //     // Add any additional claims or properties as needed
        // }, "oauth2")); // if logged in

        mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext);

        var authController = new AuthController(mockConfiguration.Object, mockUserRepo.Object, mockMessageBusClient.Object, mockHttpContextAccessor.Object);

        // Act
        var result = await authController.Login(userDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.Equal("Wrong password", badRequestResult.Value);
    }

    [Fact]
    public async Task Login_ValidUser_ReturnsOkResult()
    {
        // Arrange
        var userDto = new UserDto { Username = "User1", Password = "test_password" };
        var mockUserRepo = MockUserRepo.GetMock();
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(c => c.GetSection("AppSettings:Token").Value).Returns("my top secret key");
        var mockMessageBusClient = MockMessageBusClient.GetMock();
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        var mockHttpContext = new DefaultHttpContext();

        mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext);

        var authController = new AuthController(mockConfiguration.Object, mockUserRepo.Object, mockMessageBusClient.Object, mockHttpContextAccessor.Object);

        // Create a mock of IResponseCookies
        var cookiesMock = new Mock<IResponseCookies>();

        // Set up the behavior for IResponseCookies.Append
        cookiesMock.Setup(c => c.Append("refreshToken", It.IsAny<string>(), It.IsAny<CookieOptions>()));

        // Create a mock of HttpContext
        var httpContextMock = new Mock<HttpContext>();

        // Assign the cookies mock to the Response.Cookies property
        httpContextMock.SetupGet(c => c.Response.Cookies).Returns(cookiesMock.Object);

        // Create a custom ControllerContext implementation
        var customControllerContext = new ControllerContext()
        {
            HttpContext = httpContextMock.Object
        };

        // Pass the custom ControllerContext to your controller
        authController.ControllerContext = customControllerContext;


        // Act
        var result = await authController.Login(userDto);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);

    }

}