using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ChgManagementSystem.Models;

namespace ChgManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Circuit> Circuits { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<TitheRecord> TitheRecords { get; set; }
        public DbSet<BranchLeader> BranchLeaders { get; set; }
        public DbSet<NewsPost> NewsPosts { get; set; }
        public DbSet<OfferingType> OfferingTypes { get; set; }
        public DbSet<MonthlyOffering> MonthlyOfferings { get; set; }
        public DbSet<WebsiteSettings> WebsiteSettings { get; set; }
        public DbSet<GalleryAlbum> GalleryAlbums { get; set; }

        public DbSet<GalleryPhoto> GalleryPhotos { get; set; }
    }
}
