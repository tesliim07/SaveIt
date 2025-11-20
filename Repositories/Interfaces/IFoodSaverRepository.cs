using FoodSaver.Models;

namespace FoodSaver.Repositories.Interfaces
{
    public interface IFoodSaverRepository
    {
        public Task<Guid> CreateFoodItem(FoodItems fooditem);
        public Task<List<FoodItems>> GetAllFoodItems();
        public Task<List<FoodItems>> GetByUserId(Guid userId);
        public Task<FoodItems> GetByFoodId(Guid foodid);
        public Task<bool> UpdateFoodItem(Guid foodid, string? foodname = null, category? foodcategory = null, DateOnly? foodexpirydate = null);
        public Task<bool> DeleteFoodItem(Guid foodid);
        public Task<List<IGrouping<Guid, FoodItems>>> SendFoodExpiryReminder(int daysToExpiry);
    }
}
