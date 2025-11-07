using FoodSaver.Models;

namespace FoodSaver.Services.Interfaces
{
    public interface IFoodSaverService
    {
        public Guid CreateFoodItem(FoodItemsCreateDto foodItemsCreate);
        public List<FoodItemsReadAndUpdateDto> GetAllFoodItems();
        public bool UpdateFoodItem(FoodItemsReadAndUpdateDto foodItem);
        public bool DeleteFoodItem(FoodItemsDeleteDto foodItem);
    }
}
