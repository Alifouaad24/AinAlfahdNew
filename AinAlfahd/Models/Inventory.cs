using AinAlfahd.Models;
namespace AinAlfahd.Models_New;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class Inventory
{
    [Key]
    public int inventory_id { get; set; }

    [ForeignKey(nameof(Item))]
    public int? item_id { get; set; }
    public Item? Item { get; set; }

    public string? item_notes { get; set; }

    [ForeignKey(nameof(ItemCondetion))]
    public int? item_condition_id { get; set; }
    public ItemCondition? ItemCondetion { get; set; }

    [ForeignKey(nameof(Merchant))]
    public int? MerchantId { get; set; }
    public Merchant? Merchant { get; set; }

    public decimal? sellingprice { get; set; }

    [ForeignKey(nameof(Size))]
    public int? sizeId { get; set; }
    public TblSize? Size { get; set; }

    public int? is_removed { get; set; }

    public DateOnly? insert_date { get; set; }

    public string? insert_by { get; set; }

    public string? test { get; set; }

    public int? order_id { get; set; }

    public int? Qty { get; set; }
    public int? SystemId { get; set; }

    [ForeignKey(nameof(SystemId))]
    public Systemm Systemm { get; set; }
}

