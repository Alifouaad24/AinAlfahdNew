using System.ComponentModel.DataAnnotations;

namespace AinAlfahd.Models
{
    public class CustomerService
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
