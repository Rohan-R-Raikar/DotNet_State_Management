using Microsoft.EntityFrameworkCore;
using SampleNo.Data;
using SampleNo.Entity;
using SampleNo.Repositories.IRepositories;

namespace SampleNo.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _appDbCntxt;
        private readonly ILogger<PostRepository> _logger;
        public PostRepository(ApplicationDbContext applicationDb,
            ILogger<PostRepository> logger)
        {
            _appDbCntxt = applicationDb;
            _logger = logger;
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            try
            {
                _logger.LogInformation("fetching GetAllPostsAsync at PostRepository");
                return await _appDbCntxt.Set<Post>()
                .AsNoTracking()
                .ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError("Error Occured at GetAllPostsAsync");
                throw;
            }
        }
        public async Task<Post?> GetPostByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Post #{id}");
                return await _appDbCntxt.Set<Post>()
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Post?>> GetScheduledPostsOnly()
        {
            try
            {
                _logger.LogInformation("Fetching all the Scheduled Posts");

                return await _appDbCntxt.Set<Post>()
                    .Where(p => p.PublishAt >= DateTime.UtcNow)
                    .Include(p =>p.User)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch(Exception ex )
            {
                _logger.LogError($"{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Post>> GetPostByUserIdAsync(string Userid)
        {
            try
            {
                _logger.LogInformation($"Post #{Userid}");

                return await _appDbCntxt.Set<Post>()
                .Where(p => p.UserId == Userid)
                .Include(p => p.User)
                .AsNoTracking()
                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                throw;
            }
        }

        public async Task<Post> AddPostAsync(Post post)
        {
            try
            {
                _logger.LogInformation($"Post #{post.Id}");
                await _appDbCntxt.Set<Post>().AddAsync(post);
                await _appDbCntxt.SaveChangesAsync();
                return post;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
        public async Task<Post> UpdatePostAsync(Post post)
        {
            try
            {
                _logger.LogInformation("Updating the Post Information");
                _appDbCntxt.Set<Post>().Update(post);
                await _appDbCntxt.SaveChangesAsync();
                return post;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting the Post ");
                var post = await GetPostByIdAsync(id);
                if (post == null) return false;
                _appDbCntxt.Set<Post>().Remove(post);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
