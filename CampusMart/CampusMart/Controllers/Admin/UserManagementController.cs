using Microsoft.AspNetCore.Mvc;

namespace CampusMart.Controllers.Admin
{
    [Area("Admin")]
    public class UserManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
