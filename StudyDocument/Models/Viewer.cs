using System;
using System.Collections.Generic;

namespace StudyDocument.Models;

public partial class Viewer
{
    public int Id { get; set; }

    public string? LocalIp { get; set; }

    public string? PublicIp { get; set; }

    public DateTime? AccessTime { get; set; }

    public string? UserAgent { get; set; }

    public int? UserId { get; set; }

    public string? Location { get; set; }

    public string? DeviceType { get; set; }

    public bool? Status { get; set; }

    public int? TotalAccessCountWeek { get; set; }

    public int? TotalAccessCountMonth { get; set; }

    public int? TotalAccessCountYear { get; set; }
}
