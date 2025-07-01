using JobTracking.Application.DTOs.User;
using JobTracking.Application.Interfaces;
using JobTracking.Application.Mappers;
using JobTracking.DataAccess.Interfaces;
using JobTracking.Domain.Entities;
using JobTracking.Domain.Enums; 
using AutoMapper; 
using System;
using System.Linq; 
using System.Security.Cryptography; 
using System.Text; 
using System.Threading.Tasks;
using System.Collections.Generic; 

namespace JobTracking.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserResponseDto> RegisterUserAsync(UserRegisterDto userRegisterDto)
        {
            var existingUser = (await _unitOfWork.Users.FindAsync(u => u.Username == userRegisterDto.Username)).FirstOrDefault();
            if (existingUser != null)
            {
                throw new ArgumentException("Потребителското име вече съществува.");
            }

            var user = _mapper.Map<User>(userRegisterDto);
            user.PasswordHash = HashPassword(userRegisterDto.Password);
            user.Role = UserRole.User; // Присвояване на роля "User"

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto> LoginUserAsync(UserLoginDto userLoginDto)
        {
            var user = (await _unitOfWork.Users.FindAsync(u => u.Username == userLoginDto.Username)).FirstOrDefault();

            if (user == null || !VerifyPasswordHash(userLoginDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Грешно потребителско име или парола.");
            }

           
           
            var userResponseDto = _mapper.Map<UserResponseDto>(user);
            userResponseDto.Role = user.Role;

            return userResponseDto;
        }

        public async Task<UserResponseDto> GetUserByIdAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"Потребител с ID {userId} не е намерен.");
            }
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<bool> IsUserAdmin(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            return user != null && user.Role == UserRole.Admin;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            var hashedPassword = HashPassword(password);
            return hashedPassword == storedHash;
        }
    }
}