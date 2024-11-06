namespace AinAlfahd.Models
{
    public class Reciept
    {
        public int RecieptId { get; set; }
        public decimal Weight { get; set; }
        public decimal Cost { get; set; }
        public string Currency { get; set; }
        public string InsertBy { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime RecieptDate { get; set; }
        public decimal? DisCount { get; set; }

        public decimal SellingPrice { get; set; }
        public decimal TotalPriceFromCust { get; set; }
        public string SellingCurrency { get; set; }
        public decimal? SellingDisCount { get; set; }
        public bool IsFinanced { get; set; }
        public bool CurrentState { get; set; }


        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
