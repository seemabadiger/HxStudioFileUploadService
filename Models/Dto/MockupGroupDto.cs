using System.ComponentModel.DataAnnotations.Schema;

namespace HxStudioFileUploadService.Models.Dto
{
    public class MockupGroupDto
    {
        public int Id { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectDescription { get; set; }
        public int DomainId { get; set; }
        public DomainDto Domain { get; set; }
        public int SubDomainId { get; set; }
        public SubdomainDto Subdomain { get; set; }
        public List<Tag> Tags { get; set; }
        public List<MockupDto> Mockups { get; set; }
        public Like Like { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
