using Microsoft.AspNetCore.Mvc;

namespace CampusMart.Controllers.User
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
