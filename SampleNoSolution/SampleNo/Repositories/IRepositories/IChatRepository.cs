
using SampleNo.Entity;

namespace SampleNo.Repositories.IRepositories
{
    public interface IChatRepository
    {
        Task<List<Message>> GetMessagesBetweenUsersAsync(string userId1, string userId2);
        Task<String> GetLastMessageTextAsync(string userId1, string userId2);
    }
}
