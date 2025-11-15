namespace FoodSaver.Services.Interfaces
{
    public interface IJWTService
    {
        public string GenerateToken(string providerId, string email);
    }
}
