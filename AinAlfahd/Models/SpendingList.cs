using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class SpendingList
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public int? IsActive { get; set; }
}
