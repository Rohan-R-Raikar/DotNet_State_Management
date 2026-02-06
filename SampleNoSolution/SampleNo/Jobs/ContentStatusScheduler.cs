using SampleNo.Repositories.IRepositories;

namespace SampleNo.Jobs
{
    public class ContentStatusScheduler
    {
        private readonly ILogger<ContentStatusScheduler> _logger;
        private readonly IPostRepository _postRepo;
        private readonly IStoryRepository _storyRepo;

        public ContentStatusScheduler(ILogger<ContentStatusScheduler> logger, IPostRepository postRepo,IStoryRepository storyRepo)
        {
            _logger = logger;
            _postRepo = postRepo;
            _storyRepo = storyRepo;
        }
        public async Task RunManualCheckAsync()
        {
            _logger.LogInformation("Hangfire job started.");
            await UpdatePostStatusesAsync();
            await UpdateStoryStatusesAsync();
            _logger.LogInformation("Hangfire job finished.");
        }

        private async Task UpdatePostStatusesAsync()
        {
            var posts = await _postRepo.GetScheduledPostsOnly();
            foreach (var post in posts)
            {
                bool updated = false;

                //if (post.PublishAt == null && post.Status != Entity.ContentStatus.Published)
                //{
                //    post.Status = Entity.ContentStatus.Published;
                //    updated = true;
                //}
                if (post.PublishAt.HasValue && post.PublishAt.Value >= DateTime.UtcNow && post.Status == Entity.ContentStatus.Scheduled)
                {
                    post.Status = Entity.ContentStatus.Published;
                    updated = true;
                }

                if (updated)
                {
                    await _postRepo.UpdatePostAsync(post);
                    _logger.LogInformation($"Post #{post.Id} updated to {post.Status}");
                }
            }
        }

        private async Task UpdateStoryStatusesAsync()
        {
            var stories = await _storyRepo.GetScheduledStories();
            foreach (var story in stories)
            {
                bool updated = false;

                if (story.PublishAt == null && story.Status != Entity.ContentStatus.Published)
                {
                    story.Status = Entity.ContentStatus.Published;
                    updated = true;
                }

                if (story.PublishAt.HasValue && story.PublishAt.Value >= DateTime.UtcNow && story.Status == Entity.ContentStatus.Scheduled)
                {
                    story.Status = Entity.ContentStatus.Published;
                    updated = true;
                }

                if (story.ExpireAt.HasValue && story.ExpireAt.Value <= DateTime.UtcNow && story.Status != Entity.ContentStatus.Archived)
                {
                    story.Status = Entity.ContentStatus.Archived;
                    updated = true;
                }

                if (updated)
                {
                    await _storyRepo.UpdateStoryAsync(story);
                    _logger.LogInformation($"Story #{story.Id} updated to {story.Status}");
                }
            }
        }
    }
}
