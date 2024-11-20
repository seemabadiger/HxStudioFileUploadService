using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HxStudioFileUploadService.Models.Dto
{
    public class ProcessDiagramFileUploadRequestDto
    {
        public Guid UserId { get; set; }
        public int ProcessTypeId { get; set; }
        public int DeliverableId { get; set; }
        public IFormFile? DeliverableFile { get; set; }
        public string? DeliverableLink { get; set; }
    }
}
