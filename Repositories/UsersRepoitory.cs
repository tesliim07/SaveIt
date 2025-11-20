using FoodSaver.Contexts;
using FoodSaver.Models;
using FoodSaver.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodSaver.Repositories
{
    public class UsersRepoitory : IUsersRepository
    {
        private readonly FoodSaverDbContext _context;
        public UsersRepoitory(FoodSaverDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateUser(Users user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.UserId;
        }

        public async Task<Users> GetUserById(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            return user;
        }

        public async Task<Users> GetUserByProviderId(string providerId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ProviderId == providerId);
            return user;
        }
    }
}
