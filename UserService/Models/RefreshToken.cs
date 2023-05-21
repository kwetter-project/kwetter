using System.ComponentModel.DataAnnotations;

namespace UserService.Models
{
    public class RefreshToken
    {
        [Key]
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required]
        public DateTime Created { get; set; } = DateTime.Now;
        [Required]
        public DateTime Expires { get; set; }
    }
}