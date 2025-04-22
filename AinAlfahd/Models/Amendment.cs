using System.ComponentModel.DataAnnotations;

namespace AinAlfahd.Models
{
    public class Amendment
    {
        [Key]
        public int AmendmentId { get; set; }
        public string Description { get; set; }
    }
}
