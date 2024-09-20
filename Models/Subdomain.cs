using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HxStudioFileUploadService.Models
{
    public class Subdomain
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid DomainId { get; set; }
        [ForeignKey("DomainId")]
        public Domain Domain { get; set; }
    }
}
