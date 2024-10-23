using AinAlfahd.Models;

namespace AinAlfahd.ModelsDTO
{
    public class RecirptDto
    {
        public decimal Weight { get; set; }
        public decimal DisCount { get; set; }
        public int CustomerId { get; set; }

        public DateTime RecieptDate { get; set; }
        public decimal? SellingDisCount { get; set; }
        public DateTime FinanceDate { get; set; }
        public bool IsFinanced { get; set; }
    }
}
