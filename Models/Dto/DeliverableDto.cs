namespace HxStudioFileUploadService.Models.Dto
{
    public class DeliverableDto
    {
        public int DeliverableId { get; set; }
        public string DeliverableName { get; set; }
        public string? DeliverableFileName { get; set; }
        public string? DeliverableFilePath { get; set; }
        public string? DeliverableLink { get; set; }
    }
}