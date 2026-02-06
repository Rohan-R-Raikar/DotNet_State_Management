using SampleNo.Entity;

namespace SampleNo.Models
{
    public class PostViewModel
    {
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string PostBody { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public ContentStatus Status { get; set; }
        public string? Comment { get; set; }
        public bool IsCommentEnabled { get; set; }
        public bool IsShareable { get; set; }
        public int LikeCount { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
        public bool IsFollowing { get; set; }
    }
}
