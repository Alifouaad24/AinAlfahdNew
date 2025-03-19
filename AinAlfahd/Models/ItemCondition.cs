using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AinAlfahd.Models;

public partial class ItemCondition
{
    [Key]
    public int ConditionId { get; set; }

    public string? Description { get; set; }
}
