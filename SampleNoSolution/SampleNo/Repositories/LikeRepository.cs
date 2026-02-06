using Microsoft.EntityFrameworkCore;
using SampleNo.Data;
using SampleNo.Entity;
using SampleNo.Repositories.IRepositories;

namespace SampleNo.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LikeRepository> _logger;
        public LikeRepository(ILogger<LikeRepository> logger, ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
            _logger = logger;
        }
        public async Task<Like> AddLikeAsync(Like like)
        {
            try
            {
                _logger.LogInformation("Logg Message here");

                var existingLike = await _context.Likes
                    .FirstOrDefaultAsync(l => l.TargetId == like.TargetId && l.UserId == like.UserId && l.TargetType == like.TargetType);

                if (existingLike != null) throw new Exception("User has already Liked");

                _context.Add(like);
                _context.SaveChanges();
                return like;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<int> CountLikesAsync(string targetId, LikeTargetType targetType)
        {
            try
            {
                _logger.LogInformation("Logg Message here");
                var count = await _context.Likes.Where(l => l.TargetId == targetId && l.TargetType == targetType).CountAsync();
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Like>> GetLikesByTargetAsync(string targetId, LikeTargetType targetType)
        {
            try
            {
                _logger.LogInformation("Logg Message here");
                var likesByTarget = await _context.Likes.Where(x => x.TargetId == targetId && x.TargetType == targetType).AsNoTracking().ToListAsync();
                return likesByTarget;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Like>> GetLikesByUserAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Logg Message here");
                var likesByUser = await _context.Likes.Where(u => u.UserId == userId).AsNoTracking().ToListAsync();
                return likesByUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<bool> IsLikedAsync(string userId, string targetId, LikeTargetType targetType)
        {
            try
            {
                _logger.LogInformation("Logg Message here");

                var isLiked =  _context.Likes.Where(u => u.UserId == userId && u.TargetId == targetId && u.TargetType == targetType);
                if (isLiked.Any())
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<bool> RemoveLikeAsync(string userId, string targetId, LikeTargetType targetType)
        {
            try
            {
                _logger.LogInformation("Logg Message here");
                var like = _context.Likes.Where(l => l.UserId == userId && l.TargetId == targetId && l.TargetType == targetType);
                if (like == null)
                {
                    return false;
                }

                var liked = await _context.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.TargetId == targetId && l.TargetType == targetType);

                _context.Likes.Remove(liked);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
