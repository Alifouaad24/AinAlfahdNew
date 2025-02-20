namespace AinAlfahd.Models
{
    public class OrderDetailsModel
    {
        public string? Sku {  get; set; }   
        public string? ImgUrl { get; set; }
        public int? MakeId { get; set; }
        public int? CategoryId { get; set; }
        public int? Size { get; set; }
        public decimal WebsitePrice { get; set; }
    }
}
