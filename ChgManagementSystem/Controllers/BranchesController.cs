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
   
    public class BranchesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BranchesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Branches
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Branches.Include(b => b.Circuit);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Branches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branches
                .Include(b => b.Circuit)
                .Include(b => b.Members)
                .Include(b=> b.BranchLeaders)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }
        // GET: Branches/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CircuitId"] = new SelectList(_context.Circuits, "Id", "Name");
            return View();
        }

        // POST: Branches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Deacon")]
        public async Task<IActionResult> Create(
      Branch branch,
      IFormFile? BranchImage)
        {
            if (ModelState.IsValid)
            {
                
                if (BranchImage != null && BranchImage.Length > 0)
                {
                    string fileName = Guid.NewGuid() + Path.GetExtension(BranchImage.FileName);

                    string uploadPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot/uploads/branches");

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    string filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await BranchImage.CopyToAsync(stream);
                    }

                    branch.ImagePath = "/uploads/branches/" + fileName;
                }

               

                _context.Add(branch);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["CircuitId"] = new SelectList(
                _context.Circuits,
                "Id",
                "Name",
                branch.CircuitId);

            return View(branch);
        }

        // GET: Branches/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branches.FindAsync(id);
            if (branch == null)
            {
                return NotFound();
            }
            ViewData["CircuitId"] = new SelectList(_context.Circuits, "Id", "Name", branch.CircuitId);
            return View(branch);
        }

        // POST: Branches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Deacon")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Location,CircuitId")] Branch branch)
        {
            if (id != branch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(branch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BranchExists(branch.Id))
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
            ViewData["CircuitId"] = new SelectList(_context.Circuits, "Id", "Name", branch.CircuitId);
            return View(branch);
        }

        // GET: Branches/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branches
                .Include(b => b.Circuit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        // POST: Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch != null)
            {
                _context.Branches.Remove(branch);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BranchExists(int id)
        {
            return _context.Branches.Any(e => e.Id == id);
        }
    }
}
