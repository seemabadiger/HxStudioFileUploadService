using System.ComponentModel.DataAnnotations.Schema;

namespace HxStudioFileUploadService.Models.Dto
{
    public class SubdomainDto
    {
        public string Name { get; set; }

        public Guid DomainId { get; set; }
       
        
    }
}
