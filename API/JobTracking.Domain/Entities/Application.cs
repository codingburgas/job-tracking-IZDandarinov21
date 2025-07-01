using JobTracking.Domain.Enums;
using System;

namespace JobTracking.Domain.Entities
{
    public class Application
    {
        public int Id { get; set; }
        public int JobAdvertisementId { get; set; }
        public int UserId { get; set; }
        public ApplicationStatus Status { get; set; }
        public DateTime DateApplied { get; set; }

        // Navigation properties
        public JobAdvertisement JobAdvertisement { get; set; }
        public User User { get; set; }
    }
}