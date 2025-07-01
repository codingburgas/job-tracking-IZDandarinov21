using System.ComponentModel.DataAnnotations;

namespace JobTracking.Application.DTOs.Application
{
    public class ApplicationCreateDto
    {
        [Required]
        public int JobAdvertisementId { get; set; }
    }
}
