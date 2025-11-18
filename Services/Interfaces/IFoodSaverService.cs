using FoodSaver.Models;

namespace FoodSaver.Services.Interfaces
{
    public interface IFoodSaverService
    {
        public Guid CreateFoodItem(FoodItemsCreateDto foodItemsCreate, string providerId);
        public List<FoodItemsReadAndUpdateDto> GetAllFoodItems();
        public List<FoodItemsReadAndUpdateDto> GetByUserId(string providerId);
        public bool UpdateFoodItem(FoodItemsReadAndUpdateDto foodItem);
        public bool DeleteFoodItem(FoodItemsDeleteDto foodItem);
        public void SendFoodExpiryReminder(int daysToExpiry);
        public Task<bool> SendEmail(string to, string subject, string htmlBody);
    }
}
