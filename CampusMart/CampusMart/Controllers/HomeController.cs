using CampusMart.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using CampusMart.Data;
using Microsoft.EntityFrameworkCore;

namespace CampusMart.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // Mao ni imong HOME
        public async Task<IActionResult> HomeIndex()
        {
            var categories = await _context.Categories
                .Include(c => c.Products)
                .ToListAsync();

            ViewBag.StallCount = await _context.StallItems.CountAsync();
            ViewBag.RentalCount = await _context.RentalItems.CountAsync();

            return View(categories);
        }

        // Mao ni imong ABOUT
        public IActionResult About()
        {
            return View();
        }

        // Pwede pud ka magpuno og CONTACT
        public IActionResult Contact()
        {
            return View();
        }

        // I-pabilin lang ni para sa safety sa imong app
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
