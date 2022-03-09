using System;
using System.Collections.Generic;


namespace CashGen.Models
{
    internal class ShopifyOrder
    {
        public long id { get; set; }

        public string email { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public string token { get; set; }

        public int number { get; set; }

        public int order_number { get; set; }

        public Decimal total_price { get; set; }

        public string financial_status { get; set; }

        public string currency { get; set; }

        public string gateway { get; set; }

        public ShopifyOrderShipping shipping_address { get; set; }

        public ShopifyOrderCustomer customer { get; set; }

        public List<ShopifyOrderLine> line_items { get; set; }

        public List<ShopifyOrderAttribute> note_attributes { get; set; }

        public List<ShopifyOrderShippingLine> shipping_lines { get; set; }

        public string phone { get; set; }
    }
}
