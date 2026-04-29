using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusMart.Controllers
{
    [Authorize]
    public class UserDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/UserDashboard/Index.cshtml");
        }

        public IActionResult Saved()
        {
            return View("~/Views/UserDashboard/Saved.cshtml");
        }

        public IActionResult Academic()
        {
            return View("~/Views/UserDashboard/Academic.cshtml");
        }
    }
}
