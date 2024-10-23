using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class Area
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public int? CityId { get; set; }

    public int? Sector { get; set; }

    public int? Zone { get; set; }

    public int? Spec { get; set; }
}
