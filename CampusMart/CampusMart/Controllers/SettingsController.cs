using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CampusMart.Models.Entities;
using CampusMart.Models.ViewModels.User;

namespace CampusMart.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private static readonly string[] AllowedAvatarExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private const long MaxAvatarBytes = 5 * 1024 * 1024;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public SettingsController(UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var model = new ProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                StudentId = user.StudentId,
                Department = user.Department,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Address = user.Address,
                Bio = user.Bio,
                AvatarUrl = user.AvatarUrl,
                YearLevel = user.YearLevel,
                Section = user.Section
            };

            return View("~/Views/Settings/Index.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePersonalInfo(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                model.StudentId = user.StudentId;
                model.Department = user.Department;
                model.Email = user.Email;
                model.AvatarUrl = user.AvatarUrl;
                return View("~/Views/Settings/Index.cshtml", model);
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.FullName = $"{model.FirstName} {model.LastName}";
            user.PhoneNumber = model.Phone;
            user.Address = model.Address;
            user.YearLevel = model.YearLevel;
            user.Section = model.Section;

            // Handle avatar upload
            if (model.AvatarFile != null && model.AvatarFile.Length > 0)
            {
                if (model.AvatarFile.Length > MaxAvatarBytes)
                {
                    ModelState.AddModelError(nameof(ProfileViewModel.AvatarFile), "Image must be 5 MB or smaller.");
                }
                else
                {
                    var ext = Path.GetExtension(model.AvatarFile.FileName).ToLowerInvariant();
                    if (string.IsNullOrEmpty(ext) || !AllowedAvatarExtensions.Contains(ext))
                    {
                        ModelState.AddModelError(nameof(ProfileViewModel.AvatarFile), "Use JPG, PNG, GIF, or WEBP only.");
                    }
                    else
                    {
                        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "avatars");
                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);

                        if (!string.IsNullOrEmpty(user.AvatarUrl))
                            TryDeleteAvatarFile(_env, user.AvatarUrl);

                        var uniqueFileName = $"{Guid.NewGuid():N}{ext}";
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        await using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.AvatarFile.CopyToAsync(stream);
                        }

                        user.AvatarUrl = "/uploads/avatars/" + uniqueFileName;
                    }
                }
            }
            else if (model.RemoveAvatar)
            {
                TryDeleteAvatarFile(_env, user.AvatarUrl);
                user.AvatarUrl = null;
            }

            if (!ModelState.IsValid)
            {
                model.StudentId = user.StudentId;
                model.Department = user.Department;
                model.Email = user.Email;
                model.AvatarUrl = user.AvatarUrl;
                return View("~/Views/Settings/Index.cshtml", model);
            }

            await _userManager.UpdateAsync(user);
            TempData["SuccessMessage"] = "Personal information updated successfully!";
            return RedirectToAction("Index");
        }

        private static void TryDeleteAvatarFile(IWebHostEnvironment env, string? relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl)) return;
            // Handle both legacy and new paths
            bool isLegacy = relativeUrl.StartsWith("/images/avatars/", StringComparison.OrdinalIgnoreCase);
            bool isNew = relativeUrl.StartsWith("/uploads/avatars/", StringComparison.OrdinalIgnoreCase);
            
            if (!isLegacy && !isNew) return;

            var rel = relativeUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var full = Path.Combine(env.WebRootPath, rel);
            try
            {
                if (System.IO.File.Exists(full))
                    System.IO.File.Delete(full);
            }
            catch
            {
                /* ignore IO errors */
            }
        }
    }
}
