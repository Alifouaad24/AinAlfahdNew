using System.ComponentModel.DataAnnotations;

namespace AinAlfahd.Models
{
    public class weight_cat
    {
        [Key]
        public int weight_category_id { get; set; }
        public string weight_category_name { get; set; }
    }
}
