using SampleNo.Entity;

namespace SampleNo.Models
{
    public class StoryViewModel
    {
        public int StoryId { get; set; }
        public string StoryTitle { get; set; }
        public string StoryBody { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public ContentStatus Status { get; set; }
        public DateTime ExpiresAt { get; set; }
        public int LikeCount { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
        public bool IsFollowing { get; set; }
    }
}
