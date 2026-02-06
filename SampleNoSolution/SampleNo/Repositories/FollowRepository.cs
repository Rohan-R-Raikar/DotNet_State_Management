using Microsoft.EntityFrameworkCore;
using SampleNo.Data;
using SampleNo.Entity;
using SampleNo.Repositories.IRepositories;

namespace SampleNo.Repositories
{
    public class FollowRepository : IFollowRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FollowRepository> _logger;
        public FollowRepository(ILogger<FollowRepository> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _context = applicationDbContext;
        }
        public async Task<Follow> FollowUserAsync(string followerId, string followeeId)
        {
            try
            {
                _logger.LogInformation("Follwing a User");
                var alreadyFollowing =await _context.Follows.AnyAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId);
                if (alreadyFollowing)
                {
                    throw new InvalidOperationException("Already following this user.");
                }

                var follow = new Follow
                {
                    FollowerId = followerId,
                    FolloweeId = followeeId,
                    FollowedAt = DateTime.UtcNow
                };

                await _context.AddAsync(follow);
                await _context.SaveChangesAsync();
                return follow;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error ocured at FollowUserAsync");
                throw new NotImplementedException();
            }
        }
        public async Task<bool> UnfollowUserAsync(string followerId, string followeeId)
        {
            try
            {
                var isFollowing = await _context.Follows.AnyAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId);
                if (!isFollowing)
                    throw new InvalidOperationException("This User Is not Following this user");
                var followings = await _context.Follows.FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId);

                _context.Remove(followings);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                throw new NotImplementedException();
            }
        }
        public async Task<IEnumerable<Follow>> GetFollowersAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Fetching users Followers");

                var followers = await _context.Follows.Include(f => f.Follwer)
                                                      .Where(f => f.FolloweeId == userId)
                                                      .AsNoTracking()
                                                      .ToListAsync();
                return followers;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Fetching users Follwings");
                throw new NotImplementedException();
            }
        }

        public async Task<IEnumerable<Follow>> GetFollowingAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Fetching users Follwings");
                var followings = await _context.Follows.Include(f => f.Followee)
                                                                    .Where(f => f.FollowerId == userId)
                                                                    .AsNoTracking()
                                                                    .ToListAsync();
                return (followings);
            }
            catch (Exception ex) 
            {
                _logger.LogError($"{ex.Message}");
                throw new NotImplementedException();
            }
        }

        public async Task<bool> IsfollowUserAsync(string followerId, string followeeId)
        {
            try
            {
                return await _context.Follows.AnyAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId);
            }
            catch(Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                throw new NotImplementedException();
            }
        }

        
    }
}
