using SampleNo.Models;

namespace SampleNo.Services.IServices
{
    public interface IStoryService
    {
        Task<IEnumerable<StoryDto>> GetAllStoriesAsync();
        Task<IEnumerable<StoryDto>> GetStoryByUserIdAsync(string Userid);
        Task<IEnumerable<StoryDto>> GetStoryByUserIdAsyncNOTEXPIRED(string Userid);
        Task<StoryDto?> GetStoryByIdAsync(int id);
        Task<StoryDto> AddStoryAsync(StoryDto storydto);
        Task<StoryDto> UpdateStoryAsync(StoryDto storydto);
        Task<bool> DeleteStoryAsync(int id);
    }
}
