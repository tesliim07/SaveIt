namespace FoodSaver.Models
{
    public class FoodItemsCreateDto
    {
        public required string FoodName { get; set; }
        public category FoodCategory { get; set; }
        public required DateTime FoodExpiryDate { get; set; }
    }

    public class FoodItemsUpdateDto
    {
        public required string FoodName { get; set; }
        public category FoodCategory { get; set; }
        public required DateTime FoodExpiryDate { get; set; }
    }
}
