using SampleNo.Models;

namespace SampleNo.Services.IServices
{
    public interface IApplicationUserService
    {
        Task<IEnumerable<UserDto>> GetAllUserAsync();
        Task<IEnumerable<UserDto>> GetActiveUserAsync();
        Task<IEnumerable<UserDto>> GetDeletedUserAsync();
        Task<IEnumerable<UserDto>> GetByExcludeIdAsync(Guid id);
        Task<UserDto> GetByIdAsync(Guid id);
        Task<UserDto> UpdateUserAsync(UpdateUserDto updateUserDto);
        Task<UserDto> DeleteUserAsync(Guid id, string deletedBy);
        Task<UserDto> RestoreUserAsync(Guid id);
    }
}
