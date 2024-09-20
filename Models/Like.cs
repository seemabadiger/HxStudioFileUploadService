using System;
using System.ComponentModel.DataAnnotations;

namespace HxStudioFileUploadService.Models
{
    public class Like
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid MockupId { get; set; }
        public Mockup Mockup { get; set; }
        public bool IsLiked { get; set; }
    }
}
