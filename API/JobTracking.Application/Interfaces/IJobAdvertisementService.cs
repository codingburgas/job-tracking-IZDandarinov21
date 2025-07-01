using JobTracking.Application.DTOs.JobAdvertisement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobTracking.Application.Interfaces
{
    public interface IJobAdvertisementService
    {
        Task<IEnumerable<JobAdvertisementResponseDto>> GetAllJobAdvertisementsAsync(bool includeInactive);
        Task<JobAdvertisementResponseDto> GetJobAdvertisementByIdAsync(int id);
       
        Task<JobAdvertisementResponseDto> CreateJobAdvertisementAsync(JobAdvertisementCreateDto createDto);
       
        Task<JobAdvertisementResponseDto> UpdateJobAdvertisementAsync(int id, JobAdvertisementUpdateDto updateDto);
       
        Task<bool> DeleteJobAdvertisementAsync(int id);
        
    }
}
