using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleNo.Entity
{
    public class ApplicationUser:IdentityUser
    {
        public bool IsDeleted {  get; set; } = false;
        
        public DateTime? DeletedAt { get; set; }
        
        public String? DeletedBy { get; set; }
        
        public MemberShip_Plans Membership { get; set; }

        [InverseProperty("Followee")]
        public ICollection<Follow> Followers { get; set; } = new List<Follow>();


        [InverseProperty("Follwer")]
        public ICollection<Follow> Following { get; set; } = new List<Follow>();
        
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Content> Contents { get; set; } = new List<Content>();
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Story> Stories { get; set; } = new List<Story>();
        [InverseProperty(nameof(Message.Sender))]
        public ICollection<Message> MessagesSent { get; set; } = new List<Message>();
        [InverseProperty(nameof(Message.Receiver))]
        public ICollection<Message> MessagesReceived { get; set; } = new List<Message>();
    }
    public enum MemberShip_Plans
    {
        Normal = 0,
        Silver = 1,
        Gold = 2,
        Platinum = 3,
        Diamond = 4
    }
}
