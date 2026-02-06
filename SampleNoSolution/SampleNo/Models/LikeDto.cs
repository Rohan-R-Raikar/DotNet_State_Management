using SampleNo.Entity;

namespace SampleNo.Models
{
    public class LikeDto
    {
        public int LikeId { get; set; }
        public string UserId { get; set; }
        public String TargetId { get; set; }
        public LikeTargetType TargetType { get; set; }
        public DateTime? LikedAt { get; set; } = DateTime.Now;
    }
}
