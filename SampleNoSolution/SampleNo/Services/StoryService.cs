using AutoMapper;
using SampleNo.Data;
using SampleNo.Entity;
using SampleNo.Models;
using SampleNo.Repositories.IRepositories;
using SampleNo.Services.IServices;

namespace SampleNo.Services
{
    public class StoryService : IStoryService
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<StoryService> _logger;
        public StoryService(ILogger<StoryService> logger,
            IMapper mapper,
            IStoryRepository storyRepository)
        {
            _storyRepository = storyRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<StoryDto> AddStoryAsync(StoryDto storydto)
        {
            try
            {
                _logger.LogInformation("Adding Story at Service Level");
                var storyEntity = _mapper.Map<Story>(storydto);
                var AddedStory = await _storyRepository.AddStoryAsync(storyEntity);
                return _mapper.Map<StoryDto>(AddedStory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured at Adding Story at Service Level");
                throw new NotImplementedException();
            }
        }

        public async Task<bool> DeleteStoryAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting Story at Service Level");
                var story = await _storyRepository.GetStoryByIdAsync(id);
                if (story == null) throw new Exception("Story not found");

                return await _storyRepository.DeleteStoryAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured at Deleting Story at Service Level");
                throw new NotImplementedException();
            }
        }

        public async Task<IEnumerable<StoryDto>> GetAllStoriesAsync()
        {
            try
            {
                _logger.LogInformation("Fetching All Stories at Service Level");
                var stories = await _storyRepository.GetAllStoriesAsync();

                return _mapper.Map<IEnumerable<StoryDto>>(stories);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error occured at Fetching All Stories at Service Level");
                throw new NotImplementedException();
            }           
        }
        public async Task<IEnumerable<StoryDto>> GetStoryByUserIdAsync(string Userid)
        {
            try
            {
                _logger.LogInformation("Fetching All Stories at Service Level");
                var stories = await _storyRepository.GetStoryByUserIdAsync(Userid);

                return _mapper.Map<IEnumerable<StoryDto>>(stories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured at Fetching All Stories at Service Level");
                throw new NotImplementedException();
            }
        }

        public async Task<IEnumerable<StoryDto>> GetStoryByUserIdAsyncNOTEXPIRED(string Userid)
        {
            try
            {
                _logger.LogInformation("Fetching All Stories at Service Level");
                var stories = await _storyRepository.GetStoryByUserIdAsyncNOTEXPIRED(Userid);

                return _mapper.Map<IEnumerable<StoryDto>>(stories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured at Fetching All Stories at Service Level");
                throw new NotImplementedException();
            }
        }

        public async Task<StoryDto?> GetStoryByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching Story at Service Level");
                var story = await _storyRepository.GetStoryByIdAsync(id);
                if (story == null) throw new Exception("Story Not Found");

                return _mapper.Map<StoryDto>(story);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error occured at Fetching Story at Service Level");
                throw new NotImplementedException();
            }
        }

        public async Task<StoryDto> UpdateStoryAsync(StoryDto storydto)
        {
            try
            {
                _logger.LogInformation("Upadating Story at Service Level");
                var story = await _storyRepository.GetStoryByIdAsync(storydto.Id);
                if (story == null) throw new Exception("Story Not Found");
                
                _mapper.Map(storydto, story);
                var UpdatedStory =await _storyRepository.UpdateStoryAsync(story);
                return _mapper.Map<StoryDto>(UpdatedStory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured at Upadating Story at Service Level");
                throw new NotImplementedException();
            }
        }
    }
}
