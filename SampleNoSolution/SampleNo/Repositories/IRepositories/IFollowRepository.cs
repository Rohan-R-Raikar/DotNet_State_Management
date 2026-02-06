using SampleNo.Entity;

namespace SampleNo.Repositories.IRepositories
{
    public interface IFollowRepository
    {
        Task<IEnumerable<Follow>> GetFollowersAsync(string userId);
        Task<IEnumerable<Follow>> GetFollowingAsync(string userId);
        Task<Follow> FollowUserAsync(string followerId, string followeeId);
        Task<bool> UnfollowUserAsync(string followerId, string followeeId);
        Task<bool> IsfollowUserAsync(string followerId, string followeeId);
    }
}
