using FoodSaver.Contexts;
using FoodSaver.Models;
using FoodSaver.Repositories.Interfaces;

namespace FoodSaver.Repositories
{
    public class UsersRepoitory : IUsersRepository
    {
        private readonly FoodSaverDbContext _context;
        public UsersRepoitory(FoodSaverDbContext context)
        {
            _context = context;
        }

        public Guid CreateUser(Users user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.UserId;
        }

        public Users GetUserById(Guid userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            //_context.Dispose();
            if (user != null)
            {
                return user;
            }
            return null;
        }

        public Users GetUserByProviderId(string providerId)
        {
            var user = _context.Users.FirstOrDefault(u => u.ProviderId == providerId);
            //_context.Dispose();
            if (user != null)
            {
                return user;
            }
            return null;
        }
    }
}
