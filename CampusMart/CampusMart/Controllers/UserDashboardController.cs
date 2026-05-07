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

        public async Task<IActionResult> Index(int? floorId)
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

            var stalls = new List<CampusMart.Models.Entities.Stall>();
            if (selectedFloor != null)
            {
                stalls = await _db.Stalls
                    .Where(s => s.FloorId == selectedFloor.Id && s.IsActive)
                    .OrderBy(s => s.StallNumber)
                    .ToListAsync();
            }

            var peerProducts = await _db.Products
                .Include(p => p.Seller)
                .Include(p => p.Category)
                .Where(p => p.Status == "Approved")
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            ViewBag.PeerProducts = peerProducts;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var savedProductIds = await _db.SavedItems
                .Where(s => s.UserId == userId && s.ProductId != null)
                .Select(s => s.ProductId.Value)
                .ToListAsync();
            
            ViewBag.SavedProductIds = savedProductIds;

            return View("~/Views/UserDashboard/Index.cshtml", stalls);
        }

        public async Task<IActionResult> Saved()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var savedItems = await _db.SavedItems
                .Include(s => s.Product)
                    .ThenInclude(p => p.Seller)
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            return View("~/Views/UserDashboard/Saved.cshtml", savedItems);
        }

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
                    UserId = userId,
                    ProductId = productId
                });
                await _db.SaveChangesAsync();
                return Json(new { success = true, saved = true });
            }
        }

        public async Task<IActionResult> Academic()
        {
            var resources = await _db.AcademicResources
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
            return View("~/Views/UserDashboard/Academic.cshtml", resources);
        }
    }
}
