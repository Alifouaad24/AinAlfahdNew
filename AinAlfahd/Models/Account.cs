using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class Account
{
    public int Id { get; set; }

    public string? AccountName { get; set; }

    public int? IsActive { get; set; }
}
