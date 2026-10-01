using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Schoolmangenment.Entities.Model;

[Table("Timetable")]
public class Timetable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int ClassId { get; set; }

    [ForeignKey(nameof(ClassId))]
    public SchoolClass? Class { get; set; }

    public int? TeacherId { get; set; }

    [ForeignKey(nameof(TeacherId))]
    public Teacher? Teacher { get; set; }

    [Required]
    [MaxLength(50)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    [MaxLength(15)]
    public string DayOfWeek { get; set; } = string.Empty;

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(100)]
    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    [MaxLength(100)]
    public string? ModifiedBy { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsDeleted { get; set; } = false;
}
