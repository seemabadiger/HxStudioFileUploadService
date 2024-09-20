using HxStudioFileUploadService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HxStudioFileUploadService.Models
{
    public class Mockup
    {
        //[Key]
        //public Guid Id { get; set; }
        //public string Name { get; set; }
     
        //[Column(TypeName = "nvarchar(max)")]
        //public List<string> Tags { get; set; }
        //public string FileName { get; set; }
        //public string FilePath { get; set; }
        //public Guid CreatedBy { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public Guid? ModifiedBy { get; set; }
        //public DateTime? ModifiedDate { get; set; }
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        //public Guid DomainId { get; set; }
        //[ForeignKey("DomainId")]
        //public Domain Domain { get; set; }

        //public Guid SubdomainId { get; set; }
        //[ForeignKey("SubdomainId")]
        //public Subdomain Subdomain { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public List<string> Tags { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Domainname { get; set; }
        public string Subdomainname { get; set; }
    }
}
