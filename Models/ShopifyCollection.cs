using System;


namespace CashGen.Models
{
    internal class ShopifyCollection
    {
        public long id { get; set; }

        public string handle { get; set; }

        public string title { get; set; }

        public Guid localId { get; set; }

        public Guid parentId { get; set; }
    }
}
