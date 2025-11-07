using FoodSaver.Contexts;
using FoodSaver.Models;
using FoodSaver.Repositories.Interfaces;

namespace FoodSaver.Repositories
{
    public class FoodSaverRepository : IFoodSaverRepository
    {
        private readonly FoodSaverDbContext _context;

        public FoodSaverRepository(FoodSaverDbContext context)
        {
            _context = context;
        }

        public Guid CreateFoodItem(FoodItems fooditem)
        {
            _context.FoodItems.Add(fooditem);
            _context.SaveChanges();
            return fooditem.FoodId;
        }

        public List<FoodItems> GetAllFoodItems()
        {
            var fooditems = _context.FoodItems.OrderByDescending(
                fooditems => fooditems.FoodExpiryDate).ToList();
            _context.Dispose();
            return fooditems;
        }

        public FoodItems GetByFoodId(Guid foodid)
        {
            var existingFoodItem = _context.FoodItems
                .Where(existingFoodItem => existingFoodItem.FoodId == foodid)
                .FirstOrDefault();
            return existingFoodItem;
        }

        public bool UpdateFoodItem(Guid foodid, string? foodname, category? foodcategory, DateOnly? foodexpirydate)
        {
            var existingFoodItem = GetByFoodId(foodid);
            if (existingFoodItem == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(foodname))
            {
                existingFoodItem.FoodName = foodname;
            }
            if (foodcategory.HasValue)
            {
                existingFoodItem.FoodCategory = (category) foodcategory;
            }
            if (foodexpirydate.HasValue)
            {
                existingFoodItem.FoodExpiryDate = (DateOnly) foodexpirydate;
            }
            _context.SaveChanges();
            return true;
            
        }

        public bool DeleteFoodItem(Guid foodid)
        {
            var existingFoodItem = GetByFoodId(foodid);
            if (existingFoodItem == null) { return false; }
            _context.FoodItems.Remove(existingFoodItem);
            _context.SaveChanges();
            return true;
        }

        public List<FoodItems> SendFoodExpiryReminder(int daysToExpiry)
        {
            var todaysDate = DateOnly.FromDateTime(DateTime.Now); ;
            var allFoodItems = GetAllFoodItems();
            var expiryReminderItems = new List<FoodItems>();
            foreach(var foodItem in allFoodItems)
            {
                if (foodItem.FoodExpiryDate <= todaysDate.AddDays(daysToExpiry))
                {
                    expiryReminderItems.Add(foodItem);
                }
            }
            return expiryReminderItems;
        }
    }
}
