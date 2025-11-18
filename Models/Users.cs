using System.ComponentModel.DataAnnotations;

namespace FoodSaver.Models
{
    public class Users
    {
        [Key]
        public Guid UserId { get; set; }
        public required string UserEmail { get; set; }
        public string? UserName { get; set; }
        public required string ProviderId { get; set; }
        public DateTime CreatedUser { get; set; }

        //One user can have multiple Food Items
        //public List<FoodItems> FoodItems { get; set; } = new List<FoodItems>();
    }
}
