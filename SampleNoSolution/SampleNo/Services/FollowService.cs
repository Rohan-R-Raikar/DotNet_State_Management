using AutoMapper;
using Microsoft.Extensions.Logging;
using SampleNo.Models;
using SampleNo.Repositories.IRepositories;
using SampleNo.Services.IServices;

namespace SampleNo.Services
{
    public class FollowService : IFollowService
    {
        private readonly IFollowRepository _followRepository;
        private readonly ILogger<FollowService> _logger;
        private readonly IMapper _mapper;
        public FollowService(IMapper mapper, IFollowRepository followRepository, ILogger<FollowService> logger)
        {
            _followRepository = followRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<FollowDto> FollowUserAsync(string followerId, string followeeId)
        {
            try
            {
                _logger.LogInformation("User At Follow Usr Async");
                var data = await _followRepository.FollowUserAsync(followerId, followeeId);
                return _mapper.Map<FollowDto>(data);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "error");
                throw new NotImplementedException();
            }
        }

        public async Task<IEnumerable<FollowDto>> GetFollowersAsync(string userId)
        {
            try
            {
                _logger.LogInformation("User At GetFollowersAsync");
                var data = await _followRepository.GetFollowersAsync(userId);
                return _mapper.Map<IEnumerable<FollowDto>>(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error");
                throw new NotImplementedException();
            }
        }

        public async Task<IEnumerable<FollowDto>> GetFollowingAsync(string userId)
        {
            try
            {
                _logger.LogInformation("User At GetFollowingAsync");
                var data = await _followRepository.GetFollowingAsync(userId);
                return _mapper.Map<IEnumerable<FollowDto>>(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
                throw new NotImplementedException();
            }
        }

        public async Task<bool> IsfollowUserAsync(string followerId, string followeeId)
        {
            try
            {
                _logger.LogInformation("User at IsFollowUserAsync");
                var data = await _followRepository.IsfollowUserAsync(followerId, followeeId);
                return _mapper.Map<bool>(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
                throw new NotImplementedException();
            }
        }

        public async Task<bool> UnfollowUserAsync(string followerId, string followeeId)
        {
            try
            {
                _logger.LogInformation("User at UnfollowUserAsync");
                var data =  await _followRepository.UnfollowUserAsync(followerId, followeeId);
                return _mapper.Map<bool>(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
                throw new NotImplementedException();
            }
        }
    }
}
