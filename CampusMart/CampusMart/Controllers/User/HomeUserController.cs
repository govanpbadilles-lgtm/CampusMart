using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusMart.Controllers.User
{
    [Authorize]
    public class HomeUserController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/User/Home/Index.cshtml");
        }

        public IActionResult Saved()
        {
            return View("~/Views/User/Home/Saved.cshtml");
        }

        public IActionResult Academic()
        {
            return View("~/Views/User/Home/Academic.cshtml");
        }
    }
}
