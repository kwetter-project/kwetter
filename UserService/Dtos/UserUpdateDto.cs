using System.ComponentModel.DataAnnotations;

namespace UserService.Dtos
{
    public class UserUpdateDto
    {
        [Key]
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string OldPassword { get; set; } = string.Empty;
        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }
}