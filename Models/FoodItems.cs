using System.ComponentModel.DataAnnotations;

namespace FoodSaver.Models
{
    public enum category { Vegetables, Fruits, Grains, Protein, Dairy, Beverages, Snacks, Condiments, Spices, Others }
    public class FoodItems
    {
        [Key]
        public Guid FoodId { get; set; }
        public required string FoodName { get; set; }
        public category FoodCategory { get; set; }
        public required DateOnly FoodExpiryDate { get; set; }

        //Each food item belongs to one user
        public Guid UserId { get; set; }
        public Users? User { get; set; } //Navigation property
    }
}
