using System.ComponentModel.DataAnnotations;

namespace AinAlfahd.Models
{
    public class Make
    {
        [Key]
        public int MakeId { get; set; }
        public string MakeDescription { get; set; }
    }
}
