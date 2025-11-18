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
        public List<FoodItems> GetByUserId(Guid userId)
        {
            var foodItems = _context.FoodItems.Where(foodItems => foodItems.UserId == userId)
                .OrderByDescending(foodItems => foodItems.FoodExpiryDate)
                .ToList();
            _context.Dispose();
            return foodItems;
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

        public List<IGrouping<Guid,FoodItems>> SendFoodExpiryReminder(int daysToExpiry)
        {
            var todaysDate = DateOnly.FromDateTime(DateTime.Now); ;
            var expiringFoodItems = _context.FoodItems.
                Where(foodItems => foodItems.FoodExpiryDate > todaysDate && foodItems.FoodExpiryDate <= todaysDate.AddDays(daysToExpiry))
                .ToList();
            var foodItemsByUser = expiringFoodItems
                .GroupBy(foodItems => foodItems.UserId)
                .ToList();
            return foodItemsByUser;
        }
    }
}
