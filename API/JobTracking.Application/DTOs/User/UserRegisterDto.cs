using System.ComponentModel.DataAnnotations;
namespace JobTracking.Application.DTOs.User
{
    public class UserRegisterDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 5)]
        public string Username { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }
    }
}