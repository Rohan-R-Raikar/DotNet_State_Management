using SampleNo.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleNo.Models
{
    public class MessageDto
    {
        public int Id { get; set; }

        public string SenderId { get; set; }

        public string ReceiverId { get; set; }
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
