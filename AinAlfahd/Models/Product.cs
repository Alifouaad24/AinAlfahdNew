using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public int? CategoryId { get; set; }
}
