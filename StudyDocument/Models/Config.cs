using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyDocument.Models;

public partial class Config
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Mail không được để trống.")]
    public string? Mail { get; set; }

    public string? MailPort { get; set; }

    public string? MailInfo { get; set; }

    public string? MailNoreply { get; set; }

    public string? MailPassword { get; set; }

    public string? PlaceHead { get; set; }

    public string? PlaceBody { get; set; }

    public string? GoogleId { get; set; }

    [Required(ErrorMessage = "Contact không được để trống.")]
    public string? Contact { get; set; }

    [Required(ErrorMessage = "Copy right không được để trống.")]
    public string? Coppyright { get; set; }

    [Required(ErrorMessage = "Tiêu đề không được để trống.")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Mô tả không được để trống.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Keyword không được để trống.")]
    public string? Keyword { get; set; }

    public string? Lang { get; set; }

    [Required(ErrorMessage = "Hotline không được để trống.")]
    public string? Hotline { get; set; }

    [Required(ErrorMessage = "Link facebook không được để trống.")]
    public string? SocialLink1 { get; set; }

    [Required(ErrorMessage = "Link insta được để trống.")]
    public string? SocialLink2 { get; set; }

    [Required(ErrorMessage = "Link thread được để trống.")]
    public string? SocialLink3 { get; set; }

    [Required(ErrorMessage = "Link youtube được để trống.")]
    public string? SocialLink4 { get; set; }

    [Required(ErrorMessage = "Link github được để trống.")]
    public string? SocialLink5 { get; set; }

    [Required(ErrorMessage = "Link x được để trống.")]
    public string? SocialLink6 { get; set; }

    public string? LinkVideo1 { get; set; }

    public string? LinkVideo2 { get; set; }

    public string? LinkVideo3 { get; set; }

    public string? LinkVideo4 { get; set; }
}
