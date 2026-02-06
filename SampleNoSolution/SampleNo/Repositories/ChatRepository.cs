using Microsoft.EntityFrameworkCore;
using SampleNo.Data;
using SampleNo.Entity;
using SampleNo.Repositories.IRepositories;

namespace SampleNo.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ILogger<ChatRepository> _logger;
        private readonly ApplicationDbContext _context;
        public ChatRepository(ILogger<ChatRepository> logger, ApplicationDbContext dbContext)
        {
            _context = dbContext;
            _logger = logger;   
        }

        public async Task<List<Message>> GetMessagesBetweenUsersAsync(string userId1, string userId2)
        {
            return await _context.Messages
                .Where(m => (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                            (m.SenderId == userId2 && m.ReceiverId == userId1))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<String> GetLastMessageTextAsync(string userId1, string userId2)
        {
            return await _context.Messages
                .Where(m => (m.SenderId == userId1 || m.SenderId == userId2) &&
                            (m.ReceiverId == userId1 || m.ReceiverId == userId2))
                .OrderByDescending(m => m.Id)
                .Select(m => m.Text)
                .FirstOrDefaultAsync();
        }
    }
}
