using System;
using System.ComponentModel.DataAnnotations;

namespace Schoolmanagement.Services.ViewModel.Timetable
{
    public class TimetableVm
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public int? TeacherId { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string DayOfWeek { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsActive { get; set; }
    }

    public class TimetableRequest
    {
        public int ClassId { get; set; }

        public int? TeacherId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        [MaxLength(15)]
        public string DayOfWeek { get; set; } = string.Empty;

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }
}
