namespace FoodSaver.Models
{
    public class FoodItemsDto
    {
        public required string FoodName { get; set; }
        public category FoodCategory { get; set; }
        public required DateTime FoodExpiryDate { get; set; }
    }

    public class FoodItemsUpdateDto
    {
        public Guid FoodId { get; set; }
        public required string FoodName { get; set; }
        public category FoodCategory { get; set; }
        public required DateTime FoodExpiryDate { get; set; }
    }

    public class FoodItemsDeleteDto
    {
        public Guid FoodId { get; set; }
    }
}
