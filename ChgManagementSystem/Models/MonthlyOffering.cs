using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChgManagementSystem.Models
{
    public class MonthlyOffering
    {
        public int Id { get; set; }

        [Required]
        public int MemberId { get; set; }

        [ForeignKey("MemberId")]
        public Member Member { get; set; }

        [Required]
        public int OfferingTypeId { get; set; }

        [ForeignKey("OfferingTypeId")]
        public OfferingType OfferingType { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }
    }
}