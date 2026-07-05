namespace ChgManagementSystem.Models
{
    public class BranchLeader
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Position { get; set; }

        public string? PhoneNumber { get; set; }

        public string? PhotoPath { get; set; }

        public string? Description { get; set; }

        public int BranchId { get; set; }

        public Branch? Branch { get; set; }
    }
}
