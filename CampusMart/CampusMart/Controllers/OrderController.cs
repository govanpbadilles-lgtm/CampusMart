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
    public class OrderController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        private static OrderSummaryViewModel MapOrderToSummary(Order order)
        {
            return new OrderSummaryViewModel
            {
                OrderId = order.Id,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                PaymentMethod = order.PaymentMethod,
                ShippingAddress = order.ShippingAddress,
                ItemCount = order.OrderItems?.Sum(oi => oi.Quantity) ?? 0,
                Items = order.OrderItems?.Select(oi => new OrderItemDetailViewModel
                {
                    ProductName = oi.Product?.Name ?? "Unknown",
                    ImageUrl = oi.Product?.ImageUrl,
                    Price = oi.Price,
                    Quantity = oi.Quantity
                }).ToList() ?? new()
            };
        }

        public async Task<IActionResult> History(int? openReceipt = null)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var orders = await _db.Orders
                .Where(o => o.UserId == user.Id)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            var model = new OrderHistoryViewModel
            {
                Orders = orders.Select(o => new OrderSummaryViewModel
                {
                    OrderId = o.Id,
                    CreatedAt = o.CreatedAt,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                    PaymentMethod = o.PaymentMethod,
                    ShippingAddress = o.ShippingAddress,
                    ItemCount = o.OrderItems?.Sum(oi => oi.Quantity) ?? 0,
                    Items = o.OrderItems?.Select(oi => new OrderItemDetailViewModel
                    {
                        ProductName = oi.Product?.Name ?? "Unknown",
                        ImageUrl = oi.Product?.ImageUrl,
                        Price = oi.Price,
                        Quantity = oi.Quantity
                    }).ToList() ?? new()
                }).ToList()
            };

            ViewBag.OpenReceiptId = openReceipt;
            return View(model);
        }

        /// <summary>Legacy URL: sends users to purchase history with receipt modal.</summary>
        public IActionResult Details(int id)
        {
            return RedirectToAction(nameof(History), new { openReceipt = id });
        }

        [HttpGet]
        public async Task<IActionResult> ReceiptBody(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var order = await _db.Orders
                .Where(o => o.Id == id && o.UserId == user.Id)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync();

            if (order == null) return NotFound();

            return PartialView("_ReceiptModalBody", MapOrderToSummary(order));
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var cart = await _db.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            var model = new CheckoutViewModel
            {
                Items = cart.CartItems.Select(ci => new CartItemViewModel
                {
                    CartItemId = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.Name ?? "Unknown",
                    ImageUrl = ci.Product?.ImageUrl,
                    Price = ci.Product?.Price ?? 0,
                    Quantity = ci.Quantity,
                    CategoryName = ci.Product?.Category?.Name ?? "Uncategorized"
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var cart = await _db.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            foreach (var ci in cart.CartItems)
            {
                if (ci.Product != null && ci.Product.Stock < ci.Quantity)
                {
                    TempData["ErrorMessage"] = $"Insufficient stock for {ci.Product.Name}. Only {ci.Product.Stock} available.";
                    return RedirectToAction("Checkout");
                }
            }

            var method = model.PaymentMethod ?? "Cash";
            if (method is "GCash" or "Bank Transfer")
            {
                if (string.IsNullOrWhiteSpace(model.PaymentAccountMasked))
                {
                    TempData["ErrorMessage"] = "Please enter and confirm your GCash or bank account details before placing the order.";
                    return RedirectToAction("Checkout");
                }
            }

            var pickupLine = "Campus stall pickup";
            var detailParts = new List<string> { pickupLine };
            if (!string.IsNullOrWhiteSpace(model.PaymentAccountMasked))
                detailParts.Add(model.PaymentAccountMasked.Trim());
            if (!string.IsNullOrWhiteSpace(model.Notes))
                detailParts.Add(model.Notes.Trim());

            var order = new Order
            {
                UserId = user.Id,
                Status = "Confirmed",
                TotalAmount = cart.CartItems.Sum(ci => (ci.Product?.Price ?? 0) * ci.Quantity),
                ShippingAddress = string.Join(" · ", detailParts),
                PaymentMethod = method,
                CreatedAt = DateTime.UtcNow,
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    Price = ci.Product?.Price ?? 0
                }).ToList()
            };

            foreach (var ci in cart.CartItems)
            {
                if (ci.Product != null)
                {
                    ci.Product.Stock -= ci.Quantity;

                    var transaction = new Transaction
                    {
                        UserId = user.Id,
                        ProductId = ci.ProductId,
                        Quantity = ci.Quantity,
                        TotalPrice = (ci.Product?.Price ?? 0) * ci.Quantity,
                        TransactionDate = DateTime.UtcNow,
                        Status = "Confirmed"
                    };
                    _db.Transactions.Add(transaction);
                }
            }

            _db.Orders.Add(order);
            _db.CartItems.RemoveRange(cart.CartItems);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Purchase confirmed! Your receipt is ready below.";
            return RedirectToAction(nameof(History), new { openReceipt = order.Id });
        }
    }
}
