using System.ComponentModel.DataAnnotations;
using JobTracking.Domain.Enums;

namespace JobTracking.Application.DTOs.Application
{
    public class ApplicationUpdateStatusDto
    {
        [Required]
        public ApplicationStatus Status { get; set; }
    }
}
