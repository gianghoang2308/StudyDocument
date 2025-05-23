using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyDocument.Models;

public partial class User
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Username không được để trống.")]
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "Fullname không được để trống.")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Password không được để trống.")]
    public string Password { get; set; } = null!;

    public string? Avatar { get; set; }

    [Required(ErrorMessage = "Ngày sinh không được để trống.")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Địa chỉ không được để trống.")]
    public string Address { get; set; } = null!;

    [Required(ErrorMessage = "Email không được để trống.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Số điện thoại không được để trống.")]
    public string Phone { get; set; } = null!;

    [Required(ErrorMessage = "Số CCCD/CMND/CMT không được để trống.")]
    public string IdCard { get; set; } = null!;

    [Required(ErrorMessage = "Ngày cấp CCCD/CMND/CMT không được để trống.")]
    public DateOnly DateOfIssue { get; set; }

    [Required(ErrorMessage = "Nơi cấp CCCD/CMND/CMT không được để trống.")]
    public string PlaceOfIssue { get; set; } = null!;

    [Required(ErrorMessage = "Giới tính không được để trống.")]
    public bool Gender { get; set; }

    public bool Status { get; set; }

    public string? Image1 { get; set; }

    public string? Image2 { get; set; }

    public string? Image3 { get; set; }

    public DateTime CreateTime { get; set; }

    public int Role { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
