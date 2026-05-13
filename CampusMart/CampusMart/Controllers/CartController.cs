using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CampusMart.Data;
using CampusMart.Models.Entities;
using CampusMart.Models.ViewModels.User;

namespace CampusMart.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var cart = await _db.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            var model = new CartViewModel();
            if (cart != null && cart.CartItems != null)
            {
                model.Items = cart.CartItems.Select(ci => new CartItemViewModel
                {
                    CartItemId = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.Name ?? "Unknown",
                    ImageUrl = ci.Product?.ImageUrl,
                    Price = ci.Product?.Price ?? 0,
                    Quantity = ci.Quantity,
                    CategoryName = ci.Product?.Category?.Name ?? "Uncategorized"
                }).ToList();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var product = await _db.Products.FindAsync(productId);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return Redirect(Request.Headers["Referer"].ToString() ?? "/");
            }

            var cart = await _db.Carts.Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart == null)
            {
                cart = new Cart { UserId = user.Id, CartItems = new List<CartItem>() };
                _db.Carts.Add(cart);
                await _db.SaveChangesAsync();
            }

            var existing = cart.CartItems?.FirstOrDefault(ci => ci.ProductId == productId);
            var currentQty = existing?.Quantity ?? 0;
            if (currentQty + quantity > product.Stock)
            {
                TempData["ErrorMessage"] = product.Stock <= 0
                    ? "This product is out of stock."
                    : $"Only {product.Stock} in stock. You already have {currentQty} in your cart.";
                return Redirect(Request.Headers["Referer"].ToString() ?? "/");
            }

            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                _db.CartItems.Add(new CartItem { CartId = cart.Id, ProductId = productId, Quantity = quantity });
            }

            await _db.SaveChangesAsync();
            TempData["SuccessMessage"] = "Item added to cart successfully!";
            TempData["OpenCart"] = true;
            return Redirect(Request.Headers["Referer"].ToString() ?? "/");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            var item = await _db.CartItems.FindAsync(cartItemId);
            if (item != null)
            {
                _db.CartItems.Remove(item);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            if (quantity <= 0)
            {
                return await RemoveFromCart(cartItemId);
            }

            var item = await _db.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);

            if (item != null)
            {
                int maxStock = item.Product?.Stock ?? 0;
                if (quantity > maxStock)
                {
                    TempData["ErrorMessage"] = "Cannot exceed available stock.";
                    quantity = maxStock;
                }

                item.Quantity = quantity;
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
