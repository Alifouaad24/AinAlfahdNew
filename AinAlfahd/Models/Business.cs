using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class Business
{
    public int Id { get; set; }

    public string? BusinessName { get; set; }

    public string? BusinessPhone { get; set; }

    public string? Address { get; set; }

    public string? Address2 { get; set; }

    public string? City { get; set; }

    public int? StateCode { get; set; }

    public int? Zip { get; set; }

    public int? ActivityId { get; set; }

    public int? CustId { get; set; }
}
