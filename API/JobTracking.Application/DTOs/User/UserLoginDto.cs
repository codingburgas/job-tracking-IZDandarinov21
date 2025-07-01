using System.ComponentModel.DataAnnotations;

namespace JobTracking.Application.DTOs.User
{
    public class UserLoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
