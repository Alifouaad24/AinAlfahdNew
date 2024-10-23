using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class Activity
{
    public int ActivityId { get; set; }

    public string? ActivityName { get; set; }

    public int? CategoryId { get; set; }
}
