using System.ComponentModel.DataAnnotations;
namespace ChgManagementSystem.Models
{
    public class Circuit
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

       
        public ICollection<Branch>? Branches { get; set; }
    }
}
