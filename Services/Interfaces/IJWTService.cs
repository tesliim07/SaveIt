namespace FoodSaver.Services.Interfaces
{
    public interface IJWTService
    {
        public string GenerateToken(string email, string name);
    }
}
