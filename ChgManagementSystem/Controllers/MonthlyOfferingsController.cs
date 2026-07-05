using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ChgManagementSystem.Data;
using ChgManagementSystem.Models;
using ChgManagementSystem.ViewModels;

namespace ChgManagementSystem.Controllers
{
    public class MonthlyOfferingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MonthlyOfferingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult SelectPeriod(int branchId)
        {
            var model = new SelectOfferingPeriodViewModel
            {
                BranchId = branchId,
                Month = DateTime.Now.Month,
                Year = DateTime.Now.Year
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectPeriod(SelectOfferingPeriodViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            return RedirectToAction(nameof(Create), new
            {
                branchId = model.BranchId,
                month = model.Month,
                year = model.Year
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MonthlyOfferingEntryViewModel model)
        {
            foreach (var key in Request.Form.Keys)
            {
                if (!key.StartsWith("amounts["))
                    continue;

                string cleaned = key
    .Replace("amounts[", "")
    .Replace("][", ",")
    .Replace("]", "");

                string[] values = cleaned.Split(',');

                if (values.Length != 2)
                    continue;

                int memberId = int.Parse(values[0]);
                int offeringTypeId = int.Parse(values[1]);



                if (!decimal.TryParse(Request.Form[key], out decimal amount))
                    continue;

                if (amount > 0)
                {
                    var existing = _context.MonthlyOfferings.FirstOrDefault(x =>
    x.MemberId == memberId &&
    x.OfferingTypeId == offeringTypeId &&
    x.Month == model.Month &&
    x.Year == model.Year);

                    if (existing == null)
                    {
                        _context.MonthlyOfferings.Add(new MonthlyOffering
                        {
                            MemberId = memberId,
                            OfferingTypeId = offeringTypeId,
                            Month = model.Month,
                            Year = model.Year,
                            Amount = amount
                        });
                    }
                    else
                    {
                        existing.Amount = amount;
                    }
                }
            }

            await _context.SaveChangesAsync();

            TempData["Success"] =
    $"{Request.Form.Keys.Count(k => k.StartsWith("amounts["))} offering entries processed successfully.";

            return RedirectToAction(
    "Details",
    "Branches",
    new { id = model.BranchId });
        }

        [HttpGet]
        public IActionResult Create(int branchId, int month, int year)
        {
            var branch = _context.Branches
                .Include(b => b.Members)
                .FirstOrDefault(b => b.Id == branchId);

            if (branch == null)
                return NotFound();

            var model = new MonthlyOfferingEntryViewModel
            {
                BranchId = branchId,
                Month = month,
                Year = year,
                Members = branch.Members == null
    ? new List<Member>()
    : branch.Members
        .OrderBy(x => x.LastName)
        .ThenBy(x => x.FirstName)
        .ToList(),
                OfferingTypes = _context.OfferingTypes
    .OrderBy(x => x.Name)
    .ToList()
            };

            return View(model);
        }
        
        public IActionResult MemberStatement(int memberId)
        {
            var member = _context.Members
                .Include(m => m.Branch)
                .FirstOrDefault(m => m.Id == memberId);

            if (member == null)
                return NotFound();

            var offerings = _context.MonthlyOfferings
                .Include(o => o.OfferingType)
                .Where(o => o.MemberId == memberId)
                .OrderByDescending(o => o.Year)
                .ThenByDescending(o => o.Month)
                .ToList();

            ViewBag.Member = member;

            return View(offerings);
        }
        public IActionResult MonthlyReport(int branchId, int month, int year)
        {
            var branch = _context.Branches
                .FirstOrDefault(x => x.Id == branchId);

            if (branch == null)
                return NotFound();

            var offerings = _context.MonthlyOfferings
                .Include(x => x.Member)
                .Include(x => x.OfferingType)
                .Where(x =>
                    x.Member.BranchId == branchId &&
                    x.Month == month &&
                    x.Year == year)
                .ToList();

            ViewBag.Branch = branch;
            ViewBag.Month = month;
            ViewBag.Year = year;

            return View(offerings);
        }
        public IActionResult ExportExcel(int branchId, int month, int year)
        {
            var offerings = _context.MonthlyOfferings
                .Include(x => x.Member)
                .Include(x => x.OfferingType)
                .Where(x => x.Member.BranchId == branchId &&
                            x.Month == month &&
                            x.Year == year)
                .OrderBy(x => x.Member.LastName)
                .ToList();

            using var workbook = new XLWorkbook();

            var ws = workbook.Worksheets.Add("Monthly Report");

            ws.Cell(1, 1).Value = "Member";
            ws.Cell(1, 2).Value = "Offering";
            ws.Cell(1, 3).Value = "Amount";

            int row = 2;

            foreach (var item in offerings)
            {
                ws.Cell(row, 1).Value =
                    item.Member.FirstName + " " + item.Member.LastName;

                ws.Cell(row, 2).Value =
                    item.OfferingType.Name;

                ws.Cell(row, 3).Value =
                    item.Amount;

                row++;
            }

            ws.Cell(row, 2).Value = "TOTAL";

            ws.Cell(row, 3).FormulaA1 =
                $"SUM(C2:C{row - 1})";

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"MonthlyReport_{month}_{year}.xlsx");
        }
        public IActionResult ExportPdf(int branchId, int month, int year)
        {
            var offerings = _context.MonthlyOfferings
                .Include(x => x.Member)
                .Include(x => x.OfferingType)
                .Where(x => x.Member.BranchId == branchId &&
                            x.Month == month &&
                            x.Year == year)
                .OrderBy(x => x.Member.LastName)
                .ToList();

            QuestPDF.Settings.License = LicenseType.Community;

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header()
                        .Text($"Monthly Offering Report ({month}/{year})")
                        .FontSize(18)
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
                            header.Cell().Text("Member").Bold();
                            header.Cell().Text("Offering").Bold();
                            header.Cell().Text("Amount").Bold();
                        });

                        foreach (var item in offerings)
                        {
                            table.Cell().Text(item.Member.FirstName + " " + item.Member.LastName);
                            table.Cell().Text(item.OfferingType.Name);
                            table.Cell().Text(item.Amount.ToString("N2"));
                        }

                        table.Cell().Text("");
                        table.Cell().Text("TOTAL").Bold();
                        table.Cell().Text(
                            offerings.Sum(x => x.Amount).ToString("N2"))
                            .Bold();
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text("CHG Management System");
                });
            });

            return File(
                pdf.GeneratePdf(),
                "application/pdf",
                $"MonthlyReport_{month}_{year}.pdf");
        }

        [HttpGet]
        public IActionResult SelectReportPeriod(int branchId)
        {
            return View(new SelectOfferingPeriodViewModel
            {
                BranchId = branchId,
                Month = DateTime.Now.Month,
                Year = DateTime.Now.Year
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectReportPeriod(SelectOfferingPeriodViewModel model)
        {
            return RedirectToAction(nameof(MonthlyReport), new
            {
                branchId = model.BranchId,
                month = model.Month,
                year = model.Year
            });
        }

    }
}