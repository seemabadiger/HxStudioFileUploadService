using Microsoft.EntityFrameworkCore;
using HxStudioFileUploadService.Models;

namespace HxStudioFileUploadService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Mockup> Mockups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Domain> Domain { get; set; }
        public DbSet<Subdomain> Subdomain { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Like>()
                .HasKey(l => new { l.UserId, l.MockupId });

            modelBuilder.Entity<Subdomain>()
                .HasOne(m => m.Domain)
                .WithMany()
                .HasForeignKey(m => m.DomainId)
                .OnDelete(DeleteBehavior.NoAction);

          
        }
    }
}
