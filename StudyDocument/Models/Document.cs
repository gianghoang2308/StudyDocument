using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyDocument.Models;

public partial class Document
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên phân loại danh mục được để trống.")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Tiêu đề không được để trống.")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Mô tả không được để trống.")]
    public string? Description { get; set; }

    public string? File { get; set; }

    public string? FileType { get; set; }

    public byte[]? FileData { get; set; }

    public DateTime CreateTime { get; set; }

    public bool? Status { get; set; }

    public int? UserId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
