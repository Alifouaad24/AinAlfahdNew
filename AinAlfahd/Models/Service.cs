using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AinAlfahd.Models;

public partial class Service
{
    public int Id { get; set; }

    public string? Description { get; set; }

    [JsonIgnore]
    public ICollection<CustomerService> CustomerServices { get; set; }
}
