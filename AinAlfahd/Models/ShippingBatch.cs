using System.ComponentModel.DataAnnotations.Schema;

namespace AinAlfahd.Models
{
    public class ShippingBatch
    {
        public int ShippingBatchId { get; set; }
        public DateTime? ShippingDate { get; set; }
        public DateTime? ArrivelDate { get; set; }
        public DateTime? EntryDate { get; set; }
        public decimal? batchCostUS { get; set; }    


        [ForeignKey("ShippingTypes")]
        public int? ShippingTypeId { get; set; }
        public ShippingTypes? ShippingTypes { get; set; }
    }
}
