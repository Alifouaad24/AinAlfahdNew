using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class ErrorLog
{
    public int Id { get; set; }

    public string? Descripton { get; set; }

    public DateOnly? InsertDate { get; set; }

    public string? Sqlqry { get; set; }
}
