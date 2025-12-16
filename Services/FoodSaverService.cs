using FoodSaver.Models;
using FoodSaver.Repositories.Interfaces;
using FoodSaver.Services.Interfaces;
using System.Text;

namespace FoodSaver.Services
{
    public class FoodSaverService : IFoodSaverService
    {
        private readonly IFoodSaverRepository _foodRepository;
        private readonly ILogger _logger;
        private readonly IEmailService _emailService;
        private readonly IUsersService _usersService;
        public FoodSaverService(IFoodSaverRepository foodRepository, ILogger<FoodSaverService> logger, IEmailService emailService, IUsersService usersService)
        {
            _foodRepository = foodRepository;
            _logger = logger;
            _emailService = emailService;
            _usersService = usersService;
        }

        public async Task<Guid> CreateFoodItem(FoodItemsCreateDto foodItemsCreate, string providerId)
        {
            
            var user = await _usersService.GetUserByProviderId(providerId);
            if (user == null)
            {
                throw new Exception("User not found. Please log in first.");
            }
            var dto = new FoodItemsCreateDto()
            {
                FoodName = foodItemsCreate.FoodName,
                FoodCategory = foodItemsCreate.FoodCategory,
                FoodExpiryDate = foodItemsCreate.FoodExpiryDate
            };
            var newFoodItem = new FoodItems()
            {
                FoodId = Guid.NewGuid(),
                FoodName = dto.FoodName,
                FoodCategory = dto.FoodCategory,
                FoodExpiryDate = dto.FoodExpiryDate,
                UserId = user.UserId
            };
            var foodItemId = await _foodRepository.CreateFoodItem(newFoodItem);
            return foodItemId;
        }

        public async Task<List<FoodItemsReadAndUpdateDto>> GetAllFoodItems()
        {
            var allFoodItems = await _foodRepository.GetAllFoodItems();
            var dtoList = new List<FoodItemsReadAndUpdateDto>();
            foreach (var foodItem in allFoodItems)
            {
                dtoList.Add(new FoodItemsReadAndUpdateDto()
                {
                    FoodId = foodItem.FoodId,
                    FoodName=foodItem.FoodName,
                    FoodCategory=foodItem.FoodCategory,
                    FoodExpiryDate=foodItem.FoodExpiryDate
                });
            }
            return dtoList;
        }

        public async Task<List<FoodItemsReadAndUpdateDto>> GetFoodItemsByUserProviderId(string providerId){
            var user = await _usersService.GetUserByProviderId(providerId);
            if (user == null)
            {
                _logger.LogError("[FoodSaverService] Can't find User");
                throw new Exception("User not found. Please log in first.");
            }
            var foodItems = await _foodRepository.GetByUserId(user.UserId);
            var dtoList = new List<FoodItemsReadAndUpdateDto>();
            foreach (var foodItem in foodItems)
            {
                dtoList.Add(new FoodItemsReadAndUpdateDto()
                {
                    FoodId = foodItem.FoodId,
                    FoodName = foodItem.FoodName,
                    FoodCategory = foodItem.FoodCategory,
                    FoodExpiryDate = foodItem.FoodExpiryDate
                });
            }
            return dtoList;
        }

        public async Task<List<FoodItemsReadAndUpdateDto>> GetExpiringItemsByUserProviderId(string providerId)
        {
            var user = await _usersService.GetUserByProviderId(providerId);
            if (user == null)
            {
                _logger.LogError("[FoodSaverService] Can't find User");
                throw new Exception("User not found. Please log in first.");
            }
            var foodItems = await _foodRepository.GetExpiringItemsByUserId(user.UserId);
            var dtoList = new List<FoodItemsReadAndUpdateDto>();
            foreach (var foodItem in foodItems)
            {
                dtoList.Add(new FoodItemsReadAndUpdateDto()
                {
                    FoodId = foodItem.FoodId,
                    FoodName = foodItem.FoodName,
                    FoodCategory = foodItem.FoodCategory,
                    FoodExpiryDate = foodItem.FoodExpiryDate
                });
            }
            return dtoList;
        }

        public async Task<bool> UpdateFoodItem(FoodItemsReadAndUpdateDto foodItem)
        {
            var isUpdateSuccessful = await _foodRepository.UpdateFoodItem(foodItem.FoodId, foodItem.FoodName, foodItem.FoodCategory, foodItem.FoodExpiryDate);
            if (isUpdateSuccessful == false)
            {
                _logger.LogError("[FoodSaverService] Can't find food item Id");
            }
            return isUpdateSuccessful;
        }

        public async Task<bool> DeleteFoodItem(Guid foodId)
        {
            var isDeleteSuccessful = await _foodRepository.DeleteFoodItem(foodId);
            if (isDeleteSuccessful == false)
            {
                _logger.LogError("[FoodSaverService] Can't find food item Id");
            }
            return isDeleteSuccessful;
        }

        public async Task SendFoodExpiryReminder(int daysToExpiry)
        {
            var foodExpiryReminderGrupedByUser = await _foodRepository.SendFoodExpiryReminder(daysToExpiry);
            _logger.LogInformation($"[FoodSaverService], {foodExpiryReminderGrupedByUser}");
            foreach (var group in foodExpiryReminderGrupedByUser)
            {
                var user = await _usersService.GetUserById(group.Key);
                var userItems = group.ToList();
                var subject = $"Reminder: Your current SaveIt List (expiring soon)";
                var to = user.UserEmail;
                _logger.LogInformation($"[FoodSaverService] Email sent to '{to}' .");
                var messageBuilt = new StringBuilder();
                messageBuilt.Append($"<p>Hi {user.UserName},</p>");
                messageBuilt.Append("<p>The following items are expiring soon:</p>");
                foreach (var item in userItems)
                {
                    messageBuilt.Append(
                        $"<p><strong>Your {item.FoodName}</strong> is expiring soon on <strong>{item.FoodExpiryDate}</strong>.</p>");
                }
                var htmlbody = messageBuilt.ToString();
                await SendEmail(to, subject, htmlbody);
            }
            
        }

        public async Task<bool> DeleteExpiredFood(Guid userId)
        {
            var isDeleted = await _foodRepository.DeleteExpiredFood(userId);
            return isDeleted;
        }

        public async Task DeleteUsersExpiredFood()
        {
            var usersWithDeleteDecisionTrue = await _usersService.GetUsersWithDeleteDecisionTrue();
            foreach (var user in usersWithDeleteDecisionTrue)
            {
                var isDeleted = await _foodRepository.DeleteExpiredFood(user.UserId);
                if (isDeleted)
                {
                    _logger.LogInformation($"[FoodSaverService], Deleted expired food items for user with providerId {user.ProviderId}");
                }
                else
                {
                    _logger.LogInformation($"[FoodSaverService], No expired food items to delete for user with providerId {user.ProviderId}");
                }
            }

        }

        public async Task<bool> SendEmail(string to, string subject, string htmlBody)
        {
            var saveItEmail = await _emailService.SendEmail(to, subject, htmlBody);
            return saveItEmail;
        }
    }
}
