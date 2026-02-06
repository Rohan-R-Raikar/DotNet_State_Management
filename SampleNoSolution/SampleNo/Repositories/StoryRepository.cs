using Microsoft.EntityFrameworkCore;
using SampleNo.Data;
using SampleNo.Entity;
using SampleNo.Repositories.IRepositories;

namespace SampleNo.Repositories
{
    public class StoryRepository : IStoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StoryRepository> _logger;
        public StoryRepository(ILogger<StoryRepository> logger,
            ApplicationDbContext applicationDb)
        {
            _context = applicationDb;
            _logger = logger;
        }
        public async Task<Story> AddStoryAsync(Story story)
        {
            try
            {
                _logger.LogInformation("Adding new Story Here");
                await _context.Set<Story>().AddAsync(story);
                _context.SaveChanges();
                return story;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error Occured while adding New Stroy at Repository level");
                throw new NotImplementedException();
            }            
        }

        public async Task<bool> DeleteStoryAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting Story Here");
                var story = await GetStoryByIdAsync(id);
                if (story == null) return false;

                _context.Set<Story>().Remove(story);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While Deleting Story");
                throw new NotImplementedException();
            }
        }

        public async Task<IEnumerable<Story>> GetAllStoriesAsync()
        {
            try
            {
                _logger.LogInformation("Fetching All Stories");

                return await _context.Set<Story>()
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Occured While Fetching All Stories");
                throw new NotImplementedException();
            }
        }

        public async Task<IEnumerable<Story?>> GetScheduledStories()
        {
            try
            {
                _logger.LogInformation("Fetching Scheduled Stories");

                return await _context.Set<Story>()
                                     .Where(s => s.Status == ContentStatus.Draft || 
                                     (s.Status == ContentStatus.Published && s.ExpireAt <= DateTime.UtcNow))
                                    .Include(s => s.User)
                                    .AsNoTracking()
                                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Occured While Fetching Scheduled Stories");
                throw new NotImplementedException();
            }
        }

        public async Task<Story?> GetStoryByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Error at FindingStory");
                return await _context.Set<Story>().FirstOrDefaultAsync(a => a.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Occured while Fetching the Story");
                throw new NotImplementedException();
            }
        }

        public async Task<IEnumerable<Story>> GetStoryByUserIdAsync(string Userid)
        {
            try
            {
                _logger.LogInformation("Error at FindingStory");
                return await _context.Set<Story>()
                    .Where(p => p.UserId == Userid)
                    .Include(p => p.User)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Occured while Fetching the Story");
                throw new NotImplementedException();
            }
        }

        public async Task<IEnumerable<Story>> GetStoryByUserIdAsyncNOTEXPIRED(string Userid)
        {
            try
            {
                _logger.LogInformation("Error at FindingStory");
                return await _context.Set<Story>()
                    .Where(p => p.UserId == Userid && p.ExpireAt >= DateTime.UtcNow && p.Status == ContentStatus.Published)
                    .Include(p => p.User)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Occured while Fetching the Story");
                throw new NotImplementedException();
            }
        }

        public async Task<Story> UpdateStoryAsync(Story story)
        {
            try
            {
                _logger.LogInformation("Udating the Story");
                _context.Set<Story>().Update(story);
                await _context.SaveChangesAsync();
                return story;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex,"Error Occured while Udating the Story");
                throw new NotImplementedException();
            }
        }
    }
}
