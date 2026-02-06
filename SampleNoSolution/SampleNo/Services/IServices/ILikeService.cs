using SampleNo.Models;
using SampleNo.Entity;

namespace SampleNo.Services.IServices
{
    public interface ILikeService
    {
        Task<LikeDto> AddLikeAsync(LikeDto likeDto);
        Task<bool> RemoveLikeAsync(string userId, string targetId, LikeTargetType targetType);
        Task<bool> IsLikedAsync(string userId, string targetId, LikeTargetType targetType);
        Task<int> CountLikesAsync(string targetId, LikeTargetType targetType);
        Task<IEnumerable<LikeDto>> GetLikesByUserAsync(string userId);
        Task<IEnumerable<LikeDto>> GetLikesByTargetAsync(string targetId, LikeTargetType targetType);
    }
}
