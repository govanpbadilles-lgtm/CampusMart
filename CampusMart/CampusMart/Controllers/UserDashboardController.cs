using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CampusMart.Data;
using CampusMart.Models.Entities;
using System.Security.Claims;

namespace CampusMart.Controllers
{
    [Authorize]
    public class UserDashboardController : Controller
    {
        private readonly AppDbContext _db;

        public UserDashboardController(AppDbContext db)
        {
            _db = db;
        }

        // Main dashboard: browse marketplace by floor/stall/category
        public async Task<IActionResult> Index(int? floorId, string? searchString, string? category)
        {
            var floors = await _db.Floors
                .OrderBy(f => f.FloorNumber)
                .ToListAsync();

            ViewBag.Floors = floors;

            // Default to first floor if none selected
            var selectedFloor = floorId.HasValue
                ? floors.FirstOrDefault(f => f.Id == floorId.Value)
                : floors.FirstOrDefault();

            ViewBag.SelectedFloor = selectedFloor;

            // Get stalls for the selected floor
            var stallsQuery = _db.Stalls
                .Include(s => s.Products)
                .AsQueryable();

            if (selectedFloor != null)
            {
                stallsQuery = stallsQuery.Where(s => s.FloorId == selectedFloor.Id && s.IsActive);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                stallsQuery = stallsQuery.Where(s => 
                    s.Name.Contains(searchString) || 
                    s.Description.Contains(searchString) ||
                    s.Products.Any(p => p.Name.Contains(searchString)));
            }

            if (!string.IsNullOrEmpty(category))
            {
                stallsQuery = stallsQuery.Where(s => s.Category == category);
            }

            var stalls = await stallsQuery.OrderBy(s => s.StallNumber).ToListAsync();

            // Global product search results
            var productsQuery = _db.Products
                .Include(p => p.Category)
                .Include(p => p.Stall)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                productsQuery = productsQuery.Where(p => 
                    p.Name.Contains(searchString) || 
                    (p.Description != null && p.Description.Contains(searchString)) ||
                    p.Category!.Name.Contains(searchString) ||
                    p.Stall!.Name.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(category))
            {
                productsQuery = productsQuery.Where(p => p.Category != null && p.Category.Name == category);
            }

            var products = await productsQuery
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            ViewBag.Products = products;
            ViewBag.SearchString = searchString;
            ViewBag.Category = category;

            // Get all unique categories for the dropdown
            var stallCategories = await _db.Stalls
                .Where(s => s.IsActive && !string.IsNullOrEmpty(s.Category))
                .Select(s => s.Category)
                .Distinct()
                .ToListAsync();
            var productCategories = await _db.Categories.Select(c => c.Name).ToListAsync();
            ViewBag.AllCategories = stallCategories.Union(productCategories).OrderBy(c => c).ToList();

            // Saved items for the current user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var savedProductIds = await _db.SavedItems
                .Where(s => s.UserId == userId)
                .Select(s => s.ProductId)
                .ToListAsync();
            
            ViewBag.SavedProductIds = savedProductIds;

            return View("~/Views/UserDashboard/Index.cshtml", stalls);
        }

        // Saved items page
        public async Task<IActionResult> Saved()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var savedItems = await _db.SavedItems
                .Include(s => s.Product)
                    .ThenInclude(p => p.Category)
                .Include(s => s.Product)
                    .ThenInclude(p => p.Stall)
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            return View("~/Views/UserDashboard/Saved.cshtml", savedItems);
        }

        // Toggle save/unsave a product
        [HttpPost]
        public async Task<IActionResult> ToggleSave(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existing = await _db.SavedItems.FirstOrDefaultAsync(s => s.UserId == userId && s.ProductId == productId);
            
            if (existing != null)
            {
                _db.SavedItems.Remove(existing);
                await _db.SaveChangesAsync();
                return Json(new { success = true, saved = false });
            }
            else
            {
                _db.SavedItems.Add(new SavedItem
                {
                    UserId = userId!,
                    ProductId = productId
                });
                await _db.SaveChangesAsync();
                return Json(new { success = true, saved = true });
            }
        }
    }
}
