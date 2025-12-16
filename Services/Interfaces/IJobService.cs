namespace FoodSaver.Services.Interfaces
{
    public interface IJobService
    {
        public Task RunDeleteJob(Guid userId);
    }
}
