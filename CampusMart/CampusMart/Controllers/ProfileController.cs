using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CampusMart.Data;
using CampusMart.Models.Entities;
using CampusMart.Models.ViewModels.User;

namespace CampusMart.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private static readonly string[] AllowedAvatarExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private const long MaxAvatarBytes = 5 * 1024 * 1024;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public ProfileController(UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;
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
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                // Re-populate non-editable fields if there are validation errors
                model.StudentId = user.StudentId;
                model.Department = user.Department;
                model.Email = user.Email;
                model.AvatarUrl = user.AvatarUrl;
                return View("Index", model);
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.FullName = $"{model.FirstName} {model.LastName}";
            user.PhoneNumber = model.Phone;
            user.Address = model.Address;
            user.Bio = model.Bio;
            user.YearLevel = model.YearLevel;
            user.Section = model.Section;

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
                return View("Index", model);
            }

            await _userManager.UpdateAsync(user);
            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> QuickUpdate([FromForm] string fullName, [FromForm] string email, [FromForm] string phoneNumber, [FromForm] IFormFile? avatarFile)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            if (!string.IsNullOrWhiteSpace(fullName))
            {
                user.FullName = fullName;
                var names = fullName.Split(' ', 2);
                user.FirstName = names[0];
                if (names.Length > 1) user.LastName = names[1];
            }

            if (!string.IsNullOrWhiteSpace(email)) user.Email = email;
            if (!string.IsNullOrWhiteSpace(phoneNumber)) user.PhoneNumber = phoneNumber;

            if (avatarFile != null && avatarFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "avatars");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                if (!string.IsNullOrEmpty(user.AvatarUrl))
                    TryDeleteAvatarFile(_env, user.AvatarUrl);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(avatarFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await avatarFile.CopyToAsync(stream);
                }
                user.AvatarUrl = "/uploads/avatars/" + uniqueFileName;
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { avatarUrl = user.AvatarUrl, fullName = user.FullName, email = user.Email, phoneNumber = user.PhoneNumber });
            }
            return BadRequest("Failed to update profile");
        }
    }
}
