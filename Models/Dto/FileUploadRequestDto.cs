using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HxStudioFileUploadService.Models.Dto
{
    public class FileUploadRequestDto
    {
        public int? ImageGroupId { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectDescription { get; set; }
        public List<IFormFile> MockupFiles { get; set; }
        public List<string> Tags { get; set; }
        public string DomainName { get; set; }
        public string SubdomainName { get; set; }
    }
}
