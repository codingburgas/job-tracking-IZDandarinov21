using JobTracking.Domain.Enums;

namespace JobTracking.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; } //хеширана парола
        public UserRole Role { get; set; }

        // Navigation property
        public ICollection<Application> Applications { get; set; }
    }
}