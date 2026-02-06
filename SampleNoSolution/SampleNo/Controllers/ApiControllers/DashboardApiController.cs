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
    public class DashboardApiController : ControllerBase
    {
        private readonly IFollowService _followService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IPostService _postService;
        private readonly IStoryService _storyService;
        private readonly ILikeService _likeService;
        private readonly ILogger<DashboardApiController> _logger;

        public DashboardApiController(
            ILogger<DashboardApiController> logger,
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

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard([FromQuery] string searchTerm = "")
        {
            try
            {
                var currrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (currrentUserId == null) return Unauthorized();

                var followings = await _followService.GetFollowingAsync(currrentUserId);
                var followedUserIds = followings.Select(f => f.FolloweeId).ToList();

                var allPosts = new List<PostViewModel>();
                foreach (var userId in followedUserIds)
                {
                    var posts = await _postService.GetPostByUserIdAsync(userId.ToString());
                    if (posts != null)
                    {
                        foreach (var post in posts)
                        {
                            allPosts.Add(new PostViewModel
                            {
                                PostId = post.Id,
                                PostTitle = post.Title,
                                PostBody = post.Description,
                                UserId = post.UserId,
                                UserName = post.UserId,
                                Status = post.Status,
                                Comment = post.Comment,
                                IsCommentEnabled = post.IsCommentEnabled,
                                IsShareable = post.IsShareable,
                                IsFollowing = true,
                                LikeCount = await _likeService.CountLikesAsync(post.Id.ToString(), LikeTargetType.Post),
                                IsLikedByCurrentUser = await _likeService.IsLikedAsync(currrentUserId, post.Id.ToString(), LikeTargetType.Post)
                            });
                        }
                    }
                }

                var allStories = new List<StoryViewModel>();
                foreach (var userId in followedUserIds)
                {
                    var stories = await _storyService.GetStoryByUserIdAsyncNOTEXPIRED(userId);
                    if (stories != null)
                    {
                        foreach (var story in stories)
                        {
                            allStories.Add(new StoryViewModel
                            {
                                StoryId = story.Id,
                                StoryTitle = story.Title,
                                StoryBody = story.Description,
                                UserId = story.UserId,
                                UserName = story.UserId,
                                Status = story.Status,
                                ExpiresAt = story.ExpireAt ?? DateTime.UtcNow,
                                IsFollowing = true,
                                LikeCount = await _likeService.CountLikesAsync(story.Id.ToString(), LikeTargetType.Story),
                                IsLikedByCurrentUser = await _likeService.IsLikedAsync(currrentUserId, story.Id.ToString(), LikeTargetType.Story)
                            });
                        }
                    }
                }

                List<UserSearchViewModel> searchedUsers = new();
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    var allUsers = await _applicationUserService.GetAllUserAsync();
                    searchedUsers = allUsers
                        .Where(u => u.Id.ToString() != currrentUserId &&
                                    u.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                        .Select(u => new UserSearchViewModel
                        {
                            UserId = u.Id.ToString(),
                            Email = u.Email,
                            IsFollowing = followedUserIds.Contains(u.Id.ToString())
                        })
                        .ToList();
                }

                var dashboard = new DashboardViewModel
                {
                    CurrentUserId = currrentUserId,
                    FollowedUserPost = allPosts,
                    FollowedUserStory = allStories,
                    SearchedUsers = searchedUsers
                };

                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Dashboard");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("toggle-follow")]
        public async Task<IActionResult> ToggleFollow([FromBody] string targetUserId)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (currentUserId == null || targetUserId == currentUserId) return BadRequest();

                var isFollowing = await _followService.IsfollowUserAsync(currentUserId, targetUserId);

                if (isFollowing)
                    await _followService.UnfollowUserAsync(currentUserId, targetUserId);
                else
                    await _followService.FollowUserAsync(currentUserId, targetUserId);

                return Ok("Follow status updated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling follow status");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("toggle-like")]
        public async Task<IActionResult> ToggleLike([FromBody] LikeDto likeDto)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(currentUserId) || string.IsNullOrWhiteSpace(likeDto.TargetId))
                    return BadRequest();

                var isLiked = await _likeService.IsLikedAsync(currentUserId, likeDto.TargetId, likeDto.TargetType);

                if (isLiked)
                    await _likeService.RemoveLikeAsync(currentUserId, likeDto.TargetId, likeDto.TargetType);
                else
                {
                    likeDto.UserId = currentUserId;
                    await _likeService.AddLikeAsync(likeDto);
                }

                return Ok("Like status updated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling like");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}