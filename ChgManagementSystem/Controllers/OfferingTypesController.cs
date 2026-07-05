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
    public class OfferingTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OfferingTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OfferingTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.OfferingTypes.ToListAsync());
        }

        // GET: OfferingTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offeringType = await _context.OfferingTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offeringType == null)
            {
                return NotFound();
            }

            return View(offeringType);
        }

        // GET: OfferingTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OfferingTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] OfferingType offeringType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(offeringType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(offeringType);
        }

        // GET: OfferingTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offeringType = await _context.OfferingTypes.FindAsync(id);
            if (offeringType == null)
            {
                return NotFound();
            }
            return View(offeringType);
        }

        // POST: OfferingTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] OfferingType offeringType)
        {
            if (id != offeringType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(offeringType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfferingTypeExists(offeringType.Id))
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
            return View(offeringType);
        }

        // GET: OfferingTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offeringType = await _context.OfferingTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offeringType == null)
            {
                return NotFound();
            }

            return View(offeringType);
        }

        // POST: OfferingTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var offeringType = await _context.OfferingTypes.FindAsync(id);
            if (offeringType != null)
            {
                _context.OfferingTypes.Remove(offeringType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfferingTypeExists(int id)
        {
            return _context.OfferingTypes.Any(e => e.Id == id);
        }
    }
}
