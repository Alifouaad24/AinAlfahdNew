﻿using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class IncomeBox
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public int? TransactionType { get; set; }

    public int? Amount { get; set; }

    public DateOnly? InsertDt { get; set; }

    public int? Cleared { get; set; }

    public int? OrderNo { get; set; }

    public int? IsDeleted { get; set; }

    public int? MerchantId { get; set; }
}
