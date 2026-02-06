using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ShopingCartStateManagement.Data;
using ShopingCartStateManagement.Models;

namespace ShopingCartStateManagement.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }
        public async Task<Product> GetProductByNameAsync(string productName)
        {
            return await _context.Products.Where(p => p.Name == productName).FirstOrDefaultAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

    }
}
