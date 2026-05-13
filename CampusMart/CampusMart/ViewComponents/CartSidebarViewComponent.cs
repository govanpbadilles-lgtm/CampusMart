using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CampusMart.Data;
using CampusMart.Models.Entities;
using CampusMart.Models.ViewModels.User;

namespace CampusMart.ViewComponents
{
    public class CartSidebarViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartSidebarViewComponent(AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var model = new CartViewModel();

            if (user != null)
            {
                var cart = await _db.Carts
                    .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                            .ThenInclude(p => p.Category)
                    .FirstOrDefaultAsync(c => c.UserId == user.Id);

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
            }

            return View(model);
        }
    }
}
