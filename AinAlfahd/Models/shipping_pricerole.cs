using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AinAlfahd.Models
{
    public class shipping_pricerole
    {
        [Key]
        public int shipping_price_role_id { get; set; }

        /// //////////////////////////////////
        public ShippingTypes? ShippingTypes { get; set; } // جوي بحري الخ
        [ForeignKey(nameof(ShippingTypes))]
        public int shipping_type_id { get; set; }

        /// //////////////////////////////////

        public weight_cat? weight_cat { get; set; }  //  kg box cbm
        [ForeignKey(nameof(weight_cat))]
        public int shipping_cat_id { get; set; }

        /// //////////////////////////////////


        public decimal price { get; set; }

        /// //////////////////////////////////

        public Currency? Currency { get; set; } // iq usd
        [ForeignKey(nameof(Currency))]
        public int currency_id { get; set; }

        /// //////////////////////////////////

        public trade_types? trade_types { get; set; } // cost sell
        [ForeignKey(nameof(trade_types))]
        public int trade_type_id { get; set; }
    }
}
