using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ChgManagementSystem.Data;
using ChgManagementSystem.Models;

namespace ChgManagementSystem.Controllers
{

    public class GalleryAlbumsController : Controller
    {

        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;

        public GalleryAlbumsController(
    ApplicationDbContext context,
    IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: GalleryAlbums
        public async Task<IActionResult> Index()
        {
            return View(await _context.GalleryAlbums
                .Include(x => x.Photos)
                .OrderByDescending(x => x.DateCreated)
                .ToListAsync());
        }

        // GET: GalleryAlbums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var galleryAlbum = await _context.GalleryAlbums
                .FirstOrDefaultAsync(m => m.Id == id);
            if (galleryAlbum == null)
            {
                return NotFound();
            }

            return View(galleryAlbum);
        }

        // GET: GalleryAlbums/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: GalleryAlbums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
    GalleryAlbum galleryAlbum,
    IFormFile? CoverImage)
        {
            if (ModelState.IsValid)
            {
                if (CoverImage != null && CoverImage.Length > 0)
                {
                    string uploads =
                        Path.Combine(_environment.WebRootPath,
                        "uploads/gallery/albums");

                    Directory.CreateDirectory(uploads);

                    string fileName =
                        Guid.NewGuid().ToString() +
                        Path.GetExtension(CoverImage.FileName);

                    string path =
                        Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await CoverImage.CopyToAsync(stream);
                    }

                    galleryAlbum.CoverImagePath =
                        "/uploads/gallery/albums/" + fileName;
                }

                _context.Add(galleryAlbum);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(galleryAlbum);
        }

        // GET: GalleryAlbums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var galleryAlbum = await _context.GalleryAlbums.FindAsync(id);
            if (galleryAlbum == null)
            {
                return NotFound();
            }
            return View(galleryAlbum);
        }

        // POST: GalleryAlbums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CoverImagePath,DateCreated")] GalleryAlbum galleryAlbum)
        {
            if (id != galleryAlbum.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(galleryAlbum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GalleryAlbumExists(galleryAlbum.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(galleryAlbum);
        }

        // GET: GalleryAlbums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var galleryAlbum = await _context.GalleryAlbums
                .FirstOrDefaultAsync(m => m.Id == id);
            if (galleryAlbum == null)
            {
                return NotFound();
            }

            return View(galleryAlbum);
        }

        // POST: GalleryAlbums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var galleryAlbum = await _context.GalleryAlbums.FindAsync(id);
            if (galleryAlbum != null)
            {
                _context.GalleryAlbums.Remove(galleryAlbum);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GalleryAlbumExists(int id)
        {
            return _context.GalleryAlbums.Any(e => e.Id == id);
        }
        public async Task<IActionResult> ViewAlbum(int id)
        {
            var album = await _context.GalleryAlbums
                .Include(a => a.Photos)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (album == null)
                return NotFound();

            return View(album);
        } 

    }
}
