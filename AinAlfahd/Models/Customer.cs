using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AinAlfahd.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string? CustName { get; set; }

    public string? CustMob { get; set; }

    public string? CustMob2 { get; set; }

    [ForeignKey(nameof(City))]
    public int? CustCity { get; set; }
    public City? City { get; set; }

    [ForeignKey(nameof(Area))]
    public int? CustArea { get; set; }
    public Area? Area { get; set; }

    public string? CustLandmark { get; set; }

    public DateOnly? InsertDt { get; set; }

    public int? CustStatus { get; set; }

    public string? Gisurl { get; set; }

    public string? Hexcode { get; set; }

    public string? Lat { get; set; }

    public float? Lon { get; set; }

    public string? Fbid { get; set; }

    public int? FullPackage { get; set; }

    public string? CustProfile { get; set; }


    [ForeignKey(nameof(Merchant))]
    public int? MerchantId { get; set; }
    public Merchant? Merchant { get; set; }


    public ICollection<CustomerService>? CustomerServices { get; set; }
    [JsonIgnore]

    public ICollection<CustomerShipping>? CustomerShipping { get; set; }
    public ICollection<Address> Addresses { get; set; }
}
