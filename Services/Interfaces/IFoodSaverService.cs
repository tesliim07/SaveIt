using FoodSaver.Models;

namespace FoodSaver.Services.Interfaces
{
    public interface IFoodSaverService
    {
        public Task<Guid> CreateFoodItem(FoodItemsCreateDto foodItemsCreate, string providerId);
        public Task<List<FoodItemsReadAndUpdateDto>> GetAllFoodItems();
        public Task<List<FoodItemsReadAndUpdateDto>> GetFoodItemsByUserProviderId(string providerId);
        public Task<List<FoodItemsReadAndUpdateDto>> GetExpiringItemsByUserProviderId(string providerId);
        public Task<bool> UpdateFoodItem(FoodItemsReadAndUpdateDto foodItem);
        public Task<bool> DeleteFoodItem(Guid foodId);
        public Task<bool> DeleteExpiredFood(Guid userId);
        public Task DeleteUsersExpiredFood();
        public Task SendFoodExpiryReminder(int daysToExpiry);
        public Task<bool> SendEmail(string to, string subject, string htmlBody);
    }
}
