using Microsoft.AspNetCore.Mvc;
using CampusMart.Models.ViewModels.Account;

namespace CampusMart.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginModel)
        {
            // TODO: Implement login logic
            return RedirectToAction("HomeIndex", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel registerModel)
        {
            // TODO: Implement register logic
            return RedirectToAction("HomeIndex", "Home");
        }
    }
}
