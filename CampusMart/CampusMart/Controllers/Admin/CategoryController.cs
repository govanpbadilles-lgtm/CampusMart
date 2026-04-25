using Microsoft.AspNetCore.Mvc;

namespace CampusMart.Controllers.Admin
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
