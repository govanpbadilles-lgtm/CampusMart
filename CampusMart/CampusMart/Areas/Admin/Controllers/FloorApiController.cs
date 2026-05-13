using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CampusMart.Data;
using CampusMart.Models.Entities;

namespace CampusMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("Admin/[controller]")]
    [IgnoreAntiforgeryToken]
    public class FloorApiController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public FloorApiController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // ── FLOORS ──

        [HttpGet("floors")]
        public async Task<IActionResult> GetFloors()
        {
            var floors = await _db.Floors
                .Include(f => f.Stalls)
                .OrderBy(f => f.FloorNumber)
                .Select(f => new
                {
                    f.Id,
                    f.Name,
                    f.FloorNumber,
                    f.Capacity,
                    f.Description,
                    f.Building,
                    StallCount = f.Stalls.Count(s => s.IsActive),
                    TotalStalls = f.Stalls.Count
                })
                .ToListAsync();

            return Json(floors);
        }

        [HttpPost("floors")]
        public async Task<IActionResult> CreateFloor([FromBody] FloorDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Floor name is required.");

            var floor = new Floor
            {
                Name = dto.Name,
                FloorNumber = dto.FloorNumber,
                Capacity = dto.Capacity > 0 ? dto.Capacity : 8,
                Description = dto.Description,
                Building = dto.Building
            };

            _db.Floors.Add(floor);
            await _db.SaveChangesAsync();

            return Json(new { floor.Id, floor.Name, floor.FloorNumber, floor.Capacity, floor.Description, floor.Building, StallCount = 0, TotalStalls = 0 });
        }

        [HttpDelete("floors/{id}")]
        public async Task<IActionResult> DeleteFloor(int id)
        {
            var floor = await _db.Floors.FindAsync(id);
            if (floor == null) return NotFound();

            _db.Floors.Remove(floor);
            await _db.SaveChangesAsync();
            return Ok();
        }

        // ── STALLS ──

        [HttpGet("floors/{floorId}/stalls")]
        public async Task<IActionResult> GetStalls(int floorId)
        {
            var stalls = await _db.Stalls
                .Where(s => s.FloorId == floorId)
                .OrderBy(s => s.StallNumber)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.StallNumber,
                    s.OwnerName,
                    s.Category,
                    s.Description,
                    s.ImageUrl,
                    s.OpenTime,
                    s.CloseTime,
                    s.IsActive,
                    s.FloorId,
                    ItemCount = s.Products.Count
                })
                .ToListAsync();

            return Json(stalls);
        }

        [HttpPost("stalls")]
        public async Task<IActionResult> CreateStall([FromForm] StallCreateDto dto)
        {
            var floor = await _db.Floors.FindAsync(dto.FloorId);
            if (floor == null) return BadRequest("Floor not found.");

            string? imageUrl = null;
            if (dto.Image != null && dto.Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "stalls");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }
                imageUrl = "/uploads/stalls/" + fileName;
            }

            var stall = new Stall
            {
                Name = dto.Name ?? "",
                StallNumber = dto.StallNumber ?? "",
                OwnerName = dto.OwnerName ?? "",
                Category = dto.Category ?? "",
                Description = dto.Description ?? "",
                ImageUrl = imageUrl,
                OpenTime = dto.OpenTime ?? "08:00",
                CloseTime = dto.CloseTime ?? "20:00",
                IsActive = dto.IsActive,
                FloorId = dto.FloorId
            };

            _db.Stalls.Add(stall);
            await _db.SaveChangesAsync();

            return Json(new
            {
                stall.Id,
                stall.Name,
                stall.StallNumber,
                stall.OwnerName,
                stall.Category,
                stall.Description,
                stall.ImageUrl,
                stall.OpenTime,
                stall.CloseTime,
                stall.IsActive,
                stall.FloorId,
                ItemCount = 0
            });
        }

        [HttpPut("stalls/{id}")]
        public async Task<IActionResult> UpdateStall(int id, [FromForm] StallCreateDto dto)
        {
            var stall = await _db.Stalls.FindAsync(id);
            if (stall == null) return NotFound();

            stall.Name = dto.Name ?? stall.Name;
            stall.StallNumber = dto.StallNumber ?? stall.StallNumber;
            stall.OwnerName = dto.OwnerName ?? stall.OwnerName;
            stall.Category = dto.Category ?? stall.Category;
            stall.Description = dto.Description ?? stall.Description;
            stall.OpenTime = dto.OpenTime ?? stall.OpenTime;
            stall.CloseTime = dto.CloseTime ?? stall.CloseTime;
            stall.IsActive = dto.IsActive;

            if (dto.Image != null && dto.Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "stalls");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }
                stall.ImageUrl = "/uploads/stalls/" + fileName;
            }

            await _db.SaveChangesAsync();
            return Json(new { stall.Id, stall.Name, stall.StallNumber, stall.OwnerName, stall.Category, stall.ImageUrl, stall.OpenTime, stall.CloseTime, stall.IsActive, stall.FloorId });
        }

        [HttpPost("stalls/{id}/toggle")]
        public async Task<IActionResult> ToggleStall(int id)
        {
            var stall = await _db.Stalls.FindAsync(id);
            if (stall == null) return NotFound();

            stall.IsActive = !stall.IsActive;
            await _db.SaveChangesAsync();
            return Json(new { stall.Id, stall.IsActive });
        }

        [HttpDelete("stalls/{id}")]
        public async Task<IActionResult> DeleteStall(int id)
        {
            var stall = await _db.Stalls.FindAsync(id);
            if (stall == null) return NotFound();

            _db.Stalls.Remove(stall);
            await _db.SaveChangesAsync();
            return Ok();
        }

        // ── PRODUCTS (inside stalls) ──

        [HttpGet("stalls/{stallId}/products")]
        public async Task<IActionResult> GetProducts(int stallId)
        {
            var products = await _db.Products
                .Where(p => p.StallId == stallId)
                .Include(p => p.Category)
                .OrderBy(p => p.Name)
                .Select(p => new { p.Id, p.Name, p.Price, p.Description, p.ImageUrl, p.Stock, p.StallId, p.CategoryId, CategoryName = p.Category != null ? p.Category.Name : "" })
                .ToListAsync();

            return Json(products);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories(int? stallId)
        {
            var query = _db.Categories.AsQueryable();
            
            if (stallId.HasValue)
            {
                query = query.Where(c => c.StallId == null || c.StallId == stallId.Value);
            }
            else
            {
                query = query.Where(c => c.StallId == null);
            }

            var categories = await query
                .OrderBy(c => c.Name)
                .Select(c => new { c.Id, c.Name, c.Icon, c.StallId })
                .ToListAsync();

            return Json(categories);
        }

        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) return BadRequest("Category name is required.");

            var category = new Category
            {
                Name = dto.Name,
                Icon = dto.Icon,
                Description = dto.Description,
                StallId = dto.StallId
            };

            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            return Json(new { category.Id, category.Name, category.Icon, category.StallId });
        }

        [HttpPost("products")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDto dto)
        {
            var stall = await _db.Stalls.FindAsync(dto.StallId);
            if (stall == null) return BadRequest("Stall not found.");

            string? imageUrl = null;
            if (dto.Image != null && dto.Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "products");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }
                imageUrl = "/uploads/products/" + fileName;
            }

            var product = new Product
            {
                Name = dto.Name ?? "",
                Price = dto.Price,
                Description = dto.Description ?? "",
                ImageUrl = imageUrl,
                Stock = dto.Stock > 0 ? dto.Stock : 1,
                StallId = dto.StallId,
                CategoryId = dto.CategoryId
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            var category = await _db.Categories.FindAsync(product.CategoryId);

            return Json(new { product.Id, product.Name, product.Price, product.Description, product.ImageUrl, product.Stock, product.StallId, product.CategoryId, CategoryName = category?.Name ?? "" });
        }

        [HttpPut("products/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductCreateDto dto)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            product.Name = dto.Name ?? product.Name;
            product.Price = dto.Price;
            product.Description = dto.Description ?? product.Description;
            product.Stock = dto.Stock;
            product.CategoryId = dto.CategoryId > 0 ? dto.CategoryId : product.CategoryId;

            if (dto.Image != null && dto.Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "products");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }
                product.ImageUrl = "/uploads/products/" + fileName;
            }

            await _db.SaveChangesAsync();

            var category = await _db.Categories.FindAsync(product.CategoryId);
            return Json(new { product.Id, product.Name, product.Price, product.Description, product.ImageUrl, product.Stock, product.StallId, product.CategoryId, CategoryName = category?.Name ?? "" });
        }

        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }

    // ── DTOs ──

    public class FloorDto
    {
        public string? Name { get; set; }
        public int FloorNumber { get; set; }
        public int Capacity { get; set; }
        public string? Description { get; set; }
        public string? Building { get; set; }
    }

    public class CategoryDto
    {
        public string? Name { get; set; }
        public string? Icon { get; set; }
        public string? Description { get; set; }
        public int? StallId { get; set; }
    }

    public class StallCreateDto
    {
        public string? Name { get; set; }
        public string? StallNumber { get; set; }
        public string? OwnerName { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public string? OpenTime { get; set; }
        public string? CloseTime { get; set; }
        public bool IsActive { get; set; }
        public int FloorId { get; set; }
        public IFormFile? Image { get; set; }
    }

    public class ProductCreateDto
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int Stock { get; set; }
        public int StallId { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? Image { get; set; }
    }
}
