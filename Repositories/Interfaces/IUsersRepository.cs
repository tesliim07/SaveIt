using FoodSaver.Models;

namespace FoodSaver.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        public Guid CreateUser(Users user);
        public Users GetUserById(Guid userId);
        public Users GetUserByProviderId(string providerId);
    }
}
