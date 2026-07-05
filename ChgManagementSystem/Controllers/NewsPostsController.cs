using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ChgManagementSystem.Data;
using ChgManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ChgManagementSystem.Controllers
{
    public class NewsPostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewsPostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NewsPosts
        public async Task<IActionResult> Index()
        {
            return View(await _context.NewsPosts.ToListAsync());
        }

        // GET: NewsPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsPost = await _context.NewsPosts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newsPost == null)
            {
                return NotFound();
            }

            return View(newsPost);
        }

        // GET: NewsPosts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NewsPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Deacon")]
        public async Task<IActionResult> Create(
    [Bind("Id,Title,Content,Category,DatePosted,EventDate,IsPublished")]
    NewsPost newsPost,
    IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                // Upload Image
                if (Image != null && Image.Length > 0)
                {
                    string fileName = Guid.NewGuid() +
                                      Path.GetExtension(Image.FileName);

                    string uploadFolder = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot/uploads/news");

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    string filePath = Path.Combine(uploadFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }

                    newsPost.ImagePath = "/uploads/news/" + fileName;
                }

                _context.Add(newsPost);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(newsPost);
        }

        // GET: NewsPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsPost = await _context.NewsPosts.FindAsync(id);
            if (newsPost == null)
            {
                return NotFound();
            }
            return View(newsPost);
        }

        // POST: NewsPosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Deacon")]
        public async Task<IActionResult> Edit(
    int id,
    NewsPost newsPost,
    IFormFile? Image)
        {
            if (id != newsPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingPost =
                        await _context.NewsPosts.FindAsync(id);

                    if (existingPost == null)
                    {
                        return NotFound();
                    }

                    existingPost.Title = newsPost.Title;
                    existingPost.Content = newsPost.Content;
                    existingPost.Category = newsPost.Category;
                    existingPost.EventDate = newsPost.EventDate;
                    existingPost.IsPublished = newsPost.IsPublished;

                    // Replace image if uploaded
                    if (Image != null && Image.Length > 0)
                    {
                        string fileName = Guid.NewGuid() +
                                          Path.GetExtension(Image.FileName);

                        string uploadFolder = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot/uploads/news");

                        if (!Directory.Exists(uploadFolder))
                        {
                            Directory.CreateDirectory(uploadFolder);
                        }

                        string filePath =
                            Path.Combine(uploadFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await Image.CopyToAsync(stream);
                        }

                        existingPost.ImagePath =
                            "/uploads/news/" + fileName;
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsPostExists(newsPost.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(newsPost);
        }

        // GET: NewsPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsPost = await _context.NewsPosts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newsPost == null)
            {
                return NotFound();
            }

            return View(newsPost);
        }

        // POST: NewsPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Deacon")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var newsPost = await _context.NewsPosts.FindAsync(id);
            if (newsPost != null)
            {
                _context.NewsPosts.Remove(newsPost);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsPostExists(int id)
        {
            return _context.NewsPosts.Any(e => e.Id == id);
        }
    }
}
