namespace FoodSaver.Services.Interfaces
{
    public interface IUsersService
    {
        public Task<Guid> CreateUserFromGoogleResponse(string email, string name, string providerId);
        public Task<Guid> GetUserIdFromProviderId(string providerId);
    }
}
