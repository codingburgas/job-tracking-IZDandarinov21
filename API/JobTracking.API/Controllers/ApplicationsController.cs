using Microsoft.AspNetCore.Mvc;
using JobTracking.Application.DTOs.Application;
using JobTracking.Application.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using JobTracking.Domain.Enums;
using System.Collections.Generic;
using System; 

namespace JobTracking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "UserPolicy")] // Политика по подразбиране: автентикирани потребители (User или Admin)
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationsController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("Невалиден потребителски ID в токена.");
        }

        private bool IsCurrentUserAdmin()
        {
            return User.IsInRole(UserRole.Admin.ToString());
        }

        [HttpPost]
        [Authorize(Roles = "User")] // Само обикновени потребители могат да кандидатстват
        public async Task<ActionResult<ApplicationResponseDto>> Apply([FromBody] ApplicationCreateDto createDto)
        {
            var userId = GetUserIdFromToken();
            try
            {
                var application = await _applicationService.ApplyForJobAsync(userId, createDto);
                return CreatedAtAction(nameof(GetApplicationById), new { id = application.Id }, application);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message }); 
            }
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")] // Само администратори могат да актуализират статус
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] ApplicationUpdateStatusDto updateDto)
        {
            try
            {
                var updatedApplication = await _applicationService.UpdateApplicationStatusAsync(id, updateDto);
                return Ok(updatedApplication);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("my")]
        [Authorize(Roles = "User")] // Само потребителите могат да виждат своите кандидатури
        public async Task<ActionResult<IEnumerable<ApplicationResponseDto>>> GetMyApplications()
        {
            var userId = GetUserIdFromToken();
            var applications = await _applicationService.GetUserApplicationsAsync(userId);
            return Ok(applications);
        }

        [HttpGet("job/{jobAdvertisementId}")]
        [Authorize(Roles = "Admin")] // Само администратори могат да виждат кандидатури за конкретна обява
        public async Task<ActionResult<IEnumerable<ApplicationResponseDto>>> GetApplicationsForJob(int jobAdvertisementId)
        {
            var applications = await _applicationService.GetApplicationsForJobAdvertisementAsync(jobAdvertisementId);
            return Ok(applications);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationResponseDto>> GetApplicationById(int id)
        {
            var application = await _applicationService.GetApplicationByIdAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            // Уверете се, че потребителят може да вижда само собствените си кандидатури, освен ако не е администратор
            var userId = GetUserIdFromToken();
            if (application.UserId != userId && !IsCurrentUserAdmin())
            {
                return Forbid("Нямате достъп до тази кандидатура.");
            }

            return Ok(application);
        }
    }
}