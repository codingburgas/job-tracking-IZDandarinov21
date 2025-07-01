using JobTracking.Domain.Enums;

namespace JobTracking.Application.DTOs.User
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public UserRole Role { get; set; }
        public bool Succeeded { get; set; }
        public object Errors { get; set; }
    }
}
