using Microsoft.AspNetCore.Mvc;

namespace CampusMart.Controllers.Admin
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
