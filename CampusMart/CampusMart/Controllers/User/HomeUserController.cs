using Microsoft.AspNetCore.Mvc;

namespace CampusMart.Controllers.User
{
    public class HomeUserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
