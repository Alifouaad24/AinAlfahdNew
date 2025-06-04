using AinAlfahd.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AinAlfahd.Models_New;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class Inventory
{
    [Key]
    [Column("inventory_id")]
    public int InventoryId { get; set; }

    [ForeignKey(nameof(Item))]
    [Column("item_id")]
    public int? ItemId { get; set; }
    public Item? Item { get; set; }

    [Column("item_notes")]
    public string? ItemNotes { get; set; }

    [ForeignKey(nameof(ItemCondetion))]
    [Column("item_condition_id")]
    public int? ItemConditionId { get; set; }
    public ItemCondition? ItemCondetion { get; set; }

    [ForeignKey(nameof(Merchant))]
    public int? MerchantId { get; set; }
    public Merchant? Merchant { get; set; }

    [Column("sellingprice")]
    public decimal? SellingPrice { get; set; }

    [ForeignKey(nameof(Size))]
    [Column("sizeId")]
    public int? SizeId { get; set; }
    public TblSize? Size { get; set; }

    [Column("is_removed")]
    public int? IsRemoved { get; set; }

    [Column("insert_date")]
    public DateOnly? InsertDate { get; set; }

    [Column("insert_by")]
    public string? InsertBy { get; set; }

    [Column("test")]
    public string? Test { get; set; }

    [Column("order_id")]
    public int? OrderId { get; set; }
}

