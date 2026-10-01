using System;
using System.ComponentModel.DataAnnotations;

namespace Schoolmanagement.Services.ViewModel.Exam
{
    public class ExamVm
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? ExamDate { get; set; }
        public string? Subject { get; set; }
        public int? MaxMarks { get; set; }
        public int? ClassId { get; set; }
        public bool IsActive { get; set; }
    }

    public class ExamRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public DateTime? ExamDate { get; set; }

        [MaxLength(50)]
        public string? Subject { get; set; }

        public int? MaxMarks { get; set; }

        public int? ClassId { get; set; }
    }
}
