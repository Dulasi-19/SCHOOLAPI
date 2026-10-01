using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Schoolmangenment.Entities.Model;

[Table("AssignmentSubmission")]
public class AssignmentSubmission
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int AssignmentId { get; set; }

    [ForeignKey(nameof(AssignmentId))]
    public Assignment? Assignment { get; set; }

    public int StudentId { get; set; }

    [ForeignKey(nameof(StudentId))]
    public Student? Student { get; set; }

    public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;

    [MaxLength(500)]
    public string? FileUrl { get; set; }

    [MaxLength(1000)]
    public string? Content { get; set; }

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
