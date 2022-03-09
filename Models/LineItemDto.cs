using System;


namespace CashGen.Models
{
    public class LineItemDto
    {
        public Guid Id { get; set; }

        public long variant_id { get; set; }

        public string title { get; set; }

        public int quantity { get; set; }

        public string sku { get; set; }

        public long product_id { get; set; }

        public string fulfilment { get; set; }

        public Decimal line_price { get; set; }
    }
}
