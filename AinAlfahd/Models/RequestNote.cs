using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AinAlfahd.Models
{
    public class RequestNote
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string Description { get; set; } 
        public int Status { get; set; } 
        public DateOnly RequestDate { get; set; } 
        public DateOnly InsertDt { get; set; } 
    }
}
