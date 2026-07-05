using System.ComponentModel.DataAnnotations;

namespace ChgManagementSystem.Models
{
    public class GalleryAlbum
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Album Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        public string? CoverImagePath { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public virtual ICollection<GalleryPhoto> Photos { get; set; }
            = new List<GalleryPhoto>();
    }
}