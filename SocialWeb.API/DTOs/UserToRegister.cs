using System.ComponentModel.DataAnnotations;

namespace SocialWeb.API.DTOs
{
    public class UserToRegisterDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 16 charcters long.")]
        public string Password { get; set; }
    }
}