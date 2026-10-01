using System.ComponentModel.DataAnnotations;

namespace Schoolmanagement.Services.ViewModel.Class
{
    public class ClassVm
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Section { get; set; }
        public string? AcademicYear { get; set; }
        public int? ClassTeacherId { get; set; }
        public bool IsActive { get; set; }
    }

    public class ClassRequest
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Section { get; set; }

        [MaxLength(20)]
        public string? AcademicYear { get; set; }

        public int? ClassTeacherId { get; set; }
    }
}
