using System;
using System.ComponentModel.DataAnnotations;


namespace CashGen.Entities
{
    public class LineItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public long variant_id { get; set; }

        public string title { get; set; }

        public int quantity { get; set; }

        public string sku { get; set; }

        public long product_id { get; set; }

        public Guid ProductKey { get; set; }

        public string fulfilment { get; set; }

        public Decimal line_price { get; set; }

        public long line_id { get; set; }
    }
}
