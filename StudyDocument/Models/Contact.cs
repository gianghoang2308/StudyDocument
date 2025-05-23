using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyDocument.Models;

public partial class Contact
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên không được để trống.")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Email không được để trống.")]
    public string? Email { get; set; }

    public string? Address { get; set; }

    [Required(ErrorMessage = "Số điện thoại không được để trống.")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Nội dung tin nhắn không được để trống.")]
    public string? Message { get; set; }

    public bool? Status { get; set; }

    public DateTime CreateTime { get; set; }
}
