using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HxStudioFileUploadService.Models
{
    public class Subdomain
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("DomainId")]
        public int DomainId { get; set; }
        public virtual Domain Domain { get; set; }
    }
}
