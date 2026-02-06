using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleNo.Entity
{
    public class Follow
    {
        [Key]
        public int FollowId { get; set; }

        [Required]
        public string FollowerId { get; set; }

        [ForeignKey("FollowerId")]
        [InverseProperty("Following")]
        public ApplicationUser Follwer { get; set; }

        [Required]
        public string FolloweeId { get; set; }

        [ForeignKey("FolloweeId")]
        [InverseProperty("Followers")]
        public ApplicationUser Followee { get; set; }

        public DateTime FollowedAt { get; set; } = DateTime.UtcNow;
    }
}
