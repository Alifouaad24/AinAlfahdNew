using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class ListPrice
{
    public int Id { get; set; }

    public int? SampleId { get; set; }

    public int? LabId { get; set; }

    public int? Pricing { get; set; }
}
