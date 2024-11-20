using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HxStudioFileUploadService.Models.Dto
{
    public class FileUploadRequestDto
    {
        public int? ImageGroupId { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectDescription { get; set; }
        public List<IFormFile>? MockupFiles { get; set; }
        public string DomainName { get; set; }
        public string SubdomainName { get; set; }
        public string MockupType { get; set; }
        public int MockupTypeId { get; set; }
        public List<MockupDto>? Mockups { get; set; }
        public CaseStudyDto? CaseStudy { get; set; }
        public BeforeAfterDto? BeforeAfter { get; set; }
    }
}
