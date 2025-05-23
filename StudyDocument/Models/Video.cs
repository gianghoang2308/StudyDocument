using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyDocument.Models;

public partial class Video
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên phân loại danh mục không được để trống.")]
    public int CategoryId { get; set; }

    public string? Name { get; set; }

    public string? Link { get; set; }

    public string? Video1 { get; set; }

    public string? VideoType { get; set; }

    public byte[]? VideoData { get; set; }

    public int? UserId { get; set; }

    [Required(ErrorMessage = "Tiêu đề không được để trống.")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Mô tả không được để trống.")]
    public string? Description { get; set; }

    public bool Status { get; set; }

    public string? Thumbnail { get; set; }

    public int? Views { get; set; }

    public DateTime CreateTime { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
