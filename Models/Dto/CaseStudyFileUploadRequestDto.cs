using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HxStudioFileUploadService.Models.Dto
{
    public class CaseStudyFileUploadRequestDto
    {
        public int? ImageGroupId { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectDescription { get; set; }
        public string DomainName { get; set; }
        public string SubdomainName { get; set; }
        public string MockupType { get; set; }
        public int MockupTypeId { get; set; }
        public IFormFile? CaseStudyFile { get; set; }
        public IFormFile? ThumbnailImage { get; set; }
        public string? Tags { get; set; }
    }
}
