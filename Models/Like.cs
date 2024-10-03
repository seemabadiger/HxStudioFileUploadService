using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HxStudioFileUploadService.Models
{
    public class Like
    {
        [Key]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("MockupGroupId")]
        public int MockupGroupId { get; set; }
        public virtual MockupGroup MockupGroup { get; set; }
        public bool IsLiked { get; set; }
    }
}
