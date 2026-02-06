using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SampleNo.Data;
using SampleNo.Entity;
using SampleNo.Repositories.IRepositories;
using System.Security.Claims;

namespace SampleNo.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ApplicationUserRepository> _logger;
        public ApplicationUserRepository(ApplicationDbContext dbContext
            , ILogger<ApplicationUserRepository> logger,
HttpContextAccessor httpContextAccessor)
        {
            _context = dbContext;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        private string CurrentUser => _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == "user_id")?.Value?? 
            _httpContextAccessor.HttpContext?.User
            .FindFirstValue(ClaimTypes.NameIdentifier);

        private async Task<ApplicationUser?> FindUserByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id.ToString());
        }

        //public async Task<ApplicationUser> GetCurrentUser()
        //{

        //}

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all users");
                return await _context.Users.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all users => GetAllUserAsync => ApplicationUserRepository");
                throw;
            }
        }
        public async Task<ApplicationUser> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching User Data using UserId");
                var user = await FindUserByIdAsync(id);
                
                if (user == null)
                    throw new KeyNotFoundException("User Not Found");

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching user using ID => GetByIdAsync => ApplicationUserRepository");
                throw;
            }
        }
        public async Task<IEnumerable<ApplicationUser>> GetByExcludeIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching User Data using UserId");
                return await _context.Users.Where(u => u.Id != id.ToString()).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching user using ID => GetByIdAsync => ApplicationUserRepository");
                throw;
            }
        }
        public async Task<ApplicationUser> UpdateUserAsync(ApplicationUser user)
        {
            try
            {
                _logger.LogInformation("Updating User Data");
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while fetching user using ID => UpdateUserAsync => ApplicationUserRepository");
                throw;
            }                
        }

        public async Task<ApplicationUser> SoftDeleteUserAsync(Guid id, string deletedBy)
        {
            try
            {
                _logger.LogInformation("Soft Deleting User");
                var usr = await FindUserByIdAsync(id);
                if (usr == null)
                {
                    throw new Exception("User Not Found"); 
                }
                usr.IsDeleted = true;
                usr.DeletedAt = DateTime.UtcNow;
                usr.DeletedBy = deletedBy;
                
                await _context.SaveChangesAsync();
                
                return usr;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while fetching user using ID => SoftDeleteUserAsync => ApplicationUserRepository");
                throw;
            }
        }

        public async Task<ApplicationUser> RestoreUserAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Restoring User");
                var usr = await FindUserByIdAsync(id);
                if (usr == null)
                {
                    throw new Exception("User Not Found");
                }
                usr.IsDeleted = false;
                usr.DeletedAt = null;
                usr.DeletedBy = null;

                await _context.SaveChangesAsync();

                return usr;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching user using ID => RestoreUserAsync => ApplicationUserRepository");
                throw;
            }
        }

        public async Task<IEnumerable<ApplicationUser>> GetActiveUsersAsync()
        {
            try
            {
                _logger.LogInformation("Fetching All Active Users");
                return await _context.Users.AsNoTracking().Where(u => !u.IsDeleted).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching user using ID => GetActiveUsersAsync => ApplicationUserRepository");
                throw;
            }
        }

        public async Task<IEnumerable<ApplicationUser>> GetDeletedUsersAsync()
        {
            try
            {
                _logger.LogInformation("Fetching All Deleted Users");
                return await _context.Users.AsNoTracking().Where(u => u.IsDeleted).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching user using ID => GetDeletedUsersAsync => ApplicationUserRepository");
                throw;
            }
        }
    }

}