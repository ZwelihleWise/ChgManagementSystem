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

        // Foreign Key
        public int CircuitId { get; set; }

        [ForeignKey("CircuitId")]
        public Circuit? Circuit { get; set; }

        // Navigation Property
        public ICollection<Member>? Members { get; set; }
    }
}
