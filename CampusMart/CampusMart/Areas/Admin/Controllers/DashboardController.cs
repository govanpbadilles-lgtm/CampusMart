using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CampusMart.Data;
using CampusMart.Models.Entities;
using System.Linq;

namespace CampusMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _db;

        public DashboardController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch floor and stall data for visualization with eager loading and no tracking
            var floors = await _db.Floors
                .Include(f => f.Stalls)
                    .ThenInclude(s => s.Products)
                .OrderBy(f => f.FloorNumber)
                .AsNoTracking()
                .ToListAsync();

            // Stalls can be derived from floors to avoid a second DB call, 
            // but we'll keep a separate list for easier ViewBag access if preferred.
            var stalls = floors.SelectMany(f => f.Stalls).ToList();

            ViewBag.Floors = floors;
            ViewBag.Stalls = stalls;
            ViewBag.TotalFloors = floors.Count;
            ViewBag.TotalStalls = stalls.Count;
            ViewBag.ActiveStalls = stalls.Count(s => s.IsActive);
            ViewBag.TotalProducts = stalls.Sum(s => s.Products?.Count ?? 0);

            // Calculate occupancy percentage
            var totalCapacity = floors.Sum(f => f.Capacity);
            var occupiedSpaces = stalls.Count(s => s.IsActive);
            ViewBag.OccupancyPercentage = totalCapacity > 0 ? Math.Round((occupiedSpaces * 100.0) / totalCapacity, 2) : 0;

            return View();
        }

        public async Task<IActionResult> Overview()
        {
            // Fetch summary data
            var totalOrders = await _db.Orders.CountAsync();
            var totalUsers = await _db.Users.CountAsync();
            var totalRevenue = await _db.Orders
                .Where(o => o.Status == "Completed" || o.Status == "Confirmed")
                .SumAsync(o => o.TotalAmount);

            var recentOrders = await _db.Orders
                .Include(o => o.User)
                .OrderByDescending(o => o.CreatedAt)
                .AsNoTracking()
                .Take(10)
                .ToListAsync();

            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.RecentOrders = recentOrders;

            return View();
        }
    }
}
