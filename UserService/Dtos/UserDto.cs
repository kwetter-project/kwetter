using System.ComponentModel.DataAnnotations;

namespace UserService.Dtos
{
    public class UserDto
    {
        [Key]
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}