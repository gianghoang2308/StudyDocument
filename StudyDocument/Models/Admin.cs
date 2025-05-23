using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyDocument.Models;

public partial class Admin
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Username không được để trống.")]
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "Fullname không được để trống.")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Avatar không được để trống.")]
    public string? Avatar { get; set; }

    [Required(ErrorMessage = "Password không được để trống.")]
    public string Password { get; set; } = null!;

    [DataType(DataType.DateTime)]
    [Required(ErrorMessage = "Ngày sinh không được để trống.")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Địa chỉ không được để trống.")]
    public string? Address { get; set; }

    [Required(ErrorMessage = "Email không được để trống.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Số điện thoại không được để trống.")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Số CCCD không được để trống.")]
    [StringLength(20)]
    public string IdCard { get; set; } = null!;

    [DataType(DataType.DateTime)]
    public DateTime DateOfIssue { get; set; }

    public string? PlaceOfIssue { get; set; }

    [Required(ErrorMessage = "Giới tính không được để trống.")]
    public bool Gender { get; set; }

    public int Role { get; set; }

    public bool Status { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreateTime { get; set; }
}
