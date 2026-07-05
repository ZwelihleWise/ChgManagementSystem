using System.ComponentModel.DataAnnotations;

namespace ChgManagementSystem.Models
{
    public class WebsiteSettings
    {
        public int Id { get; set; }

        [Required]
        public string ChurchName { get; set; }

        public string? Motto { get; set; }

        public string? WelcomeMessage { get; set; }

        public string? LogoPath { get; set; }

        public string? HeroImagePath { get; set; }

        public string? LiveYoutubeUrl { get; set; }

        public string? FacebookUrl { get; set; }

        public string? InstagramUrl { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? SundayService { get; set; }

        public string? Tuesday_Thursday_Service { get; set; }

        public string? SaturdayService { get; set; }
    }
}