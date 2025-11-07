namespace FoodSaver.Models
{
    public class FoodItemsCreateDto
    {
        public required string FoodName { get; set; }
        public category FoodCategory { get; set; }
        public required DateOnly FoodExpiryDate { get; set; }
    }

    public class FoodItemsReadAndUpdateDto
    {
        public Guid FoodId { get; set; }
        public required string FoodName { get; set; }
        public category FoodCategory { get; set; }
        public required DateOnly FoodExpiryDate { get; set; }
    }

    public class FoodItemsDeleteDto
    {
        public Guid FoodId { get; set; }
    }
}
