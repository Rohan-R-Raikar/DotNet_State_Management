using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis;
using ShopingCartStateManagement.Hubs;
using ShopingCartStateManagement.Models;
using ShopingCartStateManagement.Services;

[Authorize]
public class CartController : Controller
{
    private readonly CartService _cartService;
    private readonly IHubContext<CartHub> _hubContext;
    private readonly ProductService _productService;
    private readonly UserManager<IdentityUser> _userManager;
    private static readonly Dictionary<string, List<CartItem>> _userCarts = new();

    public CartController(CartService cartService, ProductService productService, IHubContext<CartHub> hubContext, UserManager<IdentityUser> userManager)
    {
        _productService = productService;
        _cartService = cartService;
        _hubContext = hubContext;
        _userManager = userManager;
    }

    private string GetCurrentUserId() => _userManager.GetUserId(User);
    private List<CartItem> GetCart(string userId)
    {
        if (!_userCarts.ContainsKey(userId))
        {
            _userCarts[userId] = new List<CartItem>();
        }
        return _userCarts[userId];
    }
    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var cart = await _cartService.GetCartAsync(userId);
        var products = await _productService.GetAllProductsAsync();

        ViewBag.UserId = userId;
        ViewBag.Products = products;
        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> AddItem(int productId)
    {
        var userId = _userManager.GetUserId(User);
        var product = await _productService.GetProductByIdAsync(productId);

        if (product == null || product.Stock <= 0)
        {
            TempData["Error"] = $"{product?.Name ?? "Product"} is out of stock!";
            return RedirectToAction("Index");
        }

        await _cartService.AddItemAsync(userId, product.Name);

        //await _hubContext.Clients.All.SendAsync("StockUpdated", product.Name);
        await _hubContext.Clients.All.SendAsync("StockUpdated", new { productName = product.Name, stock = product.Stock });

        return RedirectToAction("Index");
    }


    [HttpPost]
    public async Task<IActionResult> RemoveItem(int Id, string productName)
    {
        var userId = _userManager.GetUserId(User);
        var product = await _productService.GetProductByNameAsync(productName);

        await _cartService.RemoveItemAsync(userId, productName, Id);

        //await _hubContext.Clients.All.SendAsync("StockUpdated", productName);
        await _hubContext.Clients.All.SendAsync("StockUpdated", new { productName = product.Name, stock = product.Stock });

        return RedirectToAction("Index");
    }
}
