using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyDocument.Models;

public partial class Image
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên phân loại danh mục được để trống.")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Tên không được để trống.")]
    public string? Name { get; set; }

    public string? Link { get; set; }

    public string? Image1 { get; set; }

    public string? ImageType { get; set; }

    public byte[]? ImageData { get; set; }

    public DateTime CreateTime { get; set; }

    public bool Status { get; set; }

    public int? UserId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
