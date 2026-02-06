using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleNo.Entity
{
    public class Story : Content
    {
        public DateTime? ExpireAt { get; set; }
    }
}