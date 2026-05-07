using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CampusMart.Data;
using CampusMart.Models.Entities;
using CampusMart.Models.ViewModels.User;

namespace CampusMart.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext db, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _db = db;
            _userManager = userManager;
            _env = env;
        }

        // ── RENTALS PAGE ────────────────────────────────────────
        public async Task<IActionResult> Index(int? categoryId)
        {
            var user = await _userManager.GetUserAsync(User);

            var categories = await _db.RentalCategories
                .OrderBy(c => c.Name)
                .ToListAsync();

            var itemsQuery = _db.RentalItems
                .Include(i => i.RentalCategory)
                .Where(i => i.IsActive);

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                itemsQuery = itemsQuery.Where(i => i.RentalCategoryId == categoryId.Value);
            }

            var items = await itemsQuery
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            var userRentals = new List<Rental>();
            if (user != null)
            {
                userRentals = await _db.Rentals
                    .Include(r => r.RentalItem)
                        .ThenInclude(ri => ri.RentalCategory)
                    .Where(r => r.UserId == user.Id && r.Status == "Active")
                    .OrderByDescending(r => r.CreatedAt)
                    .ToListAsync();
            }

            var model = new RentalPageViewModel
            {
                Categories = categories,
                AvailableItems = items,
                UserActiveRentals = userRentals,
                SelectedCategoryId = categoryId
            };

            return View(model);
        }

        // ── RENT AN ITEM ────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rent(int rentalItemId, DateTime startDate, DateTime endDate, string? notes)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var item = await _db.RentalItems
                .Include(i => i.RentalCategory)
                .FirstOrDefaultAsync(i => i.Id == rentalItemId);

            if (item == null || !item.IsActive || item.AvailableStock <= 0)
            {
                TempData["ErrorMessage"] = "This item is not available for rent.";
                return RedirectToAction(nameof(Index));
            }

            if (endDate <= startDate)
            {
                TempData["ErrorMessage"] = "End date must be after start date.";
                return RedirectToAction(nameof(Index));
            }

            // Calculate total price based on unit
            decimal totalPrice = 0;
            var duration = endDate - startDate;
            switch (item.PriceUnit)
            {
                case "hr":
                    totalPrice = item.PricePerUnit * (decimal)duration.TotalHours;
                    break;
                case "day":
                    totalPrice = item.PricePerUnit * (decimal)Math.Ceiling(duration.TotalDays);
                    break;
                case "week":
                    totalPrice = item.PricePerUnit * (decimal)Math.Ceiling(duration.TotalDays / 7.0);
                    break;
                case "mo":
                    totalPrice = item.PricePerUnit * (decimal)Math.Ceiling(duration.TotalDays / 30.0);
                    break;
                default:
                    totalPrice = item.PricePerUnit * (decimal)Math.Ceiling(duration.TotalDays);
                    break;
            }

            var rental = new Rental
            {
                UserId = user.Id,
                RentalItemId = rentalItemId,
                StartDate = startDate,
                EndDate = endDate,
                Status = "Active",
                TotalPrice = Math.Round(totalPrice, 2),
                Notes = notes,
                CreatedAt = DateTime.UtcNow
            };

            item.AvailableStock--;

            _db.Rentals.Add(rental);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Successfully rented \"{item.Name}\"! Total: ₱{rental.TotalPrice:0.00}";
            return RedirectToAction(nameof(Index));
        }

        // ── RETURN A RENTAL ─────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnRental(int rentalId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var rental = await _db.Rentals
                .Include(r => r.RentalItem)
                .FirstOrDefaultAsync(r => r.Id == rentalId && r.UserId == user.Id);

            if (rental == null)
            {
                TempData["ErrorMessage"] = "Rental not found.";
                return RedirectToAction(nameof(Index));
            }

            rental.Status = "Returned";
            if (rental.RentalItem != null)
            {
                rental.RentalItem.AvailableStock++;
            }

            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Returned \"{rental.RentalItem?.Name}\" successfully!";
            return RedirectToAction(nameof(Index));
        }

        // ── SELL PAGE (existing) ────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> Sell()
        {
            var model = new SellListingViewModel
            {
                Categories = await _db.Categories.ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Sell(SellListingViewModel model, IFormFile? listingImages)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            // Validate input
            if (string.IsNullOrWhiteSpace(model.Title))
            {
                TempData["ErrorMessage"] = "Please enter an item title.";
                return RedirectToAction(nameof(Sell));
            }

            if (model.Price <= 0)
            {
                TempData["ErrorMessage"] = "Please enter a valid price.";
                return RedirectToAction(nameof(Sell));
            }

            if (model.CategoryId <= 0)
            {
                TempData["ErrorMessage"] = "Please select a category.";
                return RedirectToAction(nameof(Sell));
            }

            // Handle image upload if provided
            string? imageUrl = null;
            if (listingImages != null && listingImages.Length > 0)
            {
                try
                {
                    // Validate file
                    var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
                    if (!allowedTypes.Contains(listingImages.ContentType))
                    {
                        TempData["ErrorMessage"] = "Invalid image format. Please use JPG, PNG, GIF, or WebP.";
                        return RedirectToAction(nameof(Sell));
                    }

                    if (listingImages.Length > 5 * 1024 * 1024) // 5MB
                    {
                        TempData["ErrorMessage"] = "Image file is too large. Maximum size is 5MB.";
                        return RedirectToAction(nameof(Sell));
                    }

                    // Save image
                    var uploadsDir = Path.Combine(_env.WebRootPath, "uploads", "products");
                    Directory.CreateDirectory(uploadsDir);

                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(listingImages.FileName)}";
                    var filePath = Path.Combine(uploadsDir, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await listingImages.CopyToAsync(stream);
                    }

                    imageUrl = $"/uploads/products/{fileName}";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error uploading image. Please try again.";
                    return RedirectToAction(nameof(Sell));
                }
            }

            var product = new Product
            {
                Name = model.Title,
                Price = model.Price,
                CategoryId = model.CategoryId,
                Description = model.Description ?? "",
                Stock = 1,
                ImageUrl = imageUrl ?? "",
                SellerId = user.Id,
                Status = "Pending"
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Listing \"{product.Name}\" posted successfully! It's now pending admin approval.";
            return RedirectToAction(nameof(Sell));
        }

        // ── PRODUCT DETAIL (existing) ───────────────────────────
        public async Task<IActionResult> Details(int id)
        {
            var product = await _db.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return RedirectToAction("Index", "HomeUser");

            var related = await _db.Products
                .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id)
                .Take(4)
                .ToListAsync();

            var model = new ProductDetailViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Stock = product.Stock,
                CategoryName = product.Category?.Name,
                CategoryId = product.CategoryId,
                RelatedProducts = related.Select(r => new RelatedProductViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Price = r.Price,
                    ImageUrl = r.ImageUrl
                }).ToList()
            };

            return View(model);
        }
    }
}
