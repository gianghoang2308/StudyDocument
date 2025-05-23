using System;
using System.Collections.Generic;

namespace StudyDocument.Models;

public partial class Advertise
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? ImageUrl { get; set; }

    public string? VideoUrl { get; set; }

    public string? Link { get; set; }

    public int? Position { get; set; }

    public bool? Status { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public DateTime CreateTime { get; set; }
}
