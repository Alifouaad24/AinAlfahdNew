using System.Text.Json.Serialization;

namespace AinAlfahd.Models
{
    public class CustomerShipping
    {
        public int CustomerShippingId { get; set; }
        public int CustomerId { get; set; }
        [JsonIgnore]
        public Customer? Customer { get; set; }

        public int ShippingTypeId { get; set; }
        [JsonIgnore]
        public ShippingTypes? ShippingType { get; set; }
    }
}
