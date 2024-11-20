namespace HxStudioFileUploadService.Models.Dto
{
    public class ProcessDiagramResponseDto
    {
        public int Id { get; set; }
        public int ProcessId { get; set; } // Discover, Define, Design, Develop
        public string ProcessName { get; set; } // Discover, Define, Design, Develop
        public List<DeliverableDto>  Deliverables { get; set; }
    }

}
