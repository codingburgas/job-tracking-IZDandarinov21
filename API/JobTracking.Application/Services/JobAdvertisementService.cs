using JobTracking.Application.DTOs.JobAdvertisement;
using JobTracking.Application.Interfaces;
using JobTracking.Domain.Entities;
using JobTracking.DataAccess.Interfaces;
using AutoMapper;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using JobTracking.Domain.Enums; 

namespace JobTracking.Application.Services
{
    public class JobAdvertisementService : IJobAdvertisementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public JobAdvertisementService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<JobAdvertisementResponseDto>> GetAllJobAdvertisementsAsync(bool includeInactive)
        {
            var jobAds = await _unitOfWork.JobAdvertisements.GetAllAsync();

            if (!includeInactive)
            {
                jobAds = jobAds.Where(ja => ja.IsActive);
            }

            return _mapper.Map<IEnumerable<JobAdvertisementResponseDto>>(jobAds);
        }

        public async Task<JobAdvertisementResponseDto> GetJobAdvertisementByIdAsync(int id)
        {
            var jobAd = await _unitOfWork.JobAdvertisements.GetByIdAsync(id);
            if (jobAd == null)
            {
                throw new KeyNotFoundException("Обявата за работа не е намерена.");
            }
            return _mapper.Map<JobAdvertisementResponseDto>(jobAd);
        }

       
        public async Task<JobAdvertisementResponseDto> CreateJobAdvertisementAsync(JobAdvertisementCreateDto createDto)
        {
            var jobAd = _mapper.Map<JobAdvertisement>(createDto);
            jobAd.DatePosted = DateTime.UtcNow;
            jobAd.IsActive = true;
            

            await _unitOfWork.JobAdvertisements.AddAsync(jobAd);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<JobAdvertisementResponseDto>(jobAd);
        }

        
        public async Task<JobAdvertisementResponseDto> UpdateJobAdvertisementAsync(int id, JobAdvertisementUpdateDto updateDto)
        {
            var existingJobAd = await _unitOfWork.JobAdvertisements.GetByIdAsync(id);
            if (existingJobAd == null)
            {
                throw new KeyNotFoundException("Обявата за работа не е намерена.");
            }

            

            _mapper.Map(updateDto, existingJobAd);
            await _unitOfWork.JobAdvertisements.UpdateAsync(existingJobAd);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<JobAdvertisementResponseDto>(existingJobAd);
        }

        
        public async Task<bool> DeleteJobAdvertisementAsync(int id)
        {
            var jobAdToDelete = await _unitOfWork.JobAdvertisements.GetByIdAsync(id);
            if (jobAdToDelete == null)
            {
                return false; // Обявата не е намерена
            }


            await _unitOfWork.JobAdvertisements.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}