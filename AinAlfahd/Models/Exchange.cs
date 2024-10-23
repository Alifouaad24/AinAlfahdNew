using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class Exchange
{
    public int Id { get; set; }

    public string? ExchangeRate { get; set; }

    public string? OldRate { get; set; }

    public string? Managerno { get; set; }

    public string? BankRate { get; set; }
}
