using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AinAlfahd.Models
{
    public class Amendment_Log
    {
        [Key]
        public int Amendment_LogId { get; set; }

        [ForeignKey("Amendment")]
        public int AmendmentId { get; set; }
        public Amendment Amendments { get; set; }


        [ForeignKey("Customer")]
        public int CustomerIdId { get; set; }
        public Customer Customer { get; set; }

        public DateTime InsertDate { get; set; }
        public bool IsCompleted { get; set; }

        [ForeignKey("Service")]
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
