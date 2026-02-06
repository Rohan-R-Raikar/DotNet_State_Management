using SampleNo.Entity;

namespace SampleNo.Repositories.IRepositories
{
    public interface ILikeRepository
    {
        Task<Like> AddLikeAsync(Like like);
        Task<bool> RemoveLikeAsync(string userId, string targetId, LikeTargetType targetType);
        Task<bool> IsLikedAsync(string userId, string targetId, LikeTargetType targetType);
        Task<int> CountLikesAsync(string targetId, LikeTargetType targetType);
        Task<IEnumerable<Like>> GetLikesByUserAsync(string userId);
        Task<IEnumerable<Like>> GetLikesByTargetAsync(string targetId, LikeTargetType targetType);
    }
}
