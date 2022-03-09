using System;


namespace CashGen.Models
{
    public class ProductListDto
    {
        public Guid Id { get; set; }

        public string Barcode { get; set; }

        public string Title { get; set; }

        public Decimal Price { get; set; }

        public string ProductType { get; set; }

        public string Brand { get; set; }

        public string Status { get; set; }

        public string Grade { get; set; }

        public string ConditionText { get; set; }

        public bool OnSale { get; set; }

        public string Handle { get; set; }

        public bool Uploading { get; set; }

        public int Quantity { get; set; }

        public Decimal WasPrice { get; set; }
    }
}
