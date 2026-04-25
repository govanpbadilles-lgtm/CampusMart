using Microsoft.AspNetCore.Mvc;

namespace CampusMart.Controllers.Admin
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
