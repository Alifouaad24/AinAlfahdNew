using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AinAlfahd.Models
{
    public class Package
    {
        [Key]
        public int PackageId { get; set; }

        public decimal ActualWeight { get; set; }
        public decimal DimentioalWeight { get; set; }

        public ShippingTypes? ShippingType { get; set; }

        [ForeignKey(nameof(ShippingType))]
        public int ShippingTypeId { get; set; }

        public decimal PurcheasCost { get; set; }

        [ForeignKey(nameof(CurrencyCostId))]
        public Currency? PurchaseCurrency { get; set; }
        public int? CurrencyCostId { get; set; }

        [ForeignKey(nameof(CurrencySaleId))]
        public Currency? SaleCurrency { get; set; }
        public int CurrencySaleId { get; set; }

        public Customer? Customer { get; set; }

        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }

        public decimal ActualWeightForCustomer { get; set; }
        public decimal SallingPrice { get; set; }
        public decimal SallingPriceIQ { get; set; }

        public string? ModifyBy { get; set; }
        public DateOnly ModifyOn { get; set; }
        public DateOnly PackageDt { get; set; }
    }

}
