using System;
using System.ComponentModel.DataAnnotations;

namespace Schoolmanagement.Services.ViewModel.Announcement
{
    public class AnnouncementVm
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? TargetAudience { get; set; }
        public DateTime PostedDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class AnnouncementRequest
    {
        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(2000)]
        public string Content { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? TargetAudience { get; set; }
    }
}
