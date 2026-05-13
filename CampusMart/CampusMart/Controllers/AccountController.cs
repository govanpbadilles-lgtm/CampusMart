using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CampusMart.Models.ViewModels.Account;
using CampusMart.Models.Entities;

namespace CampusMart.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                // If the user is an admin, send them to the admin dashboard.
                var user = await _userManager.GetUserAsync(User);
                if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

                return RedirectToAction("Index", "UserDashboard");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid input. Please check your data.";
                return View(model);
            }

            // Find user by StudentId (stored as UserName)
            var user = await _userManager.FindByNameAsync(model.StudentId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Invalid Student ID or Password.";
                ModelState.AddModelError("", "Invalid Student ID or Password.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Login Successful! Welcome back.";
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

                return RedirectToAction("Index", "UserDashboard");
            }

            TempData["ErrorMessage"] = "Invalid Student ID or Password.";
            ModelState.AddModelError("", "Invalid Student ID or Password.");
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string studentId, string email)
        {
            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "Please enter both Student ID and Email.";
                return View();
            }

            var user = await _userManager.FindByNameAsync(studentId);
            if (user == null || user.Email != email)
            {
                TempData["ErrorMessage"] = "No account found with those credentials.";
                return View();
            }

            // In a real app, you'd use a token. For this fix, we'll redirect to ResetPassword with the user's ID
            // in TempData to simulate a secure handoff.
            TempData["ResetUserId"] = user.Id;
            return RedirectToAction("ResetPassword");
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            if (TempData["ResetUserId"] == null)
            {
                return RedirectToAction("ForgotPassword");
            }
            // Keep it for the POST request
            TempData.Keep("ResetUserId");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string newPassword, string confirmPassword)
        {
            var userId = TempData["ResetUserId"] as string;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("ForgotPassword");
            }

            if (newPassword != confirmPassword)
            {
                TempData["ErrorMessage"] = "Passwords do not match.";
                TempData.Keep("ResetUserId");
                return View();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("ForgotPassword");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Password has been reset successfully. Please login.";
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            TempData.Keep("ResetUserId");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

                return RedirectToAction("Index", "UserDashboard");
            }

            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fill in all required fields correctly.";
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.StudentId,
                Email = model.Email,
                FullName = $"{model.FirstName} {model.LastName}",
                FirstName = model.FirstName,
                LastName = model.LastName,
                StudentId = model.StudentId,
                Department = model.Department,
                DateTime = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                TempData["SuccessMessage"] = "Account Created Successfully! Please login.";
                return RedirectToAction("Login", "Account");
            }

            TempData["ErrorMessage"] = "Registration failed. Please check the errors.";
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["SuccessMessage"] = "You have been logged out successfully.";
            return RedirectToAction("HomeIndex", "Home");
        }
    }
}
