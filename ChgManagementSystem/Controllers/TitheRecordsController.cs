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
using ClosedXML.Excel;


namespace ChgManagementSystem.Controllers
{
    public class TitheRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TitheRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TitheRecords
        public async Task<IActionResult> Index(int? month)
        {
            int selectedMonth = month ?? DateTime.Now.Month;

            ViewBag.SelectedMonth = selectedMonth;

            var records = _context.TitheRecords
                .Include(t => t.Member)
                .Where(t => t.Month == selectedMonth);

            return View(await records.ToListAsync());
        }

        // GET: TitheRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var titheRecord = await _context.TitheRecords
                .Include(t => t.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (titheRecord == null)
            {
                return NotFound();
            }

            return View(titheRecord);
        }

        // GET: TitheRecords/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var members = _context.Members
                .Select(m => new
                {
                    m.Id,
                    FullName = m.FirstName + " " + m.LastName
                })
                .ToList();

            ViewData["MemberId"] = new SelectList(
                members,
                "Id",
                "FullName");

            return View();
        }

        // POST: TitheRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Month,Year,PromisedAmount,PaidAmount,DatePaid,MemberId")] TitheRecord titheRecord)
        {

            if (ModelState.IsValid)
            {
                _context.Add(titheRecord);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var members = _context.Members
                .Select(m => new
                {
                    m.Id,
                    FullName = m.FirstName + " " + m.LastName
                })
                .ToList();

            ViewData["MemberId"] = new SelectList(
                members,
                "Id",
                "FullName",
                titheRecord.MemberId);

            return View(titheRecord);
        }
        // GET: TitheRecords/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var titheRecord = await _context.TitheRecords.FindAsync(id);
            if (titheRecord == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "FirstName", titheRecord.MemberId);
            return View(titheRecord);
        }

        // POST: TitheRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Month,Year,PromisedAmount,PaidAmount,DatePaid,MemberId")] TitheRecord titheRecord)
        {
            if (id != titheRecord.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(titheRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TitheRecordExists(titheRecord.Id))
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
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "FirstName", titheRecord.MemberId);
            return View(titheRecord);
        }

        // GET: TitheRecords/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var titheRecord = await _context.TitheRecords
                .Include(t => t.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (titheRecord == null)
            {
                return NotFound();
            }

            return View(titheRecord);
        }

        // POST: TitheRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var titheRecord = await _context.TitheRecords.FindAsync(id);
            if (titheRecord != null)
            {
                _context.TitheRecords.Remove(titheRecord);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TitheRecordExists(int id)
        {
            return _context.TitheRecords.Any(e => e.Id == id);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ExportTithesToExcel()
        {
            var tithes = _context.TitheRecords
                .Include(t => t.Member)
                .ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Tithes");

                worksheet.Cell(1, 1).Value = "Member";
                worksheet.Cell(1, 2).Value = "Month";
                worksheet.Cell(1, 3).Value = "Year";
                worksheet.Cell(1, 4).Value = "Promised";
                worksheet.Cell(1, 5).Value = "Paid";

                int row = 2;

                foreach (var item in tithes)
                {
                    worksheet.Cell(row, 1).Value =
                        item.Member.FirstName + " " + item.Member.LastName;

                    worksheet.Cell(row, 2).Value = item.Month;
                    worksheet.Cell(row, 3).Value = item.Year;
                    worksheet.Cell(row, 4).Value = item.PromisedAmount;
                    worksheet.Cell(row, 5).Value = item.PaidAmount;

                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);

                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Tithes.xlsx");
                }
            }
        }
    }
}
