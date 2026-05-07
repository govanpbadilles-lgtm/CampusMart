using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CampusMart.Data;
using CampusMart.Models.Entities;

namespace CampusMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RentalController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public RentalController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // ── INDEX: Show all categories + items ──────────────────
        public async Task<IActionResult> Index()
        {
            var categories = await _db.RentalCategories
                .Include(c => c.RentalItems)
                .OrderBy(c => c.Name)
                .ToListAsync();

            var recentRentals = await _db.Rentals
                .Include(r => r.User)
                .Include(r => r.RentalItem)
                    .ThenInclude(ri => ri.RentalCategory)
                .OrderByDescending(r => r.CreatedAt)
                .Take(20)
                .ToListAsync();

            ViewBag.Categories = categories;
            ViewBag.RecentRentals = recentRentals;

            return View();
        }

        // ── CREATE CATEGORY ─────────────────────────────────────
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(RentalCategory model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "Category name is required.");
                return View(model);
            }

            var category = new RentalCategory
            {
                Name = model.Name,
                Description = model.Description,
                Icon = string.IsNullOrWhiteSpace(model.Icon) ? "📦" : model.Icon,
                CreatedAt = DateTime.UtcNow
            };

            _db.RentalCategories.Add(category);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Category \"{category.Name}\" created!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _db.RentalCategories.FindAsync(id);
            if (category != null)
            {
                _db.RentalCategories.Remove(category);
                await _db.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Category \"{category.Name}\" deleted.";
            }
            return RedirectToAction(nameof(Index));
        }

        // ── CREATE ITEM ─────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> CreateItem()
        {
            ViewBag.Categories = await _db.RentalCategories.OrderBy(c => c.Name).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateItem(RentalItem model, IFormFile? imageFile)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "Item name is required.");
                ViewBag.Categories = await _db.RentalCategories.OrderBy(c => c.Name).ToListAsync();
                return View(model);
            }

            // Handle image file upload
            string? imageUrl = null;
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsDir = Path.Combine(_env.WebRootPath, "uploads", "rentals");
                Directory.CreateDirectory(uploadsDir);

                var ext = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                var fileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(uploadsDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                imageUrl = $"/uploads/rentals/{fileName}";
            }

            var item = new RentalItem
            {
                Name = model.Name,
                Description = model.Description,
                ImageUrl = imageUrl,
                PricePerUnit = model.PricePerUnit,
                PriceUnit = string.IsNullOrWhiteSpace(model.PriceUnit) ? "day" : model.PriceUnit,
                Location = model.Location,
                TotalStock = model.TotalStock > 0 ? model.TotalStock : 1,
                AvailableStock = model.TotalStock > 0 ? model.TotalStock : 1,
                IsActive = true,
                RentalCategoryId = model.RentalCategoryId,
                CreatedAt = DateTime.UtcNow
            };

            _db.RentalItems.Add(item);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Rental item \"{item.Name}\" created!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _db.RentalItems.FindAsync(id);
            if (item != null)
            {
                _db.RentalItems.Remove(item);
                await _db.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Rental item \"{item.Name}\" deleted.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleItem(int id)
        {
            var item = await _db.RentalItems.FindAsync(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
