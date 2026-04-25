using Microsoft.AspNetCore.Mvc;

namespace CampusMart.Controllers.User
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
