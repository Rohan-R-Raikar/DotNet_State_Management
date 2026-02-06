namespace ShopingCartStateManagement.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string UserId { get; set; } = default!;
        public string ProductName { get; set; } = default!;
        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
