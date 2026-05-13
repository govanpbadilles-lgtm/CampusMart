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
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;

        public ProductController(AppDbContext db)
        {
            _db = db;
        }

        // Product Detail page
        public async Task<IActionResult> Details(int id)
        {
            var product = await _db.Products
                .Include(p => p.Category)
                .Include(p => p.Stall)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            if (product == null) return RedirectToAction("Index", "UserDashboard");

            var related = await _db.Products
                .Include(p => p.Stall)
                .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id && p.IsActive)
                .Take(4)
                .ToListAsync();

            var model = new ProductDetailViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Stock = product.Stock,
                CategoryName = product.Category?.Name,
                CategoryId = product.CategoryId,
                StallName = product.Stall?.Name,
                StallId = product.StallId,
                RelatedProducts = related.Select(r => new RelatedProductViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Price = r.Price,
                    ImageUrl = r.ImageUrl
                }).ToList()
            };

            return View(model);
        }
    }
}
