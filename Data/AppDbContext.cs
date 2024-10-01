using Microsoft.EntityFrameworkCore;
using HxStudioFileUploadService.Models;

namespace HxStudioFileUploadService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<MockupGroup> MockupGroups { get; set; }
        public DbSet<Mockup> Mockups { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Domain> Domain { get; set; }
        public DbSet<Subdomain> Subdomain { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Like>()
         .HasKey(l => new { l.UserId, l.MockupGroupId });

            modelBuilder.Entity<Subdomain>()
                .HasOne(m => m.Domain)
                .WithMany()
                .HasForeignKey(m => m.DomainId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Mockup>()
                .HasOne(m => m.MockupGroup)
                .WithMany(g => g.Mockups)
                .HasForeignKey(m => m.MockupGroupId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
