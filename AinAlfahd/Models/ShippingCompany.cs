using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AinAlfahd.Models;

public partial class ShippingCompany
{
    [Key]
    public int Id { get; set; }

    public string? Description { get; set; }
    public string? CurrencyType { get; set; }

    [ForeignKey(nameof(ShippingType))]
    public int? ShipingTypeId { get; set; }
    public ShippingTypes ShippingType { get; set; }

}
