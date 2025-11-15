using FoodSaver.Models;
using FoodSaver.Repositories.Interfaces;
using FoodSaver.Services.Interfaces;

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
            var foodExpiryReminder = _foodRepository.SendFoodExpiryReminder(daysToExpiry);
            foreach (var item in foodExpiryReminder)
            {
                _logger.LogInformation($"[FoodSaverService] Reminder: '{item.FoodName}' expires on {item.FoodExpiryDate}");
                var subject = $"Reminder: {item.FoodName.ToUpper()} is about to expire";
                var to = "oluwatobiteslim@gmail.com";
                var htmlbody = $"<p><strong>Your {item.FoodName} </strong> is expiring soon on the <strong>{item.FoodExpiryDate}</strong></p>";
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
