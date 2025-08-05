using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AinAlfahd.Models
{
    public class ShippingBatch
    {
        public int ShippingBatchId { get; set; }
        public DateTime? ShippingDate { get; set; }
        public DateTime? ArrivelDate { get; set; }
        public DateTime? EntryDate { get; set; }
        public decimal? batchCostUS { get; set; }   
        public string? Notes { get; set; }
        public decimal? SellingIQ { get; set; }
        public int? ReciptsNu { get; set; }
        
        [ForeignKey("ShippingTypes")]
        public int? ShippingTypeId { get; set; }
        public ShippingTypes? ShippingTypes { get; set; }
        [JsonIgnore]
        public List<Reciept>? Recipts { get; set; }
    }
}
