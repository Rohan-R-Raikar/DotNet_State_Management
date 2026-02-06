using SampleNo.Entity;

namespace SampleNo.Repositories.IRepositories
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<IEnumerable<Post>> GetPostByUserIdAsync(string Userid);
        Task<IEnumerable<Post?>> GetScheduledPostsOnly();
        Task<Post?> GetPostByIdAsync(int id);
        Task<Post> AddPostAsync(Post post);
        Task<Post> UpdatePostAsync(Post post);
        Task<bool> DeletePostAsync(int id);
    }
}
