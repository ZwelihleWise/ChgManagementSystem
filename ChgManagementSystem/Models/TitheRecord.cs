using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ChgManagementSystem.Models
{
    public class TitheRecord
    {
        public int Id { get; set; }

        [Required]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PromisedAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; }

        public DateTime DatePaid { get; set; } = DateTime.Now;

     
        public int MemberId { get; set; }

        [ForeignKey("MemberId")]
        public Member? Member { get; set; }
    }
}
