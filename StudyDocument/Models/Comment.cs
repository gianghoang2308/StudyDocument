using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyDocument.Models;

public partial class Comment
{
    [Key]
    public int Id { get; set; }

    public int? ImageId { get; set; }

    public int? VideoId { get; set; }

    public int? DocumentId { get; set; }

    public int UserId { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Message { get; set; }

    public DateTime Date { get; set; }

    public bool? Status { get; set; }

    public virtual Document? Document { get; set; }

    public virtual Image? Image { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Video? Video { get; set; }
}
