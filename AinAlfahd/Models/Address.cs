namespace AinAlfahd.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int Area { get; set; }
        public int City { get; set; }
        public string LandMark { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime InsertDate { get; set; }


        public Customer Customers { get; set; }
        public int CustomerId { get; set; }
    }
}
