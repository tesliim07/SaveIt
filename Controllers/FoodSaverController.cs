using FoodSaver.Models;
using FoodSaver.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodSaver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FoodSaverController : ControllerBase
    {
        private readonly IFoodSaverService _foodSaverService;
        private readonly ILogger _logger;

        public FoodSaverController(IFoodSaverService foodSaverService, ILogger<FoodSaverController> logger)
        {
            _foodSaverService = foodSaverService;
            _logger = logger;
        }

        [HttpPost("CreateFoodItem")]
        public async Task<ActionResult<Guid>> CreateFoodItem([FromBody] FoodItemsCreateDto foodItemsDto)
        {
            var providerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (foodItemsDto == null)
            {
                _logger.LogError("[FoodSaverController] Food Item Creation Unsuccessful");
                return StatusCode(400);
            }
            var foodItemId = await _foodSaverService.CreateFoodItem(foodItemsDto, providerId);
            return Ok(foodItemId);
        }

        [HttpGet("GetAllFoodItems")]
        public async Task<ActionResult<List<FoodItemsReadAndUpdateDto>>> GetAllFoodItems()
        {
            var foodItems = await _foodSaverService.GetAllFoodItems();
            if (foodItems.Count == 0)
            {
                _logger.LogInformation("No Food Items Found");
            }
            return Ok(foodItems);
        }

        [HttpGet("GetFoodItemsByUserId")]
        public async Task<ActionResult<List<FoodItemsReadAndUpdateDto>>> GetByUserId()
        {
            var providerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (providerId == null)
            {
                _logger.LogError("[FoodSaverController], Unable to get provider Id from generated token");
                return StatusCode(400);
            }
            var foodItems = await _foodSaverService.GetByUserId(providerId);
            if (foodItems.Count == 0)
            {
                _logger.LogInformation("User hasn't added food Items");
            }
            return Ok(foodItems);
        }

        [HttpPut("UpdateFoodItem")]
        public async Task<ActionResult<Boolean>> UpdateFoodItem([FromBody] FoodItemsReadAndUpdateDto foodItemsUpdateDto)
        {
            if (foodItemsUpdateDto == null)
            {
                _logger.LogError("[FoodSaverController] Food Item Update Unsuccessful");
                return StatusCode(400);
            }
            var isUpdateSuccessful = await _foodSaverService.UpdateFoodItem(foodItemsUpdateDto);
            if (isUpdateSuccessful == false)
            {
                return StatusCode(404);
            }
            return Ok(isUpdateSuccessful);
        }

        [HttpDelete("DeleteFoodItem")]
        public async Task<ActionResult<Boolean>> DeleteFoodItem([FromBody] FoodItemsDeleteDto foodItemsDeleteDto)
        {
            if (foodItemsDeleteDto == null)
            {
                _logger.LogError("[FoodSaverController] Food Item Delete Unsuccessful");
                return StatusCode(400);
            }
            var isDeleteSuccessful = await _foodSaverService.DeleteFoodItem(foodItemsDeleteDto);
            if (isDeleteSuccessful == false)
            {
                return StatusCode(404);
            }
            return Ok(isDeleteSuccessful);
        }
    }
}
