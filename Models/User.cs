using System;
using System.ComponentModel.DataAnnotations;

namespace HxStudioFileUploadService.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
