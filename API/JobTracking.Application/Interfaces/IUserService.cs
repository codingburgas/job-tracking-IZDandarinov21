using JobTracking.Application.DTOs.User;
using JobTracking.Domain.Enums;
using JobTracking.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic; 

namespace JobTracking.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> RegisterUserAsync(UserRegisterDto userRegisterDto);
        Task<UserResponseDto> LoginUserAsync(UserLoginDto userLoginDto);
        Task<UserResponseDto> GetUserByIdAsync(int userId);
        Task<bool> IsUserAdmin(int userId);
    }
}