using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

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
        [FromForm]
        public List<MockupDto> Mockups { get; set; } = new List<MockupDto>();
    }
}
