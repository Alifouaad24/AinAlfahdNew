using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AinAlfahd.Models
{
    public class ItemImages
    {
        [Key]
        public int ItemImagesId { get; set; }
        public string ImageLink { get; set; }

        [ForeignKey(nameof(Item))]
        public int ItemId { get; set; }
        [JsonIgnore]
        public Item Item { get; set; }
    }
}
