using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DocumentFormat.OpenXml.Spreadsheet;
namespace ChgManagementSystem.Models
{
    public class Branch
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Location { get; set; }

        public string? BranchImage { get; set; }

        public string? BranchNewsletter { get; set; }

        public string? DeaconInformation { get; set; }

        
        public int CircuitId { get; set; }

        [ForeignKey("CircuitId")]
        public Circuit? Circuit { get; set; }

        
        public ICollection<Member>? Members { get; set; }

        
        public string? ImagePath { get; set; }

       
        public ICollection<BranchLeader>? BranchLeaders { get; set; }
    }
}
