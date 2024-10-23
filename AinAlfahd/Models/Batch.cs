using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class Batch
{
    public int Id { get; set; }

    public int? MerchantId { get; set; }

    public int? BatchStatus { get; set; }

    public string? BatchName { get; set; }

    public DateOnly? BatchShippingDate { get; set; }

    public int? ShippingCompanyId { get; set; }

    public string? CityId { get; set; }

    public int? Cbm { get; set; }

    public DateOnly? InsertDt { get; set; }

    public int? BatchUsvalue { get; set; }

    public int? Adjusted { get; set; }

    public int? Financed { get; set; }

    public DateOnly? FinanceDt { get; set; }
}
