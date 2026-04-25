using Microsoft.AspNetCore.Mvc;

namespace CampusMart.Controllers.Admin
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
