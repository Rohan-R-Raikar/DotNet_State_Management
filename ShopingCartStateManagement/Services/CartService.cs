using Microsoft.EntityFrameworkCore;
using ShopingCartStateManagement.Data;
using ShopingCartStateManagement.Models;

namespace ShopingCartStateManagement.Services
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CartItem>> GetCartAsync(string userId)
        {
            return await _context.CartItems.Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task<bool> AddItemAsync(string userId, string productName)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == productName);
            if (product == null || product.Stock <= 0)
                return false;

            product.Stock -= 1;

            var item = new CartItem
            {
                UserId = userId,
                ProductName = productName,
                Quantity = 1
            };

            _context.CartItems.Add(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task RemoveItemAsync(string userId, string productName, int Id)
        {
            CartItem item = await _context.CartItems.FirstOrDefaultAsync(cartitem => cartitem.Id == Id && cartitem.UserId == userId && cartitem.ProductName == productName);
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == productName);

            if (item != null){ _context.CartItems.Remove(item);}
            product.Stock += 1;
            await _context.SaveChangesAsync();
        }
    }
}
