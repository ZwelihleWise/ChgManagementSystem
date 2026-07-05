using System.ComponentModel.DataAnnotations;

namespace ChgManagementSystem.Models
{
    public class GalleryPhoto
    {
        public int Id { get; set; }

        [Display(Name = "Photo Title")]
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? ImagePath { get; set; }

        public DateTime DateUploaded { get; set; } = DateTime.Now;

        public int GalleryAlbumId { get; set; }

        public virtual GalleryAlbum? GalleryAlbum { get; set; }
    }
}