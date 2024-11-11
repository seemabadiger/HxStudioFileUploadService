using Microsoft.EntityFrameworkCore;
using HxStudioFileUploadService.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

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
        public DbSet<MockupType> MockupTypes { get; set; }

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

            modelBuilder.Entity<MockupType>().HasData(
                new MockupType { Id = 1, Name = "Visual Samples" },
                new MockupType { Id = 2, Name = "Case Studies" },
                new MockupType { Id = 3, Name = "Process Diagram & Artifacts" },
                new MockupType { Id = 4, Name = "Before After" }
            );

            // Add default value of 1 for existing records in MockupGroup table
            modelBuilder.Entity<MockupGroup>()
                .Property(m => m.MockupTypeId)
                .HasDefaultValue(1);
        }
    }
}
