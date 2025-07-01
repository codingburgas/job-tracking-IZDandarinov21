using AutoMapper;
using JobTracking.Application.DTOs.Application;
using JobTracking.Application.DTOs.JobAdvertisement;
using JobTracking.Application.DTOs.User;
using JobTracking.Domain.Entities;
using System; 

namespace JobTracking.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
           
            CreateMap<UserRegisterDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Паролата ще се хешира отделно
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Domain.Enums.UserRole.User)); 
            CreateMap<User, UserResponseDto>();
            CreateMap<UserLoginDto, User>();

            // JobAdvertisement Mappings
            CreateMap<JobAdvertisementCreateDto, JobAdvertisement>()
                .ForMember(dest => dest.DatePosted, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true)); // Нови обяви са активни по подразбиране
            CreateMap<JobAdvertisementUpdateDto, JobAdvertisement>();
            CreateMap<JobAdvertisement, JobAdvertisementResponseDto>();

            // Application Mappings
            CreateMap<JobTracking.Domain.Entities.Application, ApplicationResponseDto>()
                .ForMember(dest => dest.DateApplied, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Domain.Enums.ApplicationStatus.Submitted)); // Нови кандидатури са "Submitted" по подразбиране
            CreateMap<JobTracking.Domain.Entities.Application, ApplicationCreateDto>(); // Ще актуализира само статуса
            CreateMap<JobTracking.Domain.Entities.Application, ApplicationResponseDto>();
        }
    }
}