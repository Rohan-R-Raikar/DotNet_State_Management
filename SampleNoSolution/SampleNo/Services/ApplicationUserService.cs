using AutoMapper;
using SampleNo.Models;
using SampleNo.Repositories.IRepositories;
using SampleNo.Services.IServices;

namespace SampleNo.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IApplicationUserRepository _appUsrRepo;
        private readonly ILogger<ApplicationUserService> _logger;
        private readonly IMapper _mapper;
        public ApplicationUserService(IApplicationUserRepository applicationUserRepository,
            IMapper mapper,
            ILogger<ApplicationUserService> logger)
        {
            _appUsrRepo = applicationUserRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<UserDto> DeleteUserAsync(Guid id, string deletedBy)
        {
            try
            {
                _logger.LogInformation("Deleting User in Service Method");
                var user = await _appUsrRepo.GetByIdAsync(id);
                if (user == null) throw new Exception("User not found");

                var deleteusr = await _appUsrRepo.SoftDeleteUserAsync(id, deletedBy);
                return _mapper.Map<UserDto>(deleteusr);
            }
            catch (Exception ex) 
            {
                _logger.LogError("Error Occured in Service Layer While Deleting the User");
                throw;
            }              
        }

        public async Task<IEnumerable<UserDto>> GetActiveUserAsync()
        {
            try
            {
                _logger.LogInformation("Getting All Users in ApplicationUserService");
                var users = await _appUsrRepo.GetActiveUsersAsync();
                return _mapper.Map<IEnumerable<UserDto>>(users);
            }
            catch (Exception ex) 
            {
                _logger.LogError("Error Occured in Service Layer While Getting ALL Active Users");
                throw;
            }
        }

        public async Task<IEnumerable<UserDto>> GetAllUserAsync()
        {
            try
            {
                _logger.LogInformation("Getting All Users in GetAllUserAsync");
                var users = await _appUsrRepo.GetAllUsersAsync();
                return _mapper.Map<IEnumerable<UserDto>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured in Service Layer While Getting ALL Users");
                throw;
            }
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Getting All Users in GetByIdAsync");
                var users = await _appUsrRepo.GetByIdAsync(id);
                return _mapper.Map<UserDto>(users);
            }
            catch (Exception ex) 
            {
                _logger.LogError("Error Occured in Service Layer While Getting User GetByIdAsync");
                throw;
            }
        }

        public async Task<IEnumerable<UserDto>> GetByExcludeIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Getting All Users in GetByIdAsync");
                var users = await _appUsrRepo.GetByExcludeIdAsync(id);
                return _mapper.Map<IEnumerable<UserDto>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured in Service Layer While Getting User GetByIdAsync");
                throw;
            }
        }

        public async Task<IEnumerable<UserDto>> GetDeletedUserAsync()
        {
            try
            {
                _logger.LogInformation("Getting All Deleted Users in GetDeletedUserAsync");
                var users = await _appUsrRepo.GetDeletedUsersAsync();
                return _mapper.Map<IEnumerable<UserDto>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured in Service Layer While Getting All Deleted Users in GetDeletedUserAsync");
                throw;
            }
        }

        public async Task<UserDto> RestoreUserAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Restoering Deleted at Services RestoreUserAsync");
                var user = await _appUsrRepo.RestoreUserAsync(id);
                return _mapper.Map<UserDto>(user);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error Occured in Service Layer While Restoring User at RestoreUserAsync");
                throw;
            }
        }

        public async Task<UserDto> UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            try
            {
                _logger.LogInformation("Updating User at services UpdateUserAsync");
                var user = await _appUsrRepo.GetByIdAsync(updateUserDto.Id);
                if (user == null) throw new Exception("User Not Found");

                _mapper.Map(updateUserDto, user);
                var updateUser = _appUsrRepo.UpdateUserAsync(user);

                return _mapper.Map<UserDto>(updateUser);
            }
            catch (Exception ex) 
            {
                _logger.LogError("Error Occured in Service Layer While Updating User at UpdateUserAsync");
                throw;
            }
        }
    }
}
