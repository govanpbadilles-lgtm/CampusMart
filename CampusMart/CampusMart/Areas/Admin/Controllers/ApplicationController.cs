using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CampusMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ApplicationController : Controller
    {
        public IActionResult Index()
        {
            // Dummy data for now to show the UI works
            var applications = new[]
            {
                new { Id = 1, StudentName = "John Doe", StoreName = "Doe's Snacks", Category = "Food", DateApplied = DateTime.Now.AddDays(-2), Status = "Pending" },
                new { Id = 2, StudentName = "Jane Smith", StoreName = "Jane's Books", Category = "Textbooks", DateApplied = DateTime.Now.AddDays(-1), Status = "Pending" },
                new { Id = 3, StudentName = "Mike Johnson", StoreName = "Mike's Electronics", Category = "Electronics", DateApplied = DateTime.Now, Status = "Pending" }
            };

            return View(applications);
        }
    }
}
