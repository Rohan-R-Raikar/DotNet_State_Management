using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleNo.Entity;
using SampleNo.Models;
using SampleNo.Services.IServices;
using System.Security.Claims;

namespace SampleNo.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProfileApiController : ControllerBase
    {
        private readonly IFollowService _followService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IPostService _postService;
        private readonly IStoryService _storyService;
        private readonly ILikeService _likeService;
        private readonly ILogger<ProfileApiController> _logger;

        public ProfileApiController(
            ILogger<ProfileApiController> logger,
            IApplicationUserService userService,
            IFollowService followService,
            IPostService postService,
            IStoryService storyService,
            ILikeService likeService)
        {
            _applicationUserService = userService;
            _followService = followService;
            _postService = postService;
            _storyService = storyService;
            _likeService = likeService;
            _logger = logger;
        }

        [HttpGet("my-stories")]
        public async Task<IActionResult> GetMyStories()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var stories = await _storyService.GetStoryByUserIdAsync(userId);
            return Ok(stories);
        }

        [HttpGet("my-posts")]
        public async Task<IActionResult> GetMyPosts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var posts = await _postService.GetPostByUserIdAsync(userId);
            return Ok(posts);
        }

        [HttpPost("add-post")]
        public async Task<IActionResult> AddPost([FromBody] PostDto postDto)
        {
            try
            {
                    postDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (postDto.UserId == null) return Unauthorized();
                    await _postService.AddPostAsync(postDto);
                    return Ok("Post added");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding post");
                return StatusCode(500);
            }
        }

        [HttpPost("add-story")]
        public async Task<IActionResult> AddStory([FromBody] StoryDto storyDto)
        {
            try
            {
                    storyDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (storyDto.UserId == null) return Unauthorized();
                    storyDto.ExpireAt = DateTime.UtcNow.AddHours(24);
                    await _storyService.AddStoryAsync(storyDto);
                    return Ok("Story added");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding story");
                return StatusCode(500);
            }
        }

        [HttpPut("update-post-inline")]
        public async Task<IActionResult> UpdatePostInline(int id, [FromQuery] string title, [FromQuery] string description, [FromQuery] string status)
        {
            try
            {
                    var post = await _postService.GetPostByIdAsync(id);
                    if (post == null) return NotFound();

                    post.Title = title;
                    post.Description = description;
                    post.Status = Enum.Parse<ContentStatus>(status);

                    await _postService.UpdatePostAsync(post);
                    return Ok("Post updated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating post");
                return StatusCode(500);
            }
        }

        [HttpDelete("delete-post/{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                await _postService.DeletePostAsync(id);
                return Ok("Post deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting post");
                return StatusCode(500);
            }
        }

        [HttpPut("update-story-inline")]
        public async Task<IActionResult> UpdateStoryInline([FromBody] StoryDto storyDto)
        {
            try
            {
                    storyDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (storyDto.UserId == null) return Unauthorized();
                    await _storyService.UpdateStoryAsync(storyDto);
                    return Ok("Story updated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating story");
                return StatusCode(500);
            }
        }

        [HttpDelete("delete-story/{id}")]
        public async Task<IActionResult> DeleteStory(int id)
        {
            try
            {
                await _storyService.DeleteStoryAsync(id);
                return Ok("Story deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting story");
                return StatusCode(500);
            }
        }
    }
}
