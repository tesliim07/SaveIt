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

        public Guid CreateFoodItem(FoodItemsDto foodItemsCreate)
        {
            var dto = new FoodItemsDto()
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

        public List<FoodItemsDto> GetAllFoodItems()
        {
            var allFoodItems = _repository.GetAllFoodItems();
            var dtoList = new List<FoodItemsDto>();
            foreach (var foodItem in allFoodItems)
            {
                dtoList.Add(new FoodItemsDto()
                {
                    FoodName=foodItem.FoodName,
                    FoodCategory=foodItem.FoodCategory,
                    FoodExpiryDate=foodItem.FoodExpiryDate
                });
            }
            return dtoList;
        }

        public bool UpdateFoodItem(FoodItemsUpdateDto foodItem)
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
    }
}
