using FoodSaver.Models;
using FoodSaver.Repositories.Interfaces;
using FoodSaver.Services.Interfaces;

namespace FoodSaver.Services
{
    public class UsersServices : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        public UsersServices(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }
        public Guid CreateUserFromGoogleResponse(string email, string name, string providerId)
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
    }
}
