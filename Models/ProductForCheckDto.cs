using System;


namespace CashGen.Models
{
    public class ProductForCheckDto
    {
        public string Id { get; set; }

        public string Barcode { get; set; }

        public string Title { get; set; }

        public Decimal Price { get; set; }
    }
}
