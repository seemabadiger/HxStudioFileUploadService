using System.ComponentModel.DataAnnotations.Schema;

namespace HxStudioFileUploadService.Models
{
    public class Deliverable
    {
        public int Id { get; set; }
        [ForeignKey("ProcessType")]
        public int ProcessTypeId { get; set; }
        public ProcessType ProcessType { get; set; }
        public string DeliverableName { get; set; } 
    }
}