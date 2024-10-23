using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class AccountBox
{
    public int Id { get; set; }

    public int? Amount { get; set; }

    public int? TransactionType { get; set; }

    public DateOnly? TransactionDate { get; set; }

    public string? Notes { get; set; }

    public int? SpentCategory { get; set; }

    public int? ProductId { get; set; }

    public int? OrderNo { get; set; }

    public int? CustomerId { get; set; }

    public int? NotCollected { get; set; }

    public int? BatchId { get; set; }

    public int? IsDeleted { get; set; }
}
