using System.Diagnostics;
using ChgManagementSystem.Data;
using ChgManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChgManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Settings = await _context.WebsiteSettings.FirstOrDefaultAsync();

            ViewBag.GalleryAlbums = await _context.GalleryAlbums
                .Include(a => a.Photos)
                .OrderByDescending(a => a.Id)
                .ToListAsync();

            var latestPosts = await _context.NewsPosts
                .Where(x => x.IsPublished)
                .OrderByDescending(x => x.DatePosted)
                .Take(6)
                .ToListAsync();

            return View(latestPosts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}