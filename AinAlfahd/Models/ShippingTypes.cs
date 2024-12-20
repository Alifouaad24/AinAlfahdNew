using System.ComponentModel.DataAnnotations;

namespace AinAlfahd.Models
{
    public class ShippingTypes
    {
        [Key]
        public int ShippingTypeId { get; set; }
        public string? Description { get; set;}
    }
}
