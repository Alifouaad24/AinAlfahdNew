using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class Aliexbatch
{
    public int Id { get; set; }

    public DateTime? InsertDt { get; set; }

    public int? MerchantId { get; set; }

    public int? BatchStatus { get; set; }
}
