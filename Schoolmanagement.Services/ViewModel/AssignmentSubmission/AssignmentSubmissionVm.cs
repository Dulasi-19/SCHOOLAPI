using System;
using System.ComponentModel.DataAnnotations;

namespace Schoolmanagement.Services.ViewModel.AssignmentSubmission
{
    public class AssignmentSubmissionVm
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string? FileUrl { get; set; }
        public string? Content { get; set; }
        public string? Grade { get; set; }
        public string? Remarks { get; set; }
        public bool IsActive { get; set; }
    }

    public class AssignmentSubmissionRequest
    {
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }

        [MaxLength(500)]
        public string? FileUrl { get; set; }

        [MaxLength(1000)]
        public string? Content { get; set; }

        [MaxLength(10)]
        public string? Grade { get; set; }

        [MaxLength(250)]
        public string? Remarks { get; set; }
    }
}
