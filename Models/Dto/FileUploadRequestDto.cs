using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HxStudioFileUploadService.Models.Dto
{
    public class FileUploadRequestDto
    {
        public string Name { get; set; }
       // public Guid Domain { get; set; }
       // public Guid Subdomain { get; set; }
        public List<string> Tags { get; set; }
        public string Domainname { get; set; }
        public string Subdomainname { get; set; }
    }
}
