using System.ComponentModel.DataAnnotations.Schema;

namespace HxStudioFileUploadService.Models.Dto
{
    public class SubdomainDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DomainId { get; set; }
        public Domain Domain { get; set; }
    }
}
