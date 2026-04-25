using Microsoft.AspNetCore.Mvc;

namespace CampusMart.Controllers.User
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
