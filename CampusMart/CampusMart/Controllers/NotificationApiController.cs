using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CampusMart.Data;
using System.Security.Claims;

namespace CampusMart.Controllers
{
    [Authorize]
    public class NotificationApiController : Controller
    {
        private readonly AppDbContext _db;

        public NotificationApiController(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Returns user-specific notifications: order updates, system announcements.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetUserNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notifications = new List<object>();

            // 1. User's recent orders (last 30 days)
            var recentOrders = await _db.Orders
                .Where(o => o.UserId == userId && o.CreatedAt >= DateTime.UtcNow.AddDays(-30))
                .OrderByDescending(o => o.CreatedAt)
                .Take(10)
                .AsNoTracking()
                .Select(o => new
                {
                    o.Id,
                    o.TotalAmount,
                    o.Status,
                    o.CreatedAt
                })
                .ToListAsync();

            foreach (var order in recentOrders)
            {
                var (icon, iconBg, iconColor, statusMsg) = GetOrderStatusDetails(order.Status);
                notifications.Add(new
                {
                    id = $"order-{order.Id}",
                    type = "order",
                    icon,
                    iconBg,
                    iconColor,
                    title = $"Order #{order.Id} — {order.Status}",
                    message = $"{statusMsg} Total: ₱{order.TotalAmount:N2}",
                    time = FormatTimeAgo(order.CreatedAt),
                    unread = order.CreatedAt >= DateTime.UtcNow.AddHours(-24)
                });
            }

            // 2. Saved item stock alerts
            var savedItems = await _db.SavedItems
                .Include(s => s.Product)
                .Where(s => s.UserId == userId && s.Product != null && s.Product.Stock <= 5 && s.Product.IsActive)
                .Take(5)
                .AsNoTracking()
                .Select(s => new
                {
                    s.Product!.Id,
                    s.Product.Name,
                    s.Product.Stock
                })
                .ToListAsync();

            foreach (var item in savedItems)
            {
                notifications.Add(new
                {
                    id = $"saved-stock-{item.Id}",
                    type = "alert",
                    icon = "bi-heart-fill",
                    iconBg = "rgba(251, 191, 36, 0.15)",
                    iconColor = "#fbbf24",
                    title = "Saved Item Low Stock",
                    message = $"\"{item.Name}\" has only {item.Stock} unit(s) left — grab it before it's gone!",
                    time = "Active",
                    unread = true
                });
            }

            // 3. System welcome notification (always show for new users)
            notifications.Add(new
            {
                id = "system-welcome",
                type = "system",
                icon = "bi-stars",
                iconBg = "rgba(99, 102, 241, 0.15)",
                iconColor = "#818cf8",
                title = "Welcome to CampusMart!",
                message = "Browse stalls, discover products, and enjoy your campus marketplace experience.",
                time = "",
                unread = false
            });

            var sorted = notifications
                .OrderByDescending(n => ((dynamic)n).unread)
                .ThenByDescending(n => ((dynamic)n).type == "order")
                .ToList();

            var unreadCount = sorted.Count(n => ((dynamic)n).unread);

            return Json(new { notifications = sorted, unreadCount });
        }

        private static (string icon, string iconBg, string iconColor, string statusMsg) GetOrderStatusDetails(string status)
        {
            return status switch
            {
                "Pending" => ("bi-clock-fill", "rgba(251, 191, 36, 0.15)", "#fbbf24", "Your order is being reviewed."),
                "Confirmed" => ("bi-check-circle-fill", "rgba(99, 102, 241, 0.15)", "#818cf8", "Your order has been confirmed!"),
                "Completed" => ("bi-bag-check-fill", "rgba(74, 222, 128, 0.15)", "#4ade80", "Your order was completed successfully."),
                "Cancelled" => ("bi-x-circle-fill", "rgba(239, 68, 68, 0.15)", "#f87171", "This order was cancelled."),
                _ => ("bi-bag-fill", "rgba(255, 255, 255, 0.1)", "#a1a1aa", $"Order status: {status}")
            };
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
