using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleNo.Entity
{
    public class Like
    {
        [Key]
        public int LikeId { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public String TargetId { get; set; }

        public LikeTargetType TargetType { get; set; }

        public DateTime LikedAt { get; set; } = DateTime.Now;
    }

    public enum LikeTargetType
    {
        Post = 1,
        Story = 2
    }
}
