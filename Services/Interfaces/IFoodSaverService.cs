using FoodSaver.Models;

namespace FoodSaver.Services.Interfaces
{
    public interface IFoodSaverService
    {
        public Guid CreateFoodItem(FoodItemsDto foodItemsCreate);
        public List<FoodItemsDto> GetAllFoodItems();
        public bool UpdateFoodItem(FoodItemsUpdateDto foodItem);
        public bool DeleteFoodItem(FoodItemsDeleteDto foodItem);
    }
}
