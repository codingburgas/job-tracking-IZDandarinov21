using AutoMapper;
using JobTracking.Application.DTOs.Application;
using JobTracking.Application.Interfaces;
using JobTracking.DataAccess.Interfaces;
using JobTracking.Domain.Entities;
using JobTracking.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobTracking.Application.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ApplicationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApplicationResponseDto> ApplyForJobAsync(int userId, ApplicationCreateDto createDto)
        {
            var jobAd = await _unitOfWork.JobAdvertisements.GetByIdAsync(createDto.JobAdvertisementId);
            if (jobAd == null || !jobAd.IsActive)
            {
                throw new ArgumentException("Обявата за работа не съществува или не е активна.");
            }

            var existingApplication = (await _unitOfWork.Applications.FindAsync(a => a.UserId == userId && a.JobAdvertisementId == createDto.JobAdvertisementId)).FirstOrDefault();
            if (existingApplication != null)
            {
                throw new InvalidOperationException("Вече сте кандидатствали за тази обява.");
            }

            var application = _mapper.Map<JobTracking.Domain.Entities.Application>(createDto);
            application.UserId = userId;
            application.Status = ApplicationStatus.Submitted;
            application.DateApplied = DateTime.UtcNow;

            await _unitOfWork.Applications.AddAsync(application);
            await _unitOfWork.CompleteAsync();
            var createdApplication = await _unitOfWork.Applications.Query()
                                           .Include(a => a.JobAdvertisement)
                                           .FirstOrDefaultAsync(a => a.Id == application.Id);

            return _mapper.Map<ApplicationResponseDto>(createdApplication);
        }

        public async Task<ApplicationResponseDto> UpdateApplicationStatusAsync(int applicationId, ApplicationUpdateStatusDto updateDto)
        {
            var application = await _unitOfWork.Applications.GetByIdAsync(applicationId);
            if (application == null)
            {
                throw new KeyNotFoundException($"Кандидатура с ID {applicationId} не е намерена.");
            }

            application.Status = updateDto.Status;
            _unitOfWork.Applications.Update(application);
            await _unitOfWork.CompleteAsync();

            var updatedApplication = await _unitOfWork.Applications.Query()
                                           .Include(a => a.JobAdvertisement)
                                           .FirstOrDefaultAsync(a => a.Id == application.Id);
            return _mapper.Map<ApplicationResponseDto>(updatedApplication);
        }

        public async Task<IEnumerable<ApplicationResponseDto>> GetUserApplicationsAsync(int userId)
        {
            var applicationsWithJobAd = await _unitOfWork.Applications.Query()
                                           .Where(a => a.UserId == userId)
                                           .Include(a => a.JobAdvertisement)
                                           .ToListAsync();
            return _mapper.Map<IEnumerable<ApplicationResponseDto>>(applicationsWithJobAd);
        }

        public async Task<IEnumerable<ApplicationResponseDto>> GetApplicationsForJobAdvertisementAsync(int jobAdvertisementId)
        {
            var applications = await _unitOfWork.Applications.Query()
                                           .Where(a => a.JobAdvertisementId == jobAdvertisementId)
                                           .Include(a => a.User)
                                           .ToListAsync();
            return _mapper.Map<IEnumerable<ApplicationResponseDto>>(applications);
        }

        public async Task<ApplicationResponseDto> GetApplicationByIdAsync(int applicationId)
        {
            var application = await _unitOfWork.Applications.Query()
                                           .Include(a => a.JobAdvertisement)
                                           .FirstOrDefaultAsync(a => a.Id == applicationId);
            if (application == null)
            {
                return null;
            }
            return _mapper.Map<ApplicationResponseDto>(application);
        }
    }
}