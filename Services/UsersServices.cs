using FoodSaver.Models;
using FoodSaver.Repositories.Interfaces;
using FoodSaver.Services.Interfaces;

namespace FoodSaver.Services
{
    public class UsersServices : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ILogger _logger;
        public UsersServices(IUsersRepository usersRepository, ILogger<UsersServices> logger)
        {
            _usersRepository = usersRepository;
            _logger = logger;
        }
        public async Task<Guid> CreateUserFromGoogleResponse(string email, string name, string providerId)
        {
            var checkIfUserExists = await _usersRepository.GetUserByProviderId(providerId);
            if (checkIfUserExists == null)
            {

                var newUser = new Users
                {
                    UserId = Guid.NewGuid(),
                    UserEmail = email,
                    UserName = name,
                    ProviderId = providerId,
                    CreatedUser = DateTime.UtcNow
                };
                var newUserId = await _usersRepository.CreateUser(newUser);
                return newUserId;
            }
            else
            {
                _logger.LogInformation($"[UsersServices], User already exist with userId {checkIfUserExists.UserId}");
                return checkIfUserExists.UserId;
            }
        }

        public async Task<Guid> GetUserIdFromProviderId(string providerId)
        {
            var user = await _usersRepository.GetUserByProviderId(providerId);
            if (user == null)
            {
                _logger.LogError($"[UsersServices], No user found with providerId {providerId}");
                throw new Exception("User not found");
            }
            return user.UserId;
        }
    }
}
