using FoodSaver.Models;

namespace FoodSaver.Services.Interfaces
{
    public interface IUsersService
    {
        public Task<Guid> CreateUserFromGoogleResponse(string email, string name, string providerId);
        public Task<Users> GetUserById(Guid userId);
        public Task<Users> GetUserByProviderId(string providerId);
        public Task<bool> UpdateDeleteDecision(string providerId, bool deleteDecision);
        public Task<List<Users>> GetUsersWithDeleteDecisionTrue();
    }
}
