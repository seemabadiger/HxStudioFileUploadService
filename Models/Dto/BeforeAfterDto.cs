namespace HxStudioFileUploadService.Models.Dto
{
    public class BeforeAfterDto
    {
        public int Id { get; set; }
        public int MockupGroupId { get; set; }
        public MockupGroup MockupGroup { get; set; }
        public IFormFile? BeforeDesignFile { get; set; }
        public IFormFile? AfterDesignFile { get; set; }
        public string? BeforeTags { get; set; }
        public string? AfterTags { get; set; }
    }
}
