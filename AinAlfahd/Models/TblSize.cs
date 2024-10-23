using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class TblSize
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public int? GroupIndex { get; set; }

    public int? OrderIndex { get; set; }
}
