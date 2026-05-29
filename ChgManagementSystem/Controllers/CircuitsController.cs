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

namespace ChgManagementSystem.Controllers
{
    public class CircuitsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CircuitsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Circuits
        public async Task<IActionResult> Index()
        {
            return View(await _context.Circuits.ToListAsync());
        }

        // GET: Circuits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var circuit = await _context.Circuits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (circuit == null)
            {
                return NotFound();
            }

            return View(circuit);
        }

        // GET: Circuits/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Circuits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Circuit circuit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(circuit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(circuit);
        }

        // GET: Circuits/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var circuit = await _context.Circuits.FindAsync(id);
            if (circuit == null)
            {
                return NotFound();
            }
            return View(circuit);
        }

        // POST: Circuits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Circuit circuit)
        {
            if (id != circuit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(circuit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CircuitExists(circuit.Id))
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
            return View(circuit);
        }

        // GET: Circuits/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var circuit = await _context.Circuits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (circuit == null)
            {
                return NotFound();
            }

            return View(circuit);
        }

        // POST: Circuits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var circuit = await _context.Circuits.FindAsync(id);
            if (circuit != null)
            {
                _context.Circuits.Remove(circuit);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CircuitExists(int id)
        {
            return _context.Circuits.Any(e => e.Id == id);
        }
    }
}
