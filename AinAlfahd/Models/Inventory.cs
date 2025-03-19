using AinAlfahd.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AinAlfahd.Models_New;

public partial class Inventory
{
    [Key]
    public int InventoryId { get; set; }

    [ForeignKey(nameof(Item))]
    public int? ItemId { get; set; }

    public Item? Item { get; set; }

    public string? ItemNotes { get; set; }

    [ForeignKey(nameof(ItemCondetion))]
    public int? ItemConditionId { get; set; }
    public ItemCondition? ItemCondetion { get; set; }


    public int? IsRemoved { get; set; }

    public DateOnly? InsertDate { get; set; }

    public string? InsertBy { get; set; }

    public int? OrderId { get; set; }
}
