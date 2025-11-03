using FoodSaver.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodSaver.Contexts
{
    public class FoodSaverDbContext : DbContext
    {
        public FoodSaverDbContext(DbContextOptions<FoodSaverDbContext> options) : base(options)
        {
        }
        public virtual DbSet<FoodItems> FoodItems { get; set; }
    }
}
