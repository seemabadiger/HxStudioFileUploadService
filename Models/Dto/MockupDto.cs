using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HxStudioFileUploadService.Models.Dto
{
    public class MockupDto
    {
        [Required]
        [FromForm]
        public IFormFile MockupFile { get; set; }  // The actual file

        [FromForm]
        public string Tags { get; set; }     // Tags for the file

        [FromForm]
        public string FileName { get; set; } // Optional: Override file name

        [FromForm]
        public string FilePath { get; set; } // Optional: File path or placeholder
        public int? MockupGroupId { get; set; }
        public int Id { get; set; }
    }
}
