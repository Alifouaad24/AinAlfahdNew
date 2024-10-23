using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class City
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public int? IsNorth { get; set; }
}
