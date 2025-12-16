using FoodSaver.Models;
using FoodSaver.Services.Interfaces;
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
        private readonly IFoodSaverService _foodSaverService;
        public JobController(ILogger<JobController> logger, IUsersService usersService, IFoodSaverService foodSaverService)
        {
            _logger = logger;
            _usersService = usersService;
            _foodSaverService = foodSaverService;
        }

        [HttpPost("RunDeleteJob/{deleteDecision}")]
        public async Task<ActionResult> RunDeleteJob([FromRoute] bool deleteDecision)
        {
            var providerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (providerId == null)
            {
                _logger.LogError("[JobController], Unable to get provider Id from generated token");
                return StatusCode(400);
            }
            var isUpdated = await _usersService.UpdateDeleteDecision(providerId, deleteDecision);
            if (isUpdated && deleteDecision)
            {
                var user = await _usersService.GetUserByProviderId(providerId);
                var isDeleted = await _foodSaverService.DeleteExpiredFood(user.UserId);
            }
            return Ok();
        }

        [HttpGet("GetUserDeleteDecision")]
        public async Task<ActionResult<bool>> GetUserDeleteDecision()
        {
            var providerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _usersService.GetUserByProviderId(providerId);
            if (user == null)
            {
                _logger.LogError("[JobController], Unable to get user from provider Id");
                return StatusCode(400);
            }
            return Ok(user.DeleteDecision);
        }
    }
}