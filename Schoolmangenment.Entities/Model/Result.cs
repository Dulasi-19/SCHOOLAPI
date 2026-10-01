using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Schoolmangenment.Entities.Model;

[Table("Result")]
public class Result
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int ExamId { get; set; }

    [ForeignKey(nameof(ExamId))]
    public Exam? Exam { get; set; }

    public int StudentId { get; set; }

    [ForeignKey(nameof(StudentId))]
    public Student? Student { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal MarksObtained { get; set; }

    [MaxLength(10)]
    public string? Grade { get; set; }

    [MaxLength(250)]
    public string? Remarks { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(100)]
    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    [MaxLength(100)]
    public string? ModifiedBy { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsDeleted { get; set; } = false;
}
