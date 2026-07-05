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
using Microsoft.AspNetCore.Identity;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ChgManagementSystem.ViewModels;


namespace ChgManagementSystem.Controllers
{
    public class MembersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MembersController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Members
                .Include(m => m.Branch)
                .Include(m => m.TitheRecords)
                .OrderBy(m => m.LastName)
                .ThenBy(m => m.FirstName);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name");
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,PhoneNumber,Gender,DateJoined,IsActive,BranchId")] Member member)
        {
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", member.BranchId);
            return View(member);
        }

        // GET: Members/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", member.BranchId);
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,PhoneNumber,Gender,DateJoined,IsActive,BranchId")] Member member)
        {
            if (id != member.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Id))
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
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", member.BranchId);
            return View(member);
        }

        // GET: Members/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                _context.Members.Remove(member);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ExportToExcel()
        {
            var members = _context.Members
                .Include(m => m.Branch)
                .ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Members");

                // Headers
                worksheet.Cell(1, 1).Value = "First Name";
                worksheet.Cell(1, 2).Value = "Last Name";
                worksheet.Cell(1, 3).Value = "Phone";
                worksheet.Cell(1, 4).Value = "Gender";
                worksheet.Cell(1, 5).Value = "Branch";

                int row = 2;

                foreach (var member in members)
                {
                    worksheet.Cell(row, 1).Value = member.FirstName;
                    worksheet.Cell(row, 2).Value = member.LastName;
                    worksheet.Cell(row, 3).Value = member.PhoneNumber;
                    worksheet.Cell(row, 4).Value = member.Gender;
                    worksheet.Cell(row, 5).Value = member.Branch?.Name;

                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);

                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Members.xlsx");
                }
            }
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ExportToPdf()
        {
            var members = _context.Members
                .Include(m => m.Branch)
                .ToList();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header()
                        .Text("Church Members Report")
                        .FontSize(20)
                        .Bold();

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Name");
                            header.Cell().Text("Phone");
                            header.Cell().Text("Branch");
                        });

                        foreach (var member in members)
                        {
                            table.Cell().Text(
                                member.FirstName + " " + member.LastName);

                            table.Cell().Text(
                                member.PhoneNumber);

                            table.Cell().Text(
                                member.Branch?.Name);
                        }
                    });
                });
            });

            var pdf = document.GeneratePdf();

            return File(pdf, "application/pdf", "Members.pdf");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateLogin(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid member id");

            var member = _context.Members.FirstOrDefault(x => x.Id == id);

            if (member == null)
                return NotFound("Member not found");

            var model = new CreateLoginViewModel
            {
                MemberId = id,
                Roles = new List<SelectListItem>
        {
            new SelectListItem { Value = "Admin", Text = "Admin" },
            new SelectListItem { Value = "Deacon", Text = "Deacon" },
            new SelectListItem { Value = "Overseer", Text = "Overseer" }
        }
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateLogin(CreateLoginViewModel model)
        {
            var member = await _context.Members.FindAsync(model.MemberId);

            if (member == null)
                return NotFound();

            if (member.HasSystemAccess)
                return BadRequest("Member already has login.");

            string email = $"{member.FirstName}.{member.LastName}{member.Id}@church.local"
                .ToLower();

            string tempPassword = "Temp@1234";

            var user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, tempPassword);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, model.SelectedRole);

                member.HasSystemAccess = true;
                await _context.SaveChangesAsync();

                TempData["Username"] = email;
                TempData["Password"] = tempPassword;
            }

            return RedirectToAction(nameof(Details), new { id = member.Id });
        }

    }
}
