using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class Service
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public ICollection<CustomerService> CustomerServices { get; set; }
}
