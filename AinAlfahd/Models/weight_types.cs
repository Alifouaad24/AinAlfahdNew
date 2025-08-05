using System.ComponentModel.DataAnnotations;

namespace AinAlfahd.Models
{
    public class weight_types
    {
        [Key]
        public int weight_type_id {  get; set; }
        public string description { get; set; }
    }
}
