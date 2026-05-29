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
    }
}
