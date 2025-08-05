using System.ComponentModel.DataAnnotations;

namespace AinAlfahd.Models
{
    public class trade_types
    {
        [Key]
        public int trade_type_id { get; set; }
        public string description { get; set; }
    }
}
