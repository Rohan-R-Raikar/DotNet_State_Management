using AutoMapper;
using SampleNo.Models;
using SampleNo.Entity;
using SampleNo.Repositories.IRepositories;
using SampleNo.Services.IServices;

namespace SampleNo.Services
{
    public class PostService: IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PostService> _logger;
        public PostService(ILogger<PostService> logger,
            IMapper mapper,
            IPostRepository postRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _postRepository = postRepository;
        }

        public async Task<PostDto> AddPostAsync(PostDto postDto)
        {
            try
            {
                _logger.LogInformation("Adding Post At Service Layer");
                var PostEntity = _mapper.Map<Post>(postDto);
                var AddedEntity = await _postRepository.AddPostAsync(PostEntity);
                return _mapper.Map<PostDto>(AddedEntity);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error Occured while Adding Post At Service Layer");
                throw new NotImplementedException();
            }
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting Post At Service Layer");
                var Post = await _postRepository.GetPostByIdAsync(id);
                if (Post == null) throw new Exception("User Not Found");

                return await _postRepository.DeletePostAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Occured while Deleting Post At Service Layer");
                throw new NotImplementedException();
            }
        }

        public async Task<IEnumerable<PostDto>> GetAllPostsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching ALl Post At Service Layer");
                var posts = await _postRepository.GetAllPostsAsync();

                return _mapper.Map<IEnumerable<PostDto>>(posts);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error Occured while Fetching ALl Post At Service Layer");
                throw new NotImplementedException();
            }
        }

        public async Task<IEnumerable<PostDto>> GetPostByUserIdAsync(string Userid)
        {
            try
            {
                _logger.LogInformation("Fetching ALl Post At Service Layer");
                var posts = await _postRepository.GetPostByUserIdAsync(Userid);

                return _mapper.Map<IEnumerable<PostDto>>(posts);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error Occured while Fetching ALl Post At Service Layer");
                throw new NotImplementedException();
            }
        }

        public async Task<PostDto?> GetPostByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching Post At Service Layer");
                var post = await _postRepository.GetPostByIdAsync(id);
                if (post == null) throw new Exception("Post Not Found");

                return _mapper.Map<PostDto>(post);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error Occured while Fetching Post At Service Layer");
                throw new NotImplementedException();
            }            
        }

        public async Task<PostDto> UpdatePostAsync(PostDto postDto)
        {
            try
            {
                var postExists = await _postRepository.GetPostByIdAsync(postDto.Id);
                if (postExists == null) throw new Exception("Post Not Found");

                var mappedPost = _mapper.Map(postDto, postExists);

                var UpdatedPost = await _postRepository.UpdatePostAsync(postExists);

                return _mapper.Map<PostDto>(UpdatedPost);
            }
            catch (Exception ex) 
            {
                _logger.LogError("Error Occured at Service level While Updating Post");
                throw new NotImplementedException();
            }            
        }
    }
}
