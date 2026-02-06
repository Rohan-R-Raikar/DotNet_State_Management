namespace ProjectCRUD.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = "";
        public DateTime OrderDate { get; set; }
        public int TotalAmount { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
