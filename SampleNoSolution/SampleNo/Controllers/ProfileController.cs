using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleNo.Entity;
using SampleNo.Models;
using SampleNo.Services.IServices;
using System.Security.Claims;

namespace SampleNo.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IFollowService _followService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IPostService _postService;
        private readonly IStoryService _storyService;
        private readonly ILikeService _likeService;
        private readonly ILogger<DashboardController> _logger;
        public ProfileController(
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
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> MyStories()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction("Index");
            var stories = await _storyService.GetStoryByUserIdAsync(userId);
            return View(stories);
        }

        public async Task<IActionResult> MyPosts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var posts = await _postService.GetPostByUserIdAsync(userId);
            return View(posts);
        }
        [HttpGet]
        public IActionResult AddPost()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddStory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(PostDto postDto)
        {
            try
            {
                    postDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (postDto.UserId == null) return RedirectToAction("Index");
                    await _postService.AddPostAsync(postDto);
                    return RedirectToAction("Index");
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error at Controller");
                return View();
            }
        }

            [HttpPost]
        public async Task<IActionResult> AddStory(StoryDto storyDto)
        {
            try
            {
                    storyDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if(storyDto.UserId == null) return RedirectToAction("Index");
                    storyDto.ExpireAt = DateTime.UtcNow.AddHours(24);
                    await _storyService.AddStoryAsync(storyDto);
                    return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error at Controller");
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePostInline(int id, string title, string description, string status)
        {
            try
            {
                    var post = await _postService.GetPostByIdAsync(id);
                    if (post == null) return NotFound();

                    post.Title = title;
                    post.Description = description;
                    post.Status = Enum.Parse<ContentStatus>(status);

                    await _postService.UpdatePostAsync(post);
                    return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error at Controller");
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                await _postService.DeletePostAsync(id);
                return RedirectToAction("MyPosts");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error at Controller");
                return View();
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateStoryInline(StoryDto storyDto)
        {
            try
            {
                    storyDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (storyDto.UserId == null) return RedirectToAction("Index");
                    await _storyService.UpdateStoryAsync(storyDto);
                    return RedirectToAction("MyStories");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error at Controller");
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStory(int id)
        {
            try {

                    await _storyService.DeleteStoryAsync(id);
                    return RedirectToAction("MyStories");
                }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error at Controller");
                return View();
            }
        }
    }
}
