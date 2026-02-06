namespace SampleNo.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
    }
}
