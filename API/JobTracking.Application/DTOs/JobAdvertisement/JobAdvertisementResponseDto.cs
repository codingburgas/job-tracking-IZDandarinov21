using System;

namespace JobTracking.Application.DTOs.JobAdvertisement
{
    public class JobAdvertisementResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public DateTime DatePosted { get; set; }
        public bool IsActive { get; set; }
    }
}
