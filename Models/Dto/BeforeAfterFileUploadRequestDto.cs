using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HxStudioFileUploadService.Models.Dto
{
    public class BeforeAfterFileUploadRequestDto
    {
        public int? ImageGroupId { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectDescription { get; set; }
        public string DomainName { get; set; }
        public string SubdomainName { get; set; }
        public string MockupType { get; set; }        
        public IFormFile BeforeFile { get; set; }
        public string BeforeTags { get; set; }
        public IFormFile AfterFile { get; set; }        
        public string AfterTags { get; set; }
    }
}
