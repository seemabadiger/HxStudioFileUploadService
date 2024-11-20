using System.ComponentModel.DataAnnotations.Schema;

namespace HxStudioFileUploadService.Models
{
    public class Mockup
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        [ForeignKey("MockupGroupId")]
        public int MockupGroupId { get; set; }
        public virtual MockupGroup MockupGroup { get; set; }
        public string Tags { get; set; }
    }
}
