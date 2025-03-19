using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AinAlfahd.Models;

public partial class Item
{
    public int Id { get; set; }

    [JsonIgnore]
    public Category? Category { get; set; }
    [ForeignKey("Category")]
    public int? CategoryId { get; set; }

    public string? EngName { get; set; }

    public string? PCode { get; set; }
    public string? OldCode { get; set; }

    [ForeignKey("MakeId")]
    public int? MakeId { get; set; }
    public Make? Make { get; set; }

    public string? ArDesc { get; set; }

    public int? H { get; set; }

    public int? W { get; set; }

    public int? L { get; set; }

    public int? Weight { get; set; }

    public string? Fr { get; set; }

    public int? SubId { get; set; }

    public int? Cost { get; set; }

    public int? Assymbly { get; set; }

    public string? ImgUrl { get; set; }

    public int? Comb { get; set; }

    public decimal? SitePrice { get; set; }

    public string? WebUrl { get; set; }

    public string? Model { get; set; }

    public string? Sku { get; set; }

    public string? Upc { get; set; }

    public string? InternetId { get; set; }


}
