using System.ComponentModel.DataAnnotations;

namespace HxStudioFileUploadService.Models
{
    public class Domain
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
