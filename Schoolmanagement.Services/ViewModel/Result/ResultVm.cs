using System;
using System.ComponentModel.DataAnnotations;

namespace Schoolmanagement.Services.ViewModel.Result
{
    public class ResultVm
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public decimal MarksObtained { get; set; }
        public string? Grade { get; set; }
        public string? Remarks { get; set; }
        public bool IsActive { get; set; }
    }

    public class ResultRequest
    {
        public int ExamId { get; set; }
        public int StudentId { get; set; }

        public decimal MarksObtained { get; set; }

        [MaxLength(10)]
        public string? Grade { get; set; }

        [MaxLength(250)]
        public string? Remarks { get; set; }
    }
}
