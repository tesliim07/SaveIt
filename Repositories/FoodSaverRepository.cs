using FoodSaver.Contexts;
using FoodSaver.Models;
using FoodSaver.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodSaver.Repositories
{
    public class FoodSaverRepository : IFoodSaverRepository
    {
        private readonly FoodSaverDbContext _context;

        public FoodSaverRepository(FoodSaverDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateFoodItem(FoodItems fooditem)
        {
            await _context.FoodItems.AddAsync(fooditem);
            await _context.SaveChangesAsync();
            return fooditem.FoodId;
        }

        public async Task<List<FoodItems>> GetAllFoodItems()
        {
            var fooditems = await _context.FoodItems.OrderBy(
                fooditems => fooditems.FoodExpiryDate).ToListAsync();
            return fooditems;
        }

        public async Task<List<FoodItems>> GetByUserId(Guid userId)
        {
            var foodItems = await _context.FoodItems.Where(foodItems => foodItems.UserId == userId)
                .OrderBy(foodItems => foodItems.FoodExpiryDate)
                .ToListAsync();
            return foodItems;
        }

        public async Task<List<FoodItems>> GetExpiringItemsByUserId(Guid userId)
        {
            var todaysDate = DateOnly.FromDateTime(DateTime.Now);
            var foodItems = await _context.FoodItems.Where(foodItems => foodItems.UserId == userId && foodItems.FoodExpiryDate > todaysDate && foodItems.FoodExpiryDate <= todaysDate.AddDays(3))
                .OrderBy(foodItems => foodItems.FoodExpiryDate)
                .ToListAsync();
            return foodItems;
        }

        public async Task<FoodItems> GetByFoodId(Guid foodid)
        {
            var existingFoodItem = await _context.FoodItems
                .Where(existingFoodItem => existingFoodItem.FoodId == foodid)
                .FirstOrDefaultAsync();
            return existingFoodItem;
        }

        public async Task<bool> UpdateFoodItem(Guid foodid, string? foodname, category? foodcategory, DateOnly? foodexpirydate)
        {
            var existingFoodItem = await GetByFoodId(foodid);
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

        public async Task<bool> DeleteFoodItem(Guid foodid)
        {
            var existingFoodItem = await GetByFoodId(foodid);
            if (existingFoodItem == null) { return false; }
            _context.FoodItems.Remove(existingFoodItem);
            await _context.SaveChangesAsync();
            return true;
        }

        //public async Task DeleteExpiredFood()
        //{
        //    var todaysDate = DateOnly.FromDateTime(DateTime.Now);
        //    var foods = await _context.FoodItems
        //        .Where(foodItems => foodItems.FoodExpiryDate <= todaysDate)
        //        .ToListAsync();
        //    foreach(var food in foods)
        //    {
        //        _context.FoodItems.Remove(food);
        //        await _context.SaveChangesAsync();
        //    }
        //}

        public async Task<bool> DeleteExpiredFood(Guid userId)
        {
            var todaysDate = DateOnly.FromDateTime(DateTime.Now);
            var expiredUserFoods = await _context.FoodItems.Where(foodItems => foodItems.UserId == userId && foodItems.FoodExpiryDate <= todaysDate)
                .OrderBy(foodItems => foodItems.FoodExpiryDate)
                .ToListAsync();
            foreach (var expiredFood in expiredUserFoods)
            {
                _context.FoodItems.Remove(expiredFood);
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<List<IGrouping<Guid,FoodItems>>> SendFoodExpiryReminder(int daysToExpiry)
        {
            var todaysDate = DateOnly.FromDateTime(DateTime.Now);
            var expiringFoodItems = await _context.FoodItems.
                Where(foodItems => foodItems.FoodExpiryDate > todaysDate && foodItems.FoodExpiryDate <= todaysDate.AddDays(daysToExpiry))
                .ToListAsync();
            var foodItemsByUser = expiringFoodItems
                .GroupBy(foodItems => foodItems.UserId)
                .ToList();
            return foodItemsByUser;
        }
    }
}