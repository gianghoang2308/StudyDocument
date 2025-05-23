using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyDocument.Models;

public partial class Category
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên không được để trống.")]
    public string? Name { get; set; }

    public int? Type { get; set; }

    public string? Level { get; set; }

    public string? Tag { get; set; }

    [Required(ErrorMessage = "Tiêu đề không được để trống.")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Mô tả không được để trống.")]
    public string? Descrtiption { get; set; }

    [Required(ErrorMessage = "Keyword không được để trống.")]
    public string? Keyword { get; set; }

    public int? Ord { get; set; }

    public bool? Status { get; set; }

    [Required(ErrorMessage = "Ảnh không được để trống.")]
    public string? Image { get; set; }

    public int? Index { get; set; }

    public string? Lang { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
