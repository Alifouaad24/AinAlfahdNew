using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class Offlinetran
{
    public int Id { get; set; }

    public DateOnly? TransferDate { get; set; }

    public string? SqlQry { get; set; }

    public DateOnly? TranDate { get; set; }
}
