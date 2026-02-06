using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleNo.Entity
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        
        public string SenderId { get; set; }
        
        [ForeignKey(nameof(SenderId))]
        public ApplicationUser Sender { get; set; }

        public string ReceiverId { get; set; }
        
        [ForeignKey(nameof(ReceiverId))]
        public ApplicationUser Receiver { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Text { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;
        
        public bool IsDeletedBySender { get; set; } = false;
        
        public bool IsDeletedByReceiver { get; set; } = false;
        
        public MessageType Type { get; set; } = MessageType.Text;
        
        public enum MessageType
        {
            Text,
            Image,
            Video,
            File
        }
    }
}
