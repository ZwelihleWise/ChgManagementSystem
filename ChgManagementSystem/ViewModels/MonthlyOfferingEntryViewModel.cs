using ChgManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChgManagementSystem.ViewModels
{
    public class MonthlyOfferingEntryViewModel
    {
        public int BranchId { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public List<Member> Members { get; set; }

        public List<OfferingType> OfferingTypes { get; set; }

        public List<SelectListItem> Branches { get; set; }
        public List<OfferingEntryViewModel> Entries { get; set; }
    = new List<OfferingEntryViewModel>();
    }
}