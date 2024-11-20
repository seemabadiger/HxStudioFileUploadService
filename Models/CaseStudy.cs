using System.ComponentModel.DataAnnotations.Schema;

namespace HxStudioFileUploadService.Models
{
    public class CaseStudy
    {
        public int Id { get; set; }
        [ForeignKey("MockupGroupId")]
        public int MockupGroupId { get; set; }
        public MockupGroup MockupGroup { get; set; }
        public string? CaseStudyFileName { get; set; }
        public string? CaseStudyFilePath { get; set; }
        public string? ThumbnailImageName { get; set; }
        public string? ThumbnailImagePath { get; set; }
        public string? Tags { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
