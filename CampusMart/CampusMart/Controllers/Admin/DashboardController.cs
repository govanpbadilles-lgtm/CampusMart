using Microsoft.AspNetCore.Mvc;

namespace CampusMart.Controllers.Admin
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
