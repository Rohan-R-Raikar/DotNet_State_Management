using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleNo.Entity
{
    public abstract class Content
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        [Required, MaxLength(1000)]
        public string Description { get; set; }

        public DateTime? PublishAt { get; set; }

        public ContentStatus Status { get; set; } = ContentStatus.Draft;

        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }

    public enum ContentStatus
    {
        Draft = 0,
        Scheduled = 1,
        Published = 2,
        Archived = 3
    }

}
