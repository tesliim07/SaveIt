using FoodSaver.Models;

namespace FoodSaver.Repositories.Interfaces
{
    public interface IFoodSaverRepository
    {
        public Guid CreateFoodItem(FoodItems fooditem);
        public List<FoodItems> GetAllFoodItems();
        public FoodItems GetByFoodId(Guid foodid);
        public bool UpdateFoodItem(Guid foodid, string? foodname = null, category? foodcategory = null, DateOnly? foodexpirydate = null);
        public bool DeleteFoodItem(Guid foodid);
        public List<FoodItems> SendFoodExpiryReminder(int daysToExpiry);
    }
}
