using FoodSaver.Models;

namespace FoodSaver.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        public Task<Guid> CreateUser(Users user);
        public Task<Users> GetUserById(Guid userId);
        public Task<Users> GetUserByProviderId(string providerId);
        public Task<bool> UpdateDeleteDecision(string providerId, bool deleteDecision);
        public Task<List<Users>> GetUsersWithDeleteDecisionTrue();
    }
}
