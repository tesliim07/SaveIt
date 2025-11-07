using FoodSaver.Models;
using FoodSaver.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodSaver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public ActionResult<Guid> CreateFoodItem([FromBody] FoodItemsCreateDto foodItemsDto)
        {
            if (foodItemsDto == null)
            {
                _logger.LogError("[FoodSaverController] Food Item Creation Unsuccessful");
                return StatusCode(400);
            }
            var foodItemId = _foodSaverService.CreateFoodItem(foodItemsDto);
            return Ok(foodItemId);
        }

        [HttpGet("GetAllFoodItems")]
        public ActionResult<List<FoodItemsReadAndUpdateDto>> GetAllFoodItems()
        {
            var foodItems = _foodSaverService.GetAllFoodItems();
            if (foodItems.Count == 0)
            {
                _logger.LogInformation("No Food Items Found");
            }
            return Ok(foodItems);
        }

        [HttpPut("UpdateFoodItem")]
        public ActionResult<Boolean> UpdateFoodItem([FromBody] FoodItemsReadAndUpdateDto foodItemsUpdateDto)
        {
            if (foodItemsUpdateDto == null)
            {
                _logger.LogError("[FoodSaverController] Food Item Update Unsuccessful");
                return StatusCode(400);
            }
            var isUpdateSuccessful = _foodSaverService.UpdateFoodItem(foodItemsUpdateDto);
            if (isUpdateSuccessful == false)
            {
                return StatusCode(404);
            }
            return Ok(isUpdateSuccessful);
        }

        [HttpDelete("DeleteFoodItem")]
        public ActionResult<Boolean> DeleteFoodItem([FromBody] FoodItemsDeleteDto foodItemsDeleteDto)
        {
            if (foodItemsDeleteDto == null)
            {
                _logger.LogError("[FoodSaverController] Food Item Delete Unsuccessful");
                return StatusCode(400);
            }
            var isDeleteSuccessful = _foodSaverService.DeleteFoodItem(foodItemsDeleteDto);
            if (isDeleteSuccessful == false)
            {
                return StatusCode(404);
            }
            return Ok(isDeleteSuccessful);
        }
    }
}
