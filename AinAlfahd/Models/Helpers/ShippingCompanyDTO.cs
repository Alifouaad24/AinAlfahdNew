using System.ComponentModel.DataAnnotations.Schema;

namespace AinAlfahd.Models.Helpers
{
    public class ShippingCompanyDTO
    {
        public string? Description { get; set; }
        public string? CurrencyType { get; set; }
        public int? ShipingTypeId { get; set; }
    }
}
