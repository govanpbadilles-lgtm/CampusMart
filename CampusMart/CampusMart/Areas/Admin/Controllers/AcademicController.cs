using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CampusMart.Data;
using CampusMart.Models.Entities;

namespace CampusMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AcademicController : Controller
    {
        private readonly AppDbContext _db;

        public AcademicController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var resources = await _db.AcademicResources.OrderByDescending(r => r.CreatedAt).ToListAsync();
            return View(resources);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string title, string description, IFormFile? imageFile)
        {
            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(description))
            {
                string imageUrl = "";
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "academic");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    imageUrl = "/images/academic/" + uniqueFileName;
                }

                _db.AcademicResources.Add(new AcademicResource
                {
                    Title = title,
                    Description = description,
                    ImageUrl = imageUrl
                });
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Academic", new { area = "Admin" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var resource = await _db.AcademicResources.FindAsync(id);
            if (resource != null)
            {
                _db.AcademicResources.Remove(resource);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Academic", new { area = "Admin" });
        }
    }
}
