using SampleNo.Models;

namespace SampleNo.Services.IServices
{
    public interface IFollowService
    {
        Task<IEnumerable<FollowDto>> GetFollowersAsync(string userId);
        Task<IEnumerable<FollowDto>> GetFollowingAsync(string userId);
        Task<FollowDto> FollowUserAsync(string followerId, string followeeId);
        Task<bool> UnfollowUserAsync(string followerId, string followeeId);
        Task<bool> IsfollowUserAsync(string followerId, string followeeId);
    }
}
