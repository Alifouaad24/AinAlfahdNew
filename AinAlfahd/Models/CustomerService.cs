using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AinAlfahd.Models
{
    public class CustomerService
    {
        public int CustomerId { get; set; }
        [JsonIgnore]
        public Customer Customer { get; set; }

        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
