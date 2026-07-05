using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ChgManagementSystem.Data;
using ChgManagementSystem.Models;

namespace ChgManagementSystem.Controllers
{
    public class GalleryPhotoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GalleryPhotoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GalleryPhotoes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GalleryPhotos.Include(g => g.GalleryAlbum);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GalleryPhotoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var galleryPhoto = await _context.GalleryPhotos
                .Include(g => g.GalleryAlbum)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (galleryPhoto == null)
            {
                return NotFound();
            }

            return View(galleryPhoto);
        }

        // GET: GalleryPhotoes/Create
        public IActionResult Create(int albumId)
        {
            var photo = new GalleryPhoto
            {
                GalleryAlbumId = albumId
            };

            return View(photo);
        }

        // POST: GalleryPhotoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
    GalleryPhoto galleryPhoto,
    IFormFile? PhotoImage)
        {
            Console.WriteLine("Album ID = " + galleryPhoto.GalleryAlbumId);
            Console.WriteLine("Photo = " + (PhotoImage == null ? "NULL" : PhotoImage.FileName));
            if (ModelState.IsValid)
            {
                if (PhotoImage != null && PhotoImage.Length > 0)
                {
                    string fileName = Guid.NewGuid() + Path.GetExtension(PhotoImage.FileName);

                    string folder = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot/uploads/galleryphotos");

                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    string path = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await PhotoImage.CopyToAsync(stream);
                    }

                    galleryPhoto.ImagePath = "/uploads/galleryphotos/" + fileName;
                }

                galleryPhoto.DateUploaded = DateTime.Now;

                _context.GalleryPhotos.Add(galleryPhoto);

                await _context.SaveChangesAsync();

                return RedirectToAction(
                    "ViewAlbum",
                    "GalleryAlbums",
                    new { id = galleryPhoto.GalleryAlbumId });
            }

            return View(galleryPhoto);
        }

        // GET: GalleryPhotoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var galleryPhoto = await _context.GalleryPhotos.FindAsync(id);
            if (galleryPhoto == null)
            {
                return NotFound();
            }
            ViewData["GalleryAlbumId"] = new SelectList(_context.GalleryAlbums, "Id", "Name", galleryPhoto.GalleryAlbumId);
            return View(galleryPhoto);
        }

        // POST: GalleryPhotoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,ImagePath,DateUploaded,GalleryAlbumId")] GalleryPhoto galleryPhoto)
        {
            if (id != galleryPhoto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(galleryPhoto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GalleryPhotoExists(galleryPhoto.Id))
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
            ViewData["GalleryAlbumId"] = new SelectList(_context.GalleryAlbums, "Id", "Name", galleryPhoto.GalleryAlbumId);
            return View(galleryPhoto);
        }

        // GET: GalleryPhotoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var galleryPhoto = await _context.GalleryPhotos
                .Include(g => g.GalleryAlbum)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (galleryPhoto == null)
            {
                return NotFound();
            }

            return View(galleryPhoto);
        }

        // POST: GalleryPhotoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var galleryPhoto = await _context.GalleryPhotos.FindAsync(id);
            if (galleryPhoto != null)
            {
                _context.GalleryPhotos.Remove(galleryPhoto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GalleryPhotoExists(int id)
        {
            return _context.GalleryPhotos.Any(e => e.Id == id);
        }
        
    }
}
