using System.ComponentModel.DataAnnotations.Schema;

namespace HxStudioFileUploadService.Models
{
    public class ProcessDiagram
    {
        public int Id { get; set; }
        [ForeignKey("ProcessType")]
        public int ProcessTypeId { get; set; } // Discover, Define, Design, Develop
        public ProcessType ProcessType { get; set; } 
        [ForeignKey("Deliverable")]
        public int DeliverableId { get; set; } 
        public Deliverable Deliverable { get; set; }
        public string? DeliverableFileName { get; set; }
        public string? DeliverableFilePath { get; set; }
        public string? DeliverableLink { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

}
