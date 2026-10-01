using System;
using System.ComponentModel.DataAnnotations;

namespace Schoolmanagement.Services.ViewModel.HomeWork
{
    public class HomeWorkVm
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Subject { get; set; }
        public int ClassId { get; set; }
        public int? TeacherId { get; set; }
        public DateTime AssignDate { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class HomeWorkRequest
    {
        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(50)]
        public string? Subject { get; set; }

        public int ClassId { get; set; }

        public int? TeacherId { get; set; }

        public DateTime? DueDate { get; set; }
    }
}
