using SampleNo.Entity;

namespace SampleNo.Repositories.IRepositories
{
    public interface IStoryRepository
    {
        Task<IEnumerable<Story>> GetAllStoriesAsync();
        Task<IEnumerable<Story>> GetStoryByUserIdAsync(string Userid);
        Task<IEnumerable<Story>> GetStoryByUserIdAsyncNOTEXPIRED(string Userid);
        Task<IEnumerable<Story?>> GetScheduledStories();
        Task<Story?> GetStoryByIdAsync(int id);
        Task<Story> AddStoryAsync(Story story);
        Task<Story> UpdateStoryAsync(Story story);
        Task<bool> DeleteStoryAsync(int id);
    }
}
