using SampleNo.Entity;

namespace SampleNo.Models
{
    public class PostDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Comment { get; set; }
        public bool IsCommentEnabled { get; set; }
        public bool IsShareable { get; set; }
        public DateTime? PublishAt { get; set; }
        public ContentStatus Status { get; set; }
    }
}
