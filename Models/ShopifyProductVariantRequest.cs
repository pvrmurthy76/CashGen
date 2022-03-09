using Newtonsoft.Json;
using System;


namespace CashGen.Models
{
    internal class ShopifyProductVariantRequest
    {
        public Decimal price { get; set; }

        [JsonProperty]
        public Decimal? compare_at_price { get; set; }

        public int inventory_quantity { get; set; }

        public string inventory_management { get; set; }

        public string inventory_policy { get; set; }

        public bool taxable { get; set; }

        public string barcode { get; set; }

        public string sku { get; set; }
    }
}
