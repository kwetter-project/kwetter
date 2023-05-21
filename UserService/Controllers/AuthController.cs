using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using UserService.Dtos;
using UserService.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using UserService.Data;
using Microsoft.AspNetCore.Authentication;
using UserService.AsyncDataServices;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly IUserRepo _repo;
        private readonly IMessageBusClient _messageBusClient;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(IConfiguration configuration, IUserRepo userRepo, IMessageBusClient messageBusClient, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _repo = userRepo;
            _messageBusClient = messageBusClient;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("update")]
        public async Task<ActionResult<string>> UpdatePassword(UserUpdateDto request)
        {
            var user = _repo.GetUserById(request.Username);
            if (!VerifyPasswordHash(request.OldPassword, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong old password");
            }
            if (request.OldPassword == request.NewPassword)
            {
                return BadRequest("Please enter a different new password");
            }
            CreatePasswordHash(request.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _repo.updatePassword(user);
            _repo.SaveChanges();
            return Ok("password updated");

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteUser(string id)
        {
            var user = _repo.GetUserById(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            _repo.DeleteUser(id);
            _repo.SaveChanges();


            try
            {
                var tweetPublishedDto = new UserDeletePublishedDto();
                tweetPublishedDto.Id = id;
                tweetPublishedDto.Event = "User_Deleted";
                _messageBusClient.PublishUserDeleted(tweetPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }

            return Ok("User deleted");
        }

        [HttpPost("logout")]
        public async Task<ActionResult<string>> Logout()
        {
            // Clear the refresh token from the user's session
            string current_user = _httpContextAccessor.HttpContext.User.Identity.Name;
            var user = _repo.GetUserById(current_user);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.RefreshToken = null;
            user.TokenCreated = DateTime.MinValue;
            user.TokenExpires = DateTime.MinValue;
            _repo.updateUserToken(user);
            _repo.SaveChanges();

            // Remove the refresh token from the response cookies
            Response.Cookies.Delete("refreshToken");
            await _httpContextAccessor.HttpContext.SignOutAsync();
            return Ok("Logged out successfully");
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            if (_repo.userExist(request.Username))
            {
                return BadRequest("Username already exist");
            }
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new User();
            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _repo.CreateUser(user);
            _repo.SaveChanges();
            return Ok(user);

        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            bool userExist = _repo.userExist(request.Username);
            if (!userExist)
            {
                return BadRequest("User not found");
            }
            var user = _repo.GetUserById(request.Username);
            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password");
            }
            string token = CreateToken(user);
            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken, user);
            // Set the token in the Authorization header
            Response.Headers.Add("Authorization", $"Bearer {token}");

            return Ok();
        }
        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                return Unauthorized("Not Logged in");
            }
            var refreshToken = Request.Cookies["refreshToken"];
            string current_user = _httpContextAccessor.HttpContext.User.Identity.Name;
            var user = _repo.GetUserById(current_user);
            if (user == null)
            {
                return Unauthorized("User not found");
            }
            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token");

            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired");
            }
            string token = CreateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken, user);
            return Ok("refresh token created");
        }
        private void SetRefreshToken(RefreshToken newRefreshToken, User user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
            _repo.updateUserToken(user);
            _repo.SaveChanges();
        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}