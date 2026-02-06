using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleNo.Entity
{
    public class Post : Content
    {
        [MaxLength(200)]
        public string? Comment { get; set; }

        public bool IsCommentEnabled { get; set; } = true;

        public bool IsShareable { get; set; } = true;
    }
}
