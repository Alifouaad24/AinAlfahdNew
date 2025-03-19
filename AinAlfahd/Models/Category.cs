
using AinAlfahd.Models_New;
using OpenQA.Selenium.DevTools.V85.SystemInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace AinAlfahd.Models;

public partial class Category
{
    [Key]
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public int? MainCategoryId { get; set; }

    public int? CategoryGroup { get; set; }
    [ForeignKey(nameof(Platform))]

    public int? Platform_id { get; set; }
    public Platform? Platform { get; set; }

    [JsonIgnore]
    public List<Item>? Items { get; set; }

    [JsonIgnore]
    public List<TblSize>? Sizes { get; set; }

}
