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

        public Users GetUserByGoogleId(string providerId)
        {
            var user = _context.Users.FirstOrDefault(u => u.ProviderId == providerId);
            _context.Dispose();
            return user;
        }
    }
}
