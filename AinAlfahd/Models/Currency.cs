using System.ComponentModel.DataAnnotations;

namespace AinAlfahd.Models
{
    public class Currency
    {
        [Key]
        public int currency_id { get; set; }
        public string currency_name { get; set; }
    }
}
