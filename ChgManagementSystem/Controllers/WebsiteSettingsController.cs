using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChgManagementSystem.Data;
using ChgManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ChgManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class WebsiteSettingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WebsiteSettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Edit()
        {
            var settings = _context.WebsiteSettings.FirstOrDefault();

            if (settings == null)
            {
                settings = new WebsiteSettings
                {
                    ChurchName = "The Church Of The Holy Ghost",
                    Motto = "Walking in the Spirit",
                    WelcomeMessage = "Welcome to The Church Of The Holy Ghost. We are delighted to have you visit our website. May God richly bless you.",
                    LogoPath = "/img/icon.jpeg",
                    HeroImagePath = "/img/Mkhu.jpeg",
                    LiveYoutubeUrl = "",
                    FacebookUrl = "",
                    InstagramUrl = "",
                    Phone = "",
                    Email = "",
                    Address = "",
                    SundayService = "09:00",
                    Tuesday_Thursday_Service = "18:00",
                    SaturdayService = "19:00"
                };

                _context.WebsiteSettings.Add(settings);
                _context.SaveChanges();
            }

            return View(settings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
    WebsiteSettings model,
    IFormFile? Logo,
    IFormFile? HeroImage)
        {
            if (!ModelState.IsValid)
                return View(model);

            var settings = await _context.WebsiteSettings
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (settings == null)
                return NotFound();

            settings.ChurchName = model.ChurchName;
            settings.Motto = model.Motto;
            settings.WelcomeMessage = model.WelcomeMessage;
            settings.LiveYoutubeUrl = model.LiveYoutubeUrl;
            settings.FacebookUrl = model.FacebookUrl;
            settings.InstagramUrl = model.InstagramUrl;
            settings.Phone = model.Phone;
            settings.Email = model.Email;
            settings.Address = model.Address;
            settings.SundayService = model.SundayService;
            settings.Tuesday_Thursday_Service = model.Tuesday_Thursday_Service;
            settings.SaturdayService = model.SaturdayService;

            if (Logo != null && Logo.Length > 0)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(Logo.FileName);

                string folder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/uploads");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string path = Path.Combine(folder, fileName);

                using var stream = new FileStream(path, FileMode.Create);

                await Logo.CopyToAsync(stream);

                settings.LogoPath = "/uploads/" + fileName;
            }

            if (HeroImage != null && HeroImage.Length > 0)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(HeroImage.FileName);

                string folder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/uploads");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string path = Path.Combine(folder, fileName);

                using var stream = new FileStream(path, FileMode.Create);

                await HeroImage.CopyToAsync(stream);

                settings.HeroImagePath = "/uploads/" + fileName;
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Website settings updated successfully.";

            return RedirectToAction(nameof(Edit));
        }
    }
}