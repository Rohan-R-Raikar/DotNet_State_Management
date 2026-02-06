using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using SampleNo.Entity;
using SampleNo.Models;
using SampleNo.Services.IServices;
using System.Security.Claims;

namespace SampleNo.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IFollowService _followService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IPostService _postService;
        private readonly IStoryService _storyService;
        private readonly ILikeService _likeService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            ILogger<DashboardController> logger,
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

        public async Task<IActionResult> Index(string searchTerm = "")
        {
            try
            {
                _logger.LogInformation("Loading Dashboard");
                var currrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (currrentUserId == null) return NotFound();

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

                return View(dashboard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Dashboard");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFollow(string targetUserId)
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

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Error Occured at Controller");
                return View();
            }            
        }

        [HttpPost]
        public async Task<IActionResult> ToggleLike(string targetId, LikeTargetType targetType)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(currentUserId) || string.IsNullOrWhiteSpace(targetId))
                    return BadRequest();

                var isLiked = await _likeService.IsLikedAsync(currentUserId, targetId, targetType);

                if (isLiked)
                    await _likeService.RemoveLikeAsync(currentUserId, targetId, targetType);
                else
                {
                    var likeDto = new LikeDto
                    {
                        UserId = currentUserId,
                        TargetId = targetId,
                        TargetType = targetType
                    };
                    await _likeService.AddLikeAsync(likeDto);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error at Controller");
                return View();
            }
        }
    }
}
