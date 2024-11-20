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
        public DbSet<CaseStudy> CaseStudies { get; set; }
        public DbSet<ProcessType> ProcessTypes { get; set; }
        public DbSet<Deliverable> Deliverables { get; set; }
        public DbSet<ProcessDiagram> ProcessDiagrams { get; set; }
        public DbSet<BeforeAfter> BeforeAfters { get; set; }

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
            modelBuilder.Entity<ProcessType>().HasData(
              new ProcessType { Id = 1, ProcessName = "Discover" },
              new ProcessType { Id = 2, ProcessName = "Define" },
              new ProcessType { Id = 3, ProcessName = "Design" },
              new ProcessType { Id = 4, ProcessName = "Develop" }
          );
            modelBuilder.Entity<Deliverable>().HasData(
              new Deliverable { Id = 1, DeliverableName = "Empathy Mapping" },
              new Deliverable { Id = 2, DeliverableName = "Journey Mapping" },
              new Deliverable { Id = 3, DeliverableName = "Task Flow" },
              new Deliverable { Id = 4, DeliverableName = "Personas" },
              new Deliverable { Id = 5, DeliverableName = "Scenarios" },
              new Deliverable { Id = 6, DeliverableName = "Heuristic Evaluation" },
              new Deliverable { Id = 7, DeliverableName = "Information Architecture (Block Diagram)" },
              new Deliverable { Id = 8, DeliverableName = "Low-hi Fidelity Wireframes" },
              new Deliverable { Id = 9, DeliverableName = "Prototype" },
              new Deliverable { Id = 10, DeliverableName = "Research Report" },
              new Deliverable { Id = 11, DeliverableName = "Branding Style Guide" },
              new Deliverable { Id = 12, DeliverableName = "Visual Design" },
              new Deliverable { Id = 13, DeliverableName = "Design System (Assets, Micro interactions)" },
              new Deliverable { Id = 14, DeliverableName = "Clickable Prototype" },
              new Deliverable { Id = 15, DeliverableName = "HTML CSS Markups" },
              new Deliverable { Id = 16, DeliverableName = "Atomic Design" },
              new Deliverable { Id = 17, DeliverableName = "Accessibilty Compliance(WCAG)" },
              new Deliverable { Id = 18, DeliverableName = "React/Angular based components" }
          );

            // Add default value of 1 for existing records in MockupGroup table
            modelBuilder.Entity<MockupGroup>()
                .Property(m => m.MockupTypeId)
                .HasDefaultValue(1);
        }
    }
}
