using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClosedXML.Excel;
using System.IO;

namespace ChgManagementSystem.Models
{
    public class Member
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Gender { get; set; }

        public DateTime DateJoined { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        // Foreign Key
        public int BranchId { get; set; }

        [ForeignKey("BranchId")]
        public Branch? Branch { get; set; }

        // Navigation Property
        public ICollection<TitheRecord>? TitheRecords { get; set; }
    }
}
