using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string? CustName { get; set; }

    public string? CustMob { get; set; }

    public string? CustMob2 { get; set; }

    public int? CustCity { get; set; }

    public int? CustArea { get; set; }

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

    public ICollection<CustomerService>? CustomerServices { get; set; }
    public ICollection<CustomerShipping>? CustomerShipping { get; set; }
    public ICollection<Address> Addresses { get; set; }
}
