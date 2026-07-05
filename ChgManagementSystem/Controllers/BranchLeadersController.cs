using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChgManagementSystem.Data;
using ChgManagementSystem.Models;
using System.IO;

namespace ChgManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BranchLeadersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BranchLeadersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Create
        public IActionResult Create(int branchId)
        {
            var model = new BranchLeader
            {
                BranchId = branchId
            };

            return View(model);
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BranchLeader leader, IFormFile? Photo)
        {
            if (!ModelState.IsValid)
            {
                return View(leader);
            }

            // Upload image
            if (Photo != null && Photo.Length > 0)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(Photo.FileName);

                string folder = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot/uploads/leaders");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fullPath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await Photo.CopyToAsync(stream);
                }

                leader.PhotoPath = "/uploads/leaders/" + fileName;
            }

            // Save to database
            _context.BranchLeaders.Add(leader);
            await _context.SaveChangesAsync();

            // Redirect back to branch details
            return RedirectToAction("Details", "Branches", new { id = leader.BranchId });
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var leader = await _context.BranchLeaders.FindAsync(id);

            if (leader == null)
                return NotFound();

            return View(leader);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, BranchLeader leader, IFormFile? Photo)
        {
            if (id != leader.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(leader);

            var existing = await _context.BranchLeaders.FindAsync(id);

            if (existing == null)
                return NotFound();

            // update fields
            existing.FullName = leader.FullName;
            existing.Position = leader.Position;
            existing.PhoneNumber = leader.PhoneNumber;
            existing.Description = leader.Description;

            // image update
            if (Photo != null && Photo.Length > 0)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(Photo.FileName);

                string folder = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot/uploads/leaders");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fullPath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await Photo.CopyToAsync(stream);
                }

                existing.PhotoPath = "/uploads/leaders/" + fileName;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Branches", new { id = existing.BranchId });
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var leader = await _context.BranchLeaders.FindAsync(id);

            if (leader == null)
                return NotFound();

            _context.BranchLeaders.Remove(leader);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Branches", new { id = leader.BranchId });
        }
    }
}