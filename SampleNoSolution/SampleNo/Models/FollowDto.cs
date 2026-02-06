using SampleNo.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleNo.Models
{
    public class FollowDto
    {
        public int FollowId { get; set; }
        public string FollowerId { get; set; }
        public string FolloweeId { get; set; }
        public DateTime FollowedAt { get; set; }
    }
}
