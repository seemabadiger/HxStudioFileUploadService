using System.ComponentModel.DataAnnotations.Schema;

namespace HxStudioFileUploadService.Models
{
    public class BeforeAfter
    {
        public int Id { get; set; }
        [ForeignKey("MockupGroupId")]
        public int MockupGroupId { get; set; }
        public MockupGroup MockupGroup { get; set; }
        public string? BeforeDesignFileName { get; set; }
        public string? BeforeDesignFilePath { get; set; }
        public string? AfterDesignFileName { get; set; }
        public string? AfterDesignFilePath { get; set; }
        public string? BeforeTags { get; set; }
        public string? AfterTags { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
