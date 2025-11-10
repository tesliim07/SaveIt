using FoodSaver.Models;

namespace FoodSaver.Services.Interfaces
{
    public interface IFoodSaverService
    {
        public Guid CreateFoodItem(FoodItemsCreateDto foodItemsCreate);
        public List<FoodItemsReadAndUpdateDto> GetAllFoodItems();
        public bool UpdateFoodItem(FoodItemsReadAndUpdateDto foodItem);
        public bool DeleteFoodItem(FoodItemsDeleteDto foodItem);
        public void SendFoodExpiryReminder(int daysToExpiry);
        public Task<bool> SendEmail(string to, string subject, string htmlBody);
    }
}
