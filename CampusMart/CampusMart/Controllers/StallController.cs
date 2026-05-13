using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CampusMart.Data;
using CampusMart.Models.Entities;
using System.Linq;

namespace CampusMart.Controllers
{
    [Authorize]
    public class StallController : Controller
    {
        private readonly AppDbContext _db;

        public StallController(AppDbContext db)
        {
            _db = db;
        }

        // View a single stall with its products (with optional search & filter)
        public async Task<IActionResult> Index(int id, string searchString = "", string category = "")
        {
            var stall = await _db.Stalls
                .Include(s => s.Floor)
                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);

            if (stall == null) return NotFound();

            // Load products with filtering
            var productsQuery = _db.Products
                .Where(p => p.StallId == id && p.IsActive)
                .Include(p => p.Category)
                .AsQueryable();

            // Search filter
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                productsQuery = productsQuery.Where(p =>
                    p.Name.Contains(searchString) ||
                    (p.Description != null && p.Description.Contains(searchString)) ||
                    (p.Category != null && p.Category.Name.Contains(searchString)));
            }

            // Category filter
            if (!string.IsNullOrWhiteSpace(category))
            {
                productsQuery = productsQuery.Where(p => p.Category != null && p.Category.Name == category);
            }

            stall.Products = await productsQuery
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            // Populate category list for dropdown
            var allCategories = await _db.Categories
                .Where(c => c.Products.Any(p => p.StallId == id && p.IsActive))
                .OrderBy(c => c.Name)
                .Select(c => c.Name)
                .ToListAsync();
            ViewBag.AllCategories = allCategories;
            ViewBag.SearchString = searchString;
            ViewBag.Category = category;

            return View(stall);
        }
    }
}
