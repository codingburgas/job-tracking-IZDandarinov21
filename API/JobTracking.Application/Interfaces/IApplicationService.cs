using JobTracking.Application.DTOs.Application;
using JobTracking.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobTracking.Application.Interfaces
{
    public interface IApplicationService
    {
        Task<ApplicationResponseDto> ApplyForJobAsync(int userId, ApplicationCreateDto createDto);
        Task<ApplicationResponseDto> UpdateApplicationStatusAsync(int applicationId, ApplicationUpdateStatusDto updateDto);
        Task<IEnumerable<ApplicationResponseDto>> GetUserApplicationsAsync(int userId);
        Task<IEnumerable<ApplicationResponseDto>> GetApplicationsForJobAdvertisementAsync(int jobAdvertisementId);
        Task<ApplicationResponseDto> GetApplicationByIdAsync(int applicationId);
    }
}