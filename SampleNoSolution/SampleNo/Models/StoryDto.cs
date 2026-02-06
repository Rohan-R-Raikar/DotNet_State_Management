using SampleNo.Entity;

namespace SampleNo.Models
{
    public class StoryDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ContentStatus Status { get; set; }
        public DateTime? PublishAt { get; set; }
        public DateTime? ExpireAt { get; set; }
    }
}
