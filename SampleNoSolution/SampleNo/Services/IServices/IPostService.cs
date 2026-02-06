using SampleNo.Models;

namespace SampleNo.Services.IServices
{
    public interface IPostService
    {
        Task<IEnumerable<PostDto>> GetAllPostsAsync();
        Task<IEnumerable<PostDto>> GetPostByUserIdAsync(string Userid);
        Task<PostDto?> GetPostByIdAsync(int id);
        Task<PostDto> AddPostAsync(PostDto postDto);
        Task<PostDto> UpdatePostAsync(PostDto postDto);
        Task<bool> DeletePostAsync(int id);
    }
}
