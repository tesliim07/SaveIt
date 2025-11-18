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
        public Guid CreateUserFromGoogleResponse(string email, string name, string providerId)
        {
            var checkIfUserExists = _usersRepository.GetUserByProviderId(providerId);
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
                var newUserId = _usersRepository.CreateUser(newUser);
                return newUserId;
            }
            else
            {
                _logger.LogInformation($"[UsersServices], User already exist with userId {checkIfUserExists.UserId}");
                return checkIfUserExists.UserId;
            }
        }
    }
}
