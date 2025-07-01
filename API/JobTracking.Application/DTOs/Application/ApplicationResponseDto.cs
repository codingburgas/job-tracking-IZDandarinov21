using System;
using JobTracking.Domain.Enums;
using JobTracking.Application.DTOs.JobAdvertisement;

namespace JobTracking.Application.DTOs.Application
{
    public class ApplicationResponseDto
    {
        public int Id { get; set; }
        public int JobAdvertisementId { get; set; }
        public JobAdvertisementResponseDto JobAdvertisement { get; set; }
        public int UserId { get; set; }
        public ApplicationStatus Status { get; set; }
        public DateTime DateApplied { get; set; }
    }
}
