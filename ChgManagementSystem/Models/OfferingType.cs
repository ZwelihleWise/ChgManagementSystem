using System.ComponentModel.DataAnnotations;

namespace ChgManagementSystem.Models
{
    public class OfferingType
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }
        public ICollection<MonthlyOffering>? MonthlyOfferings { get; set; }
    }
}