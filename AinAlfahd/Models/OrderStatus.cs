using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class OrderStatus
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public int? Role { get; set; }
}
