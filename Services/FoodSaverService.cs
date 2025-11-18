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
        private readonly IUsersRepository _usersRepository;
        public FoodSaverService(IFoodSaverRepository foodRepository, ILogger<FoodSaverService> logger, IEmailService emailService, IUsersRepository usersRepository)
        {
            _foodRepository = foodRepository;
            _logger = logger;
            _emailService = emailService;
            _usersRepository = usersRepository;
        }

        public Guid CreateFoodItem(FoodItemsCreateDto foodItemsCreate, string providerId)
        {
            
            var user = _usersRepository.GetUserByProviderId(providerId);
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
            var foodItemId = _foodRepository.CreateFoodItem(newFoodItem);
            return foodItemId;
        }

        public List<FoodItemsReadAndUpdateDto> GetAllFoodItems()
        {
            var allFoodItems = _foodRepository.GetAllFoodItems();
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

        public List<FoodItemsReadAndUpdateDto> GetByUserId(string providerId){
            var user = _usersRepository.GetUserByProviderId(providerId);
            if (user == null)
            {
                _logger.LogError("[FoodSaverService] Can't find User");
                throw new Exception("User not found. Please log in first.");
            }
            var foodItems = _foodRepository.GetByUserId(user.UserId);
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

        public bool UpdateFoodItem(FoodItemsReadAndUpdateDto foodItem)
        {
            var isUpdateSuccessful = _foodRepository.UpdateFoodItem(foodItem.FoodId, foodItem.FoodName, foodItem.FoodCategory, foodItem.FoodExpiryDate);
            if (isUpdateSuccessful == false)
            {
                _logger.LogError("[FoodSaverService] Can't find food item Id");
            }
            return isUpdateSuccessful;
        }

        public bool DeleteFoodItem(FoodItemsDeleteDto foodItem)
        {
            var isDeleteSuccessful = _foodRepository.DeleteFoodItem(foodItem.FoodId);
            if (isDeleteSuccessful == false)
            {
                _logger.LogError("[FoodSaverService] Can't find food item Id");
            }
            return isDeleteSuccessful;
        }

        public void SendFoodExpiryReminder(int daysToExpiry)
        {
            //var user = _usersRepository.GetUserByProviderId(providerId);
            //if (user == null)
            //{
            //    _logger.LogError("[FoodSaverService] Can't find User");
            //    throw new Exception("User not found.");
            //}
            var foodExpiryReminderGrupedByUser = _foodRepository.SendFoodExpiryReminder(daysToExpiry);
            _logger.LogInformation($"[FoodSaverService], {foodExpiryReminderGrupedByUser}");
            foreach (var group in foodExpiryReminderGrupedByUser)
            {
                var user = _usersRepository.GetUserById(group.Key);
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
                SendEmail(to, subject, htmlbody);
            }
            
        }

        public Task<bool> SendEmail(string to, string subject, string htmlBody)
        {
            var saveItEmail = _emailService.SendEmail(to, subject, htmlBody);
            return saveItEmail;
        }
    }
}
