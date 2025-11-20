using FoodSaver.Models;

namespace FoodSaver.Services.Interfaces
{
    public interface IFoodSaverService
    {
        public Task<Guid> CreateFoodItem(FoodItemsCreateDto foodItemsCreate, string providerId);
        public Task<List<FoodItemsReadAndUpdateDto>> GetAllFoodItems();
        public Task<List<FoodItemsReadAndUpdateDto>> GetByUserId(string providerId);
        public Task<bool> UpdateFoodItem(FoodItemsReadAndUpdateDto foodItem);
        public Task<bool> DeleteFoodItem(FoodItemsDeleteDto foodItem);
        public Task SendFoodExpiryReminder(int daysToExpiry);
        public Task<bool> SendEmail(string to, string subject, string htmlBody);
    }
}
