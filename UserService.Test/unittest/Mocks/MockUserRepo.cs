using System.Security.Cryptography;
using Moq;
using UserService.Data;
using UserService.Models;

internal class MockUserRepo
{
    public static Mock<IUserRepo> GetMock()
    {
        var mock = new Mock<IUserRepo>();

        // Setup the mock
        CreatePasswordHash("test_password", out byte[] passwordHash1, out byte[] passwordSalt1);
        CreatePasswordHash("test_password2", out byte[] passwordHash2, out byte[] passwordSalt2);
        CreatePasswordHash("test_password3", out byte[] passwordHash3, out byte[] passwordSalt3);
        var users = new List<User>
            {
                new User { Username = "User1", PasswordHash=passwordHash1,PasswordSalt=passwordSalt1,Role="Admin" },
                new User { Username = "User2", PasswordHash=passwordHash2,PasswordSalt=passwordSalt2,Role="Member"},
                new User { Username = "User3", PasswordHash=passwordHash3,PasswordSalt=passwordSalt3,Role="Business"}
            };

        mock.Setup(m => m.GetAllUsers()).Returns(() => users);
        mock.Setup(m => m.GetUserById(It.IsAny<string>())).Returns((string id) => users.FirstOrDefault(o => o.Username == id));
        mock.Setup(m => m.CreateUser(It.IsAny<User>())).Callback(() => { return; });
        mock.Setup(m => m.DeleteUser(It.IsAny<string>())).Callback(() => { return; });
        mock.Setup(m => m.updatePassword(It.IsAny<User>())).Callback(() => { return; });
        mock.Setup(m => m.updateUserToken(It.IsAny<User>())).Callback(() => { return; });
        mock.Setup(m => m.userExist(It.IsAny<string>())).Returns((string id) => users.Any(p => p.Username == id));
        mock.Setup(m => m.SaveChanges()).Callback(() => { return; });

        return mock;
    }

    private static void CreatePasswordHash(string v, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(v));
        }
    }
}