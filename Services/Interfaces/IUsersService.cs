namespace FoodSaver.Services.Interfaces
{
    public interface IUsersService
    {
        public Guid CreateUserFromGoogleResponse(string email, string name, string providerId);
    }
}
