using FoodSaver.Models;
using FoodSaver.Repositories.Interfaces;
using FoodSaver.Services.Interfaces;

namespace FoodSaver.Services
{
    public class FoodSaverService : IFoodSaverService
    {
        private readonly IFoodSaverRepository _repository;
        private readonly ILogger _logger;
        public FoodSaverService(IFoodSaverRepository repository, ILogger<FoodSaverService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public Guid CreateFoodItem(FoodItemsCreateDto foodItemsCreate)
        {
            var dto = new FoodItemsCreateDto()
            {
                FoodName = foodItemsCreate.FoodName,
                FoodCategory = foodItemsCreate.FoodCategory,
                FoodExpiryDate = foodItemsCreate.FoodExpiryDate
            };
            var newFoodItem = new FoodItems()
            {
                FoodId = Guid.NewGuid(),
                FoodName = dto.FoodName,
                FoodCategory = dto.FoodCategory,
                FoodExpiryDate = dto.FoodExpiryDate
            };
            var foodItemId = _repository.CreateFoodItem(newFoodItem);
            return foodItemId;
        }

        public List<FoodItemsReadAndUpdateDto> GetAllFoodItems()
        {
            var allFoodItems = _repository.GetAllFoodItems();
            var dtoList = new List<FoodItemsReadAndUpdateDto>();
            foreach (var foodItem in allFoodItems)
            {
                dtoList.Add(new FoodItemsReadAndUpdateDto()
                {
                    FoodId = foodItem.FoodId,
                    FoodName=foodItem.FoodName,
                    FoodCategory=foodItem.FoodCategory,
                    FoodExpiryDate=foodItem.FoodExpiryDate
                });
            }
            return dtoList;
        }

        public bool UpdateFoodItem(FoodItemsReadAndUpdateDto foodItem)
        {
            var isUpdateSuccessful = _repository.UpdateFoodItem(foodItem.FoodId, foodItem.FoodName, foodItem.FoodCategory, foodItem.FoodExpiryDate);
            if (isUpdateSuccessful == false)
            {
                _logger.LogError("[FoodSaverService] Can't find food item Id");
            }
            return isUpdateSuccessful;
        }

        public bool DeleteFoodItem(FoodItemsDeleteDto foodItem)
        {
            var isDeleteSuccessful = _repository.DeleteFoodItem(foodItem.FoodId);
            if (isDeleteSuccessful == false)
            {
                _logger.LogError("[FoodSaverService] Can't find food item Id");
            }
            return isDeleteSuccessful;
        }

        public void SendFoodExpiryReminder(int daysToExpiry)
        {
            var foodExpiryReminder = _repository.SendFoodExpiryReminder(daysToExpiry);
            foreach (var item in foodExpiryReminder)
            {
                // Simulate sending a reminder (email, push notification, etc.)
                _logger.LogInformation($"Reminder: '{item.FoodName}' expires on {item.FoodExpiryDate:d}");
            }
        }
    }
}
