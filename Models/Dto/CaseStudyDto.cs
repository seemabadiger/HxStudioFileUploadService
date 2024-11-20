namespace HxStudioFileUploadService.Models.Dto
{
    public class CaseStudyDto
    {
        public int Id { get; set; }
        public int MockupGroupId { get; set; }
        public MockupGroup MockupGroup { get; set; }
        public IFormFile? CaseStudyFile { get; set; }
        public IFormFile? ThumbnailImage { get; set; }
        public string? Tags { get; set; }
    }
}
