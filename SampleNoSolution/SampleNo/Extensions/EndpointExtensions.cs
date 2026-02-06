using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SampleNo.Entity;
using SampleNo.Models;
using SampleNo.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SampleNo.Extensions
{
    public static class EndpointExtensions
    {
        public static void MapFollowEndpoints(this IEndpointRouteBuilder app)
        {

            app.MapPost("/api/auth/minimal-login", async (
            LoginRequestDto model,
            UserManager<ApplicationUser> userManager,
            IConfiguration config) =>
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                {
                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                    var authSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(config["Jwt:Key"])
                    );

                    var token = new JwtSecurityToken(
                        issuer: config["Jwt:Issuer"],
                        audience: config["Jwt:Audience"],
                        expires: DateTime.UtcNow.AddHours(1),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                    return Results.Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
                return Results.Unauthorized();
            });

            app.MapGet("/minimal-dashboard", [Authorize] async (
                HttpContext httpContext,
                IFollowService _followService ,
                IPostService _postService ,
                IStoryService _storyService ,
                ILikeService _likeService ,
                IApplicationUserService _appUserService ,
                ILogger<Program> _logger ,
                [FromQuery] string searchTern = "") =>
            {
                try
                {
                    var currentUserId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (currentUserId == null)
                        return Results.Unauthorized();

                    var followings = await _followService.GetFollowingAsync(currentUserId);
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
                                    IsLikedByCurrentUser = await _likeService.IsLikedAsync(currentUserId,userId.ToString(),LikeTargetType.Post)
                                });
                            }
                        }
                    }

                    var allStories = new List<StoryViewModel>();
                    foreach (var userId in followedUserIds)
                    {
                        var stories = await _storyService.GetStoryByUserIdAsyncNOTEXPIRED(userId.ToString());
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
                                    IsLikedByCurrentUser = await _likeService.IsLikedAsync(currentUserId, story.Id.ToString(), LikeTargetType.Story)
                                });
                            }
                        }
                    }

                    List<UserSearchViewModel> searchedUsers = new();
                    if (!string.IsNullOrWhiteSpace(searchTern))
                    {
                        var allUsers = await _appUserService.GetAllUserAsync();
                        searchedUsers = allUsers
                        .Where(u => u.Id.ToString() != currentUserId &&
                        u.Email.Contains(searchTern, StringComparison.OrdinalIgnoreCase))
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
                        CurrentUserId = currentUserId,
                        FollowedUserPost = allPosts,
                        FollowedUserStory = allStories,
                        SearchedUsers = searchedUsers
                    };
                    return Results.Ok(dashboard);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error loading Dashboard");
                    return Results.Problem("Internal server error", statusCode: 500);
                }
            });

            app.MapPost("/minimal-toggle-follow",[Authorize] async (
                HttpContext httpContext,
                [FromBody] string targetUserId,
                IFollowService followService,
                ILogger<Program> logger) =>
            {
                try
                {
                    var currentUserId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (string.IsNullOrWhiteSpace(currentUserId) || currentUserId == targetUserId)
                        return Results.BadRequest();

                    var isFollowing = await followService.IsfollowUserAsync(currentUserId, targetUserId);

                    if (isFollowing)
                        await followService.UnfollowUserAsync(currentUserId, targetUserId);
                    else
                        await followService.FollowUserAsync(currentUserId, targetUserId);

                    return Results.Ok("Follow status updated.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error toggling follow status");
                    return Results.Problem("Internal server error", statusCode: 500);
                }
            });

            app.MapPost("/minimal-toggle-like", [Authorize] async (
                HttpContext httpContext,
                [FromBodyAttribute] LikeDto likeDto,
                ILikeService _likeService,
                ILogger<Program> logger) =>
                {
                    try
                    {
                        var currentUserId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                        if (string.IsNullOrWhiteSpace(currentUserId) || string.IsNullOrWhiteSpace(likeDto.TargetId))
                            return Results.BadRequest();

                        var isLiked = await _likeService.IsLikedAsync(currentUserId, likeDto.TargetId, likeDto.TargetType);

                        if (isLiked)
                        {
                            await _likeService.RemoveLikeAsync(currentUserId,likeDto.TargetId, likeDto.TargetType);
                        }
                        else
                        {
                            likeDto.UserId = currentUserId;
                            await _likeService.AddLikeAsync(likeDto);
                        }
                        return Results.Ok("Like status updated");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error toggling like status");
                        return Results.Problem("Internal server error", statusCode: 500);
                    }
            });
        }
    }
}
