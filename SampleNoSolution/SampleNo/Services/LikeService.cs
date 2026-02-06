using AutoMapper;
using SampleNo.Entity;
using SampleNo.Models;
using SampleNo.Repositories.IRepositories;
using SampleNo.Services.IServices;

namespace SampleNo.Services
{
    public class LikeService : ILikeService
    {
        private readonly ILogger<LikeService> _logger;
        private readonly ILikeRepository _likeRepository;
        private readonly IMapper _mapper;
        public LikeService(IMapper mapper, ILikeRepository likeRepository, ILogger<LikeService> logger)
        {
            _likeRepository = likeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LikeDto> AddLikeAsync(LikeDto likeDto)
        {
            try
            {
                _logger.LogInformation("Log Message here");
                var data = _mapper.Map<Like>(likeDto);
                var newdata = await _likeRepository.AddLikeAsync(data);
                return _mapper.Map<LikeDto>(newdata);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<int> CountLikesAsync(string targetId, LikeTargetType targetType)
        {
            try
            {
                _logger.LogInformation("Log Message here");
                 var data = await _likeRepository.CountLikesAsync(targetId, targetType);
                return _mapper.Map<int>(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<LikeDto>> GetLikesByTargetAsync(string targetId, LikeTargetType targetType)
        {
            try
            {
                _logger.LogInformation("Log Message here");
                var data = await _likeRepository.GetLikesByTargetAsync(targetId, targetType);

                return _mapper.Map<IEnumerable<LikeDto>>(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<LikeDto>> GetLikesByUserAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Log Message here");
                var data = await _likeRepository.GetLikesByUserAsync(userId);
                return _mapper.Map<IEnumerable<LikeDto>>(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<bool> IsLikedAsync(string userId, string targetId, LikeTargetType targetType)
        {
            try
            {
                _logger.LogInformation("Log Message here");
                var data = await _likeRepository.IsLikedAsync(userId, targetId, targetType);
                return _mapper.Map<bool>(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<bool> RemoveLikeAsync(string userId, string targetId, LikeTargetType targetType)
        {
            try
            {
                _logger.LogInformation("Log Message here");
                var data = await _likeRepository.RemoveLikeAsync(userId, targetId, targetType);
                return _mapper.Map<bool>(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
