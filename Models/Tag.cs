using System.ComponentModel.DataAnnotations.Schema;

namespace HxStudioFileUploadService.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("MockupGroupId")]
        public int MockupGroupId {  get; set; }
        public virtual MockupGroup MockupGroup { get; set; }
    }
}