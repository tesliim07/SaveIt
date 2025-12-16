using FoodSaver.Services;
using FoodSaver.Services.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodSaver.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class JobController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUsersService _usersService;
        public JobController(ILogger<JobController> logger, IUsersService usersService)
        {
            _logger = logger;
            _usersService = usersService;
        }

        [HttpPost("RunDeleteJob/{deleteDecision}")]
        public async Task<ActionResult> RunDeleteJob([FromRoute] bool deleteDecision)
        {
            if (deleteDecision == false)
            {
                return Ok();
            }
            var providerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (providerId == null)
            {
                _logger.LogError("[JobController], Unable to get provider Id from generated token");
                return StatusCode(400);
            }
            var userId = await _usersService.GetUserIdFromProviderId(providerId);
            RecurringJob.AddOrUpdate<JobService>(
                $"recurring-{userId}",
                s => s.RunDeleteJob(userId),
                Cron.Daily);
            _logger.LogInformation($"[JobController], Recurring job created for userId: {userId}");
            return Ok();
        }
    }
}
