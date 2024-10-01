using HxStudioFileUploadService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HxStudioFileUploadService.Models
{
    public class MockupGroup
    {
        [Key]
        public int Id { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectDescription { get; set; }

        [ForeignKey("Domain")]
        public int DomainId { get; set; }
        public virtual Domain Domain { get; set; }

        [ForeignKey("SubDomain")]
        public int SubDomainId { get; set; }
        public virtual Subdomain SubDomain { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public virtual List<Tag> Tags { get; set; }
        public virtual List<Mockup> Mockups { get; set; }
        public virtual Like Like { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }
}
