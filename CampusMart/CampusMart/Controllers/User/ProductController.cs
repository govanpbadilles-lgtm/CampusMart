using Microsoft.AspNetCore.Mvc;

namespace CampusMart.Controllers.User
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
