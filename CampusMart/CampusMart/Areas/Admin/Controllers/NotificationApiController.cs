using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CampusMart.Data;

namespace CampusMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NotificationApiController : Controller
    {
        private readonly AppDbContext _db;

        public NotificationApiController(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Returns admin notifications: recent orders, new users, low-stock items.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAdminNotifications()
        {
            var notifications = new List<object>();

            // 1. Recent orders (last 7 days)
            var recentOrders = await _db.Orders
                .Include(o => o.User)
                .Where(o => o.CreatedAt >= DateTime.UtcNow.AddDays(-7))
                .OrderByDescending(o => o.CreatedAt)
                .Take(10)
                .AsNoTracking()
                .Select(o => new
                {
                    o.Id,
                    UserName = o.User != null ? o.User.FullName : "Unknown",
                    o.TotalAmount,
                    o.Status,
                    o.CreatedAt
                })
                .ToListAsync();

            foreach (var order in recentOrders)
            {
                notifications.Add(new
                {
                    id = $"order-{order.Id}",
                    type = "order",
                    icon = "bi-bag-check-fill",
                    iconBg = "rgba(74, 222, 128, 0.15)",
                    iconColor = "#4ade80",
                    title = $"New Order #{order.Id}",
                    message = $"{order.UserName} placed an order for ₱{order.TotalAmount:N2} — Status: {order.Status}",
                    time = FormatTimeAgo(order.CreatedAt),
                    unread = order.CreatedAt >= DateTime.UtcNow.AddHours(-24)
                });
            }

            // 2. Recent user registrations (last 7 days)
            var recentUsers = await _db.Users
                .Where(u => u.DateTime >= DateTime.UtcNow.AddDays(-7))
                .OrderByDescending(u => u.DateTime)
                .Take(5)
                .AsNoTracking()
                .Select(u => new
                {
                    u.FullName,
                    u.StudentId,
                    u.DateTime
                })
                .ToListAsync();

            foreach (var user in recentUsers)
            {
                notifications.Add(new
                {
                    id = $"user-{user.StudentId}",
                    type = "user",
                    icon = "bi-person-plus-fill",
                    iconBg = "rgba(99, 102, 241, 0.15)",
                    iconColor = "#818cf8",
                    title = "New Student Registered",
                    message = $"{user.FullName} ({user.StudentId ?? "N/A"}) joined CampusMart",
                    time = FormatTimeAgo(user.DateTime),
                    unread = user.DateTime >= DateTime.UtcNow.AddHours(-24)
                });
            }

            // 3. Low stock alerts (products with stock <= 5)
            var lowStockProducts = await _db.Products
                .Include(p => p.Stall)
                .Where(p => p.Stock <= 5 && p.IsActive)
                .OrderBy(p => p.Stock)
                .Take(5)
                .AsNoTracking()
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Stock,
                    StallName = p.Stall != null ? p.Stall.Name : "Unknown"
                })
                .ToListAsync();

            foreach (var product in lowStockProducts)
            {
                notifications.Add(new
                {
                    id = $"stock-{product.Id}",
                    type = "alert",
                    icon = "bi-exclamation-triangle-fill",
                    iconBg = "rgba(251, 191, 36, 0.15)",
                    iconColor = "#fbbf24",
                    title = "Low Stock Alert",
                    message = $"\"{product.Name}\" at {product.StallName} has only {product.Stock} unit(s) remaining",
                    time = "Active",
                    unread = true
                });
            }

            // Sort by unread first, then by most recent type
            var sorted = notifications
                .OrderByDescending(n => ((dynamic)n).unread)
                .ToList();

            var unreadCount = sorted.Count(n => ((dynamic)n).unread);

            return Json(new { notifications = sorted, unreadCount });
        }

        private static string FormatTimeAgo(DateTime dateTime)
        {
            var span = DateTime.UtcNow - dateTime;
            if (span.TotalMinutes < 1) return "Just now";
            if (span.TotalMinutes < 60) return $"{(int)span.TotalMinutes}m ago";
            if (span.TotalHours < 24) return $"{(int)span.TotalHours}h ago";
            if (span.TotalDays < 7) return $"{(int)span.TotalDays}d ago";
            return dateTime.ToString("MMM dd");
        }
    }
}
