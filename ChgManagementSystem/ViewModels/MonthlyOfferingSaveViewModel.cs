namespace ChgManagementSystem.ViewModels
{
    public class MonthlyOfferingSaveViewModel
    {
        public int BranchId { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public Dictionary<int, Dictionary<int, decimal>> Amounts { get; set; }
        // MemberId → (OfferingTypeId → Amount)
    }

}
