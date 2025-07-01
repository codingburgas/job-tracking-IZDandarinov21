using Microsoft.AspNetCore.Mvc;
using JobTracking.Application.Interfaces;
using JobTracking.Application.DTOs.JobAdvertisement;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Security.Claims; 

namespace JobTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // Тази политика гарантира, че всички методи в контролера изискват автентикация
    // и потребителят да има роля "User" или "Admin".
    [Authorize(Policy = "UserPolicy")]
    public class JobAdvertisementsController : ControllerBase
    {
        private readonly IJobAdvertisementService _jobAdvertisementService;

        public JobAdvertisementsController(IJobAdvertisementService jobAdvertisementService)
        {
            _jobAdvertisementService = jobAdvertisementService;
        }

        [HttpGet]
        [AllowAnonymous] // Публично достъпни за преглед на активни обяви
        public async Task<ActionResult<IEnumerable<JobAdvertisementResponseDto>>> GetAll([FromQuery] bool includeInactive = false)
        {
            // Тази проверка за Admin роля все още е валидна, ако искаш само Admin да вижда неактивни
            if (includeInactive && User.Identity.IsAuthenticated && !User.IsInRole("Admin"))
            {
                return Forbid("Потребителите нямат право да виждат неактивни обяви.");
            }

            var jobAds = await _jobAdvertisementService.GetAllJobAdvertisementsAsync(includeInactive && User.IsInRole("Admin"));
            return Ok(jobAds);
        }

        [HttpGet("{id}")]
        [AllowAnonymous] // Публично достъпни за преглед на конкретна обява
        public async Task<ActionResult<JobAdvertisementResponseDto>> GetById(int id)
        {
            try
            {
                var jobAd = await _jobAdvertisementService.GetJobAdvertisementByIdAsync(id);
                return Ok(jobAd);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize(Policy = "UserPolicy")] // Само логнати потребители (User или Admin) могат да създават обяви
        public async Task<ActionResult<JobAdvertisementResponseDto>> Create([FromBody] JobAdvertisementCreateDto createDto)
        {
            try
            {
                
                var jobAd = await _jobAdvertisementService.CreateJobAdvertisementAsync(createDto);
                return CreatedAtAction(nameof(GetById), new { id = jobAd.Id }, jobAd);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UserPolicy")] // Само логнати потребители (User или Admin) могат да редактират обяви
        public async Task<IActionResult> Update(int id, [FromBody] JobAdvertisementUpdateDto updateDto)
        {
            try
            {
                
                var updatedAd = await _jobAdvertisementService.UpdateJobAdvertisementAsync(id, updateDto);
                return Ok(updatedAd);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "UserPolicy")] // Само логнати потребители (User или Admin) могат да изтриват обяви
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
               
                var result = await _jobAdvertisementService.DeleteJobAdvertisementAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Възникна грешка при изтриване на обявата: " + ex.Message });
            }
        }
    }
}
