using System.ComponentModel.DataAnnotations;

namespace JobTracking.Application.DTOs.JobAdvertisement
{
    public class JobAdvertisementUpdateDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 10)]
        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
