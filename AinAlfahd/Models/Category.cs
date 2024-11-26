using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AinAlfahd.Models;

public partial class Category
{
    [Key]
    public int Id { get; set; }
    public string? CategoryName { get; set; }
    public int? CategoryId { get; set; }
}
