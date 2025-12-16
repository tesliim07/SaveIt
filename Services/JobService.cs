using FoodSaver.Repositories.Interfaces;
using FoodSaver.Services.Interfaces;

namespace FoodSaver.Services
{
    public class JobService : IJobService
    {
        private readonly IFoodSaverRepository _foodRepository;
        public JobService(IFoodSaverRepository foodRepository)
        {
            _foodRepository = foodRepository;
        }

        public async Task RunDeleteJob(Guid userId)
        {
            await _foodRepository.DeleteExpiredFood(userId);
        }
    }
}
