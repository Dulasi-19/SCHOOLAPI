using System;
using System.ComponentModel.DataAnnotations;

namespace Schoolmanagement.Services.ViewModel.Teacher
{
    public class TeacherVm
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? EmployeeId { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Qualification { get; set; }
        public string? Specialization { get; set; }
        public int? UserId { get; set; }
        public bool IsActive { get; set; }
    }

    public class TeacherRequest
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? EmployeeId { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(100)]
        public string? Qualification { get; set; }

        [MaxLength(100)]
        public string? Specialization { get; set; }

        public int? UserId { get; set; }
    }
}
