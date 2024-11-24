using AinAlfahd.Models;

namespace AinAlfahd.ModelsDTO
{
    public class RecirptDto
    {
        public int RecieptId { get; set; }
        public decimal Weight { get; set; }
        public decimal Cost { get; set; }
        public decimal DisCount { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal TotalPriceFromCust { get; set; }
        public int CustomerId { get; set; }

        public DateTime RecieptDate { get; set; }
        public decimal? SellingDisCount { get; set; }
        public bool IsFinanced { get; set; }
        public bool CurrentState { get; set; }
        public string? Notes { get; set; }
    }
}
