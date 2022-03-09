using System;

namespace CashGen.Models
{
    internal class ShopifyOrderLine
    {
        public long id { get; set; }

        public long? variant_id { get; set; }

        public string title { get; set; }

        public int quantity { get; set; }

        public string sku { get; set; }

        public Decimal price { get; set; }

        public long? product_id { get; set; }
    }
}
